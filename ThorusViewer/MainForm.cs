using OPMFileUploader;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusCommon.Export;
using ThorusCommon.IO;
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
                    uploadUrl: $"{baseUri}/meteo/database/preview",
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
                        MessageBox.Show($"Failed to publish subregion data. {t.Result}",
                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Subregion data succesfully published.",
                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                _pf.DisplayProgress(this, 0, 0, "");
                MessageBox.Show($"Failed to publish subregion data. Details: {ex.Message}",
                    Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnGenerateSubregionData(object sender, EventArgs e)
        {
            try
            {
                ExportEngine.GenerateSubregionData((current, total, desc) => _pf.DisplayProgress(this, current, total, desc));
                MessageBox.Show("Succesfully generated subregion data.",
                    Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate subregion data. Details: {ex.Message}",
                    Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _simDlg?.BringToFront();
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
