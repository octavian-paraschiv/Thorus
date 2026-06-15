using FileUploader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusCommon.Export;
using ThorusCommon.IO;
using ThorusViewer.Extensions;
using ThorusViewer.Models;

namespace ThorusViewer.Forms
{
    public partial class MainForm : Form
    {
        private readonly ProgressForm _pf = new ProgressForm();
        private readonly JsonSerializerOptions _serializationOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

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

            try
            {
                ExportEngine.GenerateSubregionData((current, total, desc) => _pf.DisplayProgress(this, current, total, desc));
                if (MessageBox.Show(this, "Succesfully generated subregion data.\r\nDo you want to publish it, too?",
                    Constants.Product, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    MessageBox.Show(this, $"The data was generated and saved as:\r\n{exportDbPath}\r\nRemember this path in case you want to publish it manually to ocpa.ro website.");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to generate subregion data. Details: {ex.Message}",
                    Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            if (!File.Exists(exportDbPath))
            {
                MessageBox.Show(this, "Threre is nothing to publish yet.");
                return;
            }

            try
            {
                _pf.DisplayProgress(this, 0, -1, "Preparing to publish...");

                string baseUri = ConfigurationManager.AppSettings["apiBaseUri"].TrimEnd('/');
                string[] credentials = ConfigurationManager.AppSettings["apiCredentials"].Split(':');

                var dbLister = new RestUploader<object>(
                    requestUrl: $"{baseUri}/meteo/databases/all",
                    authUrl: $"{baseUri}/users/authenticate",
                    uploadData: new(),
                    loginId: credentials[0],
                    password: credentials[1],
                    useCompression: false);

                dbLister.Download(CancellationToken.None).ContinueWith(t =>
                {
                    try
                    {
                        List<MeteoDbInfo> databases = null;

                        try
                        {
                            databases = JsonSerializer.Deserialize<List<MeteoDbInfo>>(t.Result, options: _serializationOptions);
                        }
                        catch
                        {
                            // Not relevant
                        }

                        if (databases?.Count > 0)
                        {
                            var dlg = new SelectDatabaseDlg { Databases = databases.Where(mdi => mdi.Status != MeteoDbStatus.Online).ToList() };
                            var res = dlg.ShowDialog(this);
                            if (res == DialogResult.OK && dlg.SelectedDatabase != null)
                            {
                                var uploader = new FileUploader.FileUploader(
                                    requestUrl: $"{baseUri}/meteo/database/upload/{dlg.SelectedDatabase.Dbi}",
                                    authUrl: $"{baseUri}/users/authenticate",
                                    uploadFilePath: exportDbPath,
                                    loginId: credentials[0],
                                    password: credentials[1]);

                                uploader.FileUploadProgress += (x) =>
                                    Invoke(new MethodInvoker(() => _pf.DisplayProgress(this, (int)x, 100, "Publishing subregion data: ")));

                                uploader.Upload(CancellationToken.None).ContinueWith(t =>
                                {
                                    if (t?.Result?.Length > 0)
                                        MessageBox.Show(this, $"Failed to publish subregion data. {t.Result}",
                                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else
                                        MessageBox.Show(this, "Subregion data succesfully published.",
                                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    _pf.DisplayProgress(this, 0, 0, "");

                                }, TaskScheduler.FromCurrentSynchronizationContext());
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, $"Failed to query database information. {t.Result ?? string.Empty}".Trim(),
                                Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"Failed to query database information. {ex.Message ?? string.Empty}".Trim(" :".ToCharArray()),
                            Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    _pf.DisplayProgress(this, 0, 0, "");

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to publish subregion data. Details: {ex.Message}",
                    Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _simDlg?.BringToFront();
                e.Cancel = true;
            }
        }

        private static string GetBaseTitle()
        {
            SimDateTime snapshot = ControlPanelModel.Instance.SelectedSnapshot;
            if (snapshot != null)
                return string.Format("{0} [Data path: {1}], Snapshot: {2}",
                    Constants.Product, SimulationData.DataFolder, snapshot);

            return string.Format("{0} [Data path: {1}], No snapshot currently loaded.",
                Constants.Product, SimulationData.DataFolder);
        }

        private void tsmiGenerateAnimations_Click(object sender, EventArgs e)
        {
            string imageRoot = Path.Combine(SimulationData.WorkFolder, "image");

            var foldersWithPngs = Directory
                .EnumerateFiles(imageRoot, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .Select(Path.GetDirectoryName)
                .Where(d => d != null)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foldersWithPngs.ForEach(f =>
            {
                var title = f.Replace(imageRoot, string.Empty).Replace("\\", "_").Trim('_');
                var gifPath = Path.Combine(imageRoot, $"{title}.gif");
                GifBuilder.CreateGifFromFolder(f, gifPath, pauseBetweenLoopsMs: 2000, compressionLevel: GifCompressionLevel.Highest);
            });

            MessageBox.Show(this, "Animations succesfully generated.",
                Constants.Product, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
