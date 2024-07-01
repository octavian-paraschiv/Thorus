using MathNet.Numerics.LinearAlgebra.Single;
using OPMFileUploader;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using ThorusCommon;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.SQLite;
using ThorusViewer.WinForms;

namespace ThorusViewer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProgressForm _pf = new ProgressForm();

        public MainWindow()
        {
            InitializeComponent();

            App.ControlPanelModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);

            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            this.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);

            _pf.Hide();
            _pf.VisibleChanged += (s, e) => this.IsEnabled = !_pf.Visible;
        }

        void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mapView.RefitMap();
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            SelectWorkingFolder(false);
            SelectDataFolder();
        }

        void SelectWorkingFolder(bool reselect)
        {
        begin:
            if (reselect || string.IsNullOrEmpty(SimulationData.WorkFolder))
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = SimulationData.WorkFolder;

                dlg.Description = string.IsNullOrEmpty(SimulationData.WorkFolder) ?
                    $"Please select the global working folder (where the simulation engine will write its files)" :
                    $"Please select the global working folder (current value: {SimulationData.WorkFolder ?? ""})\r\n" +
                    $"Click Cancel or hit Escape to leave it as-is.";

                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    SimulationData.WorkFolder = dlg.SelectedPath;
            }

            if (string.IsNullOrEmpty(SimulationData.WorkFolder))
            {
                if (System.Windows.MessageBox.Show("The application cannot continue without a working folder.\r\n" +
                    "Do you want to try again? If you choose NO, the application will exit.",
                    Constants.Product, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    goto begin;

                Process.GetCurrentProcess().Kill();
            }
        }

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedSnapshot":
                    this.Title = GetBaseTitle();
                    break;

                case "AutoSave":
                    mapView.SaveImage(true);
                    break;
            }
        }

        private void mnuLoadDataSet_Click(object sender, RoutedEventArgs e)
        {
            SelectDataFolder();
        }

        public void SelectDataFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = SimulationData.DataFolder;
            dlg.Description = $"Please select the dataset root path (current value: {SimulationData.DataFolder ?? ""})\r\n" +
                $"Click Cancel or hit Escape to leave it as-is.";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                App.ControlPanelModel.SelectedCategory = "";
                SimulationData.SetNewDataFolder(dlg.SelectedPath);
            }

            mnuSimulation.IsEnabled = SimulationData.IsDefaultDataFolder;
        }

        private void mnuSaveAsImage_Click(object sender, RoutedEventArgs e)
        {
            mapView.SaveImage(false);
        }

        private void mnuPublish_Click(object sender, RoutedEventArgs e)
        {
            string exportDbPath = Path.Combine(Directory.GetParent(SimulationData.DataFolder).FullName, "Snapshot.db3");

            if (!File.Exists(exportDbPath))
            {
                System.Windows.MessageBox.Show("Threre is nothing to publish yet.");
                return;
            }

            try
            {
                _pf.DisplayProgress(0, -1, "Preparing to publish...");

                string baseUri = ConfigurationManager.AppSettings["apiBaseUri"].TrimEnd('/');
                string[] credentials = ConfigurationManager.AppSettings["apiCredentials"].Split(':');

                var uploader = new FileUploader(
                    uploadUrl: $"{baseUri}/meteo/uploadDatabase",
                    authUrl: $"{baseUri}/users/authenticate",
                    uploadFilePath: exportDbPath,
                    loginId: credentials[0],
                    password: credentials[1]);

                uploader.FileUploadProgress += (x) =>
                    System.Windows.Application.Current.Dispatcher.Invoke(() => _pf.DisplayProgress((int)x, 100, "Publishing subregion data: "));

                uploader.Run().ContinueWith(t =>
                {
                    _pf.DisplayProgress(0, 0, "");

                    if (t?.Result?.Length > 0)
                        System.Windows.MessageBox.Show($"Upload failed. Details: {t.Result}",
                            "", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        System.Windows.MessageBox.Show("Data succesfully published.", "", MessageBoxButton.OK, MessageBoxImage.Information);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                _pf.DisplayProgress(0, 0, "");
                System.Windows.MessageBox.Show($"Upload failed. Details: {ex.Message}",
                            "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void mnuSubmatrix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _pf.DisplayProgress(0, -1, "Preparing to generate subregion data...");

                string exportDbPath = Path.Combine(Directory.GetParent(SimulationData.DataFolder).FullName, "Snapshot.db3");

                MeteoDB exportDb = MeteoDB.OpenOrCreate(exportDbPath, true);

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

                            _pf.DisplayProgress(step++, count, "Generating subregion data...");
                        }
                    }
                }

                _pf.DisplayProgress(0, -1, "Saving subregion data...");

                exportDb.SaveAndClose();

                _pf.DisplayProgress(0, 0, "");

                System.Windows.MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                _pf.DisplayProgress(0, 0, "");
            }
        }

        private void mnuAutoSave_Click(object sender, RoutedEventArgs e)
        {
            bool x = App.ControlPanelModel.AutoSaveImage;
            App.ControlPanelModel.AutoSaveImage = !x;
        }

        private void mnuGlobalSettings_Click(object sender, RoutedEventArgs e)
        {
            SelectWorkingFolder(true);
        }


        SimControlPanel _simDlg = null;

        private void mnuSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (_simDlg == null)
            {
                _simDlg = new SimControlPanel();
                _simDlg.FormClosed += new FormClosedEventHandler(_simDlg_FormClosed);
                _simDlg.Show();
            }

            _simDlg.BringToFront();
        }

        void _simDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            _simDlg = null;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_simDlg != null || _pf.Visible)
            {
                System.Windows.MessageBox.Show(this, "Please close all open windows before exiting the application.");
                _simDlg.BringToFront();
                e.Cancel = true;
            }
        }

        private static string GetHMACSHA1Hash(string input, string key)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (var hmac = new HMACSHA1(keyBytes))
            {
                var hash = hmac.ComputeHash(inputBytes);
                return Convert.ToBase64String(hash);
            }
        }

        private string GetBaseTitle()
        {
            SimDateTime snapshot = App.ControlPanelModel.SelectedSnapshot;
            if (snapshot != null)
                return string.Format("{0} [Data path: {1}], Snapshot: {2}",
                    App.AppName, SimulationData.DataFolder, snapshot);

            return string.Format("{0} [Data path: {1}], No snapshot currently loaded.",
                App.AppName, SimulationData.DataFolder);
        }
    }
}
