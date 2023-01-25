using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;
using ThorusViewer.Palettes;
using ThorusCommon.Engine;
using System.Windows.Forms;
using System.Windows.Interop;
using ThorusViewer.Models;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon;
using Application = System.Windows.Forms.Application;
using ThorusCommon.SQLite;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.IO.Compression;
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
            SelectWorkingFolder();
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
            SelectWorkingFolder();
        }

        public void SelectWorkingFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Please select the dataset root path:";
            dlg.SelectedPath = SimulationData.DataFolder;
            
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

            int i = 0, totalParts = 0;

            try
            {
                _pf.DisplayProgress(0, -1, "Preparing to publish...");

                string base64 = null;

                using (MemoryStream output = new MemoryStream())
                using (GZipStream gzip = new GZipStream(output, CompressionLevel.Optimal))
                {
                    var inData = File.ReadAllBytes(exportDbPath);

                    gzip.Write(inData, 0, inData.Length);
                    gzip.Close();

                    var outData = output.ToArray();
                    base64 = Convert.ToBase64String(outData);
                }

                string baseUri = ConfigurationManager.AppSettings["apiBaseUri"].TrimEnd('/');
                string credentials = ConfigurationManager.AppSettings["apiCredentials"];

                const int desiredChunkCount = 512;
                int uploadChunkSize = (int)Math.Ceiling((double)base64.Length / desiredChunkCount);

                // May be different than 512 but it should be of same order of magnitude
                totalParts = (int)Math.Ceiling((double)base64.Length / (double)uploadChunkSize); 

                int retries = 0;

                for (; i < totalParts; i++)
                {
                    string chunk = base64.Substring(i * uploadChunkSize, Math.Min(base64.Length - i * uploadChunkSize, uploadChunkSize));
                    var req = new UploadDataPart
                    {
                        PartBase64 = chunk,
                        PartIndex = i,
                        TotalParts = totalParts
                    };

                    var body = JsonConvert.SerializeObject(req);
                    var requestResource = "/meteo/uploadPart";
                    string contentType = "application/json";
                    string contentHash = string.Empty;
                    string credentialsHash = string.Empty;

                    using (SHA1 sha1 = SHA1.Create())
                    {
                        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(body));
                        contentHash = Convert.ToBase64String(hash);

                        hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(credentials));
                        credentialsHash = Convert.ToBase64String(hash);
                    }

                    using (HttpClient cl = new HttpClient())
                    {
                        cl.Timeout = TimeSpan.FromMinutes(10);
                        
                        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                        cl.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", credentialsHash);

                        var seed = cl.GetStringAsync($"{baseUri}/token/seed").Result;

                        // After getting the token seed we have a window of 5 min in which we should 
                        // be sending the current chunk (and getting the OK back), before the token expires

                        var content = new StringContent(body);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                        var stringToSign =
                            $"POST\n" +
                            $"{contentHash}\n" +
                            $"{contentType}\n" +
                            $"{timestamp}\n" +
                            $"{requestResource}";

                        var calcSignature = GetHMACSHA1Hash(stringToSign, seed);

                        cl.DefaultRequestHeaders.TryAddWithoutValidation("X-Signature", calcSignature);
                        cl.DefaultRequestHeaders.TryAddWithoutValidation("X-Date", timestamp);
                        cl.DefaultRequestHeaders.TryAddWithoutValidation("X-Request-Id", seed);

                        var x = cl.PostAsync($"{baseUri}{requestResource}", content).Result;

                        if (x.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            retries = 0;
                        }
                        else
                        {
                            retries++;

                            if (retries < 10)
                            {
                                i--; // Remain on same chunk and retry upload
                                continue;
                            }
                            else
                                throw new Exception(x.Content?.ReadAsStringAsync().Result);
                        }

                        _pf.DisplayProgress((i + 1), totalParts, "Publishing subregion data: ");
                    }
                }
                System.Windows.MessageBox.Show("Upload done");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed upload at chunk {i} of {totalParts}. Details: {ex.Message}");
            }
            finally
            {
                _pf.DisplayProgress(0, 0, "");
            }
        }

        private void mnuSubmatrix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _pf.DisplayProgress(0, -1, "Preparing to generate subregion data...");

                string exportDbPath = Path.Combine(Directory.GetParent(SimulationData.DataFolder).FullName, "Snapshot.db3");

                //if (File.Exists(exportDbPath))
                //  File.Delete(exportDbPath);

                MeteoDB exportDb = MeteoDB.OpenOrCreate(exportDbPath, true);

                // Clean up DB in case already used eg. by local web site
                exportDb.PurgeAllData();

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
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = SimulationData.WorkFolder;
            dlg.Description = "Please select the global working folder.\r\nNote: The simulation engine will write the data here.";
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SimulationData.WorkFolder = dlg.SelectedPath;
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

    public class UploadDataPart
    {
        public string PartBase64 { get; set; }
        public int PartIndex { get; set; }
        public int TotalParts { get; set; }
    }
}
