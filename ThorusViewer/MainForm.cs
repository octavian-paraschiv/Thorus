using MathNet.Numerics.LinearAlgebra.Single;
using OPMFileUploader;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThorusCommon;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.SQLite;
using ThorusViewer.Models;

namespace ThorusViewer.Forms
{
    public partial class MainForm : Form
    {
        private readonly ProgressForm _pf = new ProgressForm();

        public MainForm()
        {
            InitializeComponent();

            this.Icon = Resources.WeatherStudio;
            this.ShowIcon = true;

            ControlPanelModel.Instance.PropertyChanged += ControlPanelModel_PropertyChanged;

            this.Shown += (s, e) => tsmiLaunchSimulation.Enabled = SimulationData.IsDefaultDataFolder;
            this.SizeChanged += OnSizeChanged;
            this.Closing += OnClosing;

            _pf.Hide();
            _pf.VisibleChanged += (s, e) => this.Enabled = !_pf.Visible;
        }

        void OnSizeChanged(object sender, EventArgs e)
        {
            mapView.RefitMap();
        }

        

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedSnapshot":
                    this.Text = GetBaseTitle();
                    break;

                case "AutoSave":
                    mapView.SaveImage(true);
                    break;
            }
        }

        private void OnLoadDataSet(object sender, EventArgs e)
        {
            SimulationDataUtility.SelectDataFolder();
        }

        private void OnSaveAsImage(object sender, EventArgs e)
        {
            mapView.SaveImage(false);
        }

        private void OnPublish(object sender, EventArgs e)
        {
            string exportDbPath = Path.Combine(Directory.GetParent(SimulationData.DataFolder).FullName, "Snapshot.db3");

            if (!File.Exists(exportDbPath))
            {
                MessageBox.Show("Threre is nothing to publish yet.");
                return;
            }

            try
            {
                _pf.DisplayProgress(this, 0, -1, "Preparing to publish...");

                string baseUri = ConfigurationManager.AppSettings["apiBaseUri"].TrimEnd('/');
                string[] credentials = ConfigurationManager.AppSettings["apiCredentials"].Split(':');

                var uploader = new FileUploader(
                    uploadUrl: $"{baseUri}/meteo/uploadDatabase",
                    authUrl: $"{baseUri}/users/authenticate",
                    uploadFilePath: exportDbPath,
                    loginId: credentials[0],
                    password: credentials[1]);

                uploader.FileUploadProgress += (x) =>
                    Invoke(new MethodInvoker(() => _pf.DisplayProgress(this, (int)x, 100, "Publishing subregion data: ")));

                uploader.Run().ContinueWith(t =>
                {
                    _pf.DisplayProgress(this, 0, 0, "");

                    if (t?.Result?.Length > 0)
                        MessageBox.Show($"Upload failed. Details: {t.Result}",
                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Data succesfully published.",
                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                _pf.DisplayProgress(this, 0, 0, "");
                MessageBox.Show($"Upload failed. Details: {ex.Message}",
                    Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnGenerateSubregionData(object sender, EventArgs e)
        {
            try
            {
                _pf.DisplayProgress(this, 0, -1, "Preparing to generate subregion data...");

                string exportDbPath = Path.Combine(Directory.GetParent(SimulationData.DataFolder).FullName, "Snapshot.db3");

                MeteoDB exportDb = null;
                try
                {
                    File.Copy("Data/Template.db3", "./Template.db3");
                    exportDb = MeteoDB.OpenOrCreate(exportDbPath, true);
                }
                finally
                {
                    File.Delete("./Template.db3");
                }

                // Clean up DB in case already used eg. by local web site
                exportDb.PurgeAll<Data>();

                var allFiles = Directory.GetFiles(SimulationData.DataFolder);
                if (allFiles != null)
                {
                    int count = 0;
                    int step = 0;

                    foreach (string file in allFiles)
                    {
                        string title = Path.GetFileNameWithoutExtension(file).ToUpperInvariant();
                        string type = title.Substring(0, 4);

                        switch (type)
                        {
                            case "C_00":
                            case "L_00":
                            case "N_00":
                            case "T_01":
                            case "T_SH":
                            case "T_SL":
                            case "T_TE":
                            case "T_TS":
                            case "F_SI":
                            case "T_NL":
                            case "T_NH":
                            case "R_00":
                            case "R_DD":
                            case "N_DD":
                            case "P_00":
                            case "P_01":
                                count += exportDb.Regions.Count;
                                break;

                            default:
                                continue;
                        }
                    }

                    foreach (var region in exportDb.Regions)
                    {
                        foreach (string file in allFiles)
                        {
                            string title = Path.GetFileNameWithoutExtension(file).ToUpperInvariant();
                            string type = title.Substring(0, 4);
                            string timestamp = title.Substring(9, 10);

                            DenseMatrix rawMatrix = FileSupport.LoadSubMatrixFromFile(file, region.MinLon, region.MaxLon, region.MinLat, region.MaxLat);
                            bool interpolate = (rawMatrix.RowCount < 10);
                            DenseMatrix output = interpolate ? rawMatrix.Interpolate() : rawMatrix;

                            bool isWindMap;

                            switch (type)
                            {
                                case "C_00":
                                case "L_00":
                                case "N_00":
                                case "T_01":
                                case "T_SH":
                                case "T_SL":
                                case "T_TE":
                                case "T_TS":
                                case "F_SI":
                                case "T_NL":
                                case "T_NH":
                                case "R_00":
                                case "R_DD":
                                case "N_DD":
                                    isWindMap = false;
                                    break;

                                case "P_00":
                                case "P_01":
                                    isWindMap = true;
                                    break;

                                default:
                                    continue;
                            }

                            exportDb.AddMatrix(region.Id, timestamp, type, output);

                            if (isWindMap)
                            {
                                var wind = output.ToWindComponents();
                                var module = DenseMatrix.Create(output.RowCount, output.ColumnCount, 0);
                                var angle = DenseMatrix.Create(output.RowCount, output.ColumnCount, 0);

                                for (int r = 0; r < output.RowCount; r++)
                                {
                                    for (int c = 0; c < output.ColumnCount; c++)
                                    {
                                        var wx = wind[Direction.X][r, c];
                                        var wy = wind[Direction.Y][r, c];

                                        var ang = Math.Atan2(-wy, wx);
                                        if (ang < 0)
                                            ang += 2 * Math.PI;

                                        module[r, c] = (float)Math.Sqrt(wx * wx + wy * wy);
                                        angle[r, c] = (float)ang;
                                    }
                                }

                                string mType = type.Replace("P_0", "W_0");
                                string aType = type.Replace("P_0", "W_1");
                                exportDb.AddMatrix(region.Id, timestamp, mType, module);
                                exportDb.AddMatrix(region.Id, timestamp, aType, angle);
                            }

                            _pf.DisplayProgress(this, step++, count, "Generating subregion data...");
                        }
                    }
                }

                _pf.DisplayProgress(this, 0, -1, "Saving subregion data...");

                exportDb.SaveAndClose();

                _pf.DisplayProgress(this, 0, 0, "");

                MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _pf.DisplayProgress(this, 0, 0, "");
            }
        }

        private void OnToggleAutoSave(object sender, EventArgs e)
        {
            bool x = ControlPanelModel.Instance.AutoSaveImage;
            ControlPanelModel.Instance.AutoSaveImage = !x;
        }

        private void OnGlobalSettings(object sender, EventArgs e)
        {
            SimulationDataUtility.SelectWorkingFolder(true);
        }


        SimControlPanel _simDlg = null;

        private void OnSimulation(object sender, EventArgs e)
        {
            if (_simDlg == null)
            {
                _simDlg = new SimControlPanel();
                _simDlg.FormClosed += (ss, ee) => _simDlg = null;
                _simDlg.Show(this);
            }

            _simDlg.BringToFront();
        }

        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_simDlg != null || _pf.Visible)
            {
                MessageBox.Show(this, "Please close all open windows before exiting the application.");
                _simDlg.BringToFront();
                e.Cancel = true;
            }
        }

        private string GetBaseTitle()
        {
            SimDateTime snapshot = ControlPanelModel.Instance.SelectedSnapshot;
            if (snapshot != null)
                return string.Format("{0} [Data path: {1}], Snapshot: {2}",
                    Constants.Product, SimulationData.DataFolder, snapshot);

            return string.Format("{0} [Data path: {1}], No snapshot currently loaded.",
                Constants.Product, SimulationData.DataFolder);
        }
    }
}
