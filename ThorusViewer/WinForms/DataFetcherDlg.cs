using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThorusCommon.Data;
using ThorusCommon.Engine;

namespace ThorusViewer.WinForms
{
    public partial class DataFetcherDlg : Form
    {
        private System.Windows.Forms.Timer _tmrCheckFiles = null;
        private ManualResetEvent _downloadEmails = new ManualResetEvent(false);
        private ManualResetEvent _abort = new ManualResetEvent(false);

        private HttpClient CreateNewClient()
        {
            return new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(60)
            };
        }

        public DataFetcherDlg()
        {
            InitializeComponent();

            btnAbort.Visible = true;
            btnDone.Visible = false;

            this.Shown += (s1, e1) =>
            {
                ValidateInitialConditionFiles();
            };
        }

        private void btnFetchSstData_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this, "This will delete any existing initial condition files. Are you sure you want to proceed ?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes)
                return;

            DateTime selDate = NetCdfImporter.ImportDateTime("SST.NC");
            Log($"Initial condition download started for date: {selDate:yyyy-MM-dd}");

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    DeleteClientSideData();
                    // await FetchSstFile(selDate).ConfigureAwait(false);
                    await FetchGribFile().ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    Log("Could not download all required files, although waited for 1 hour.");
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            }).ContinueWith(_ =>
            {
                if (ValidateFiles())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            });
        }

        private void DeleteClientSideData()
        {
            if (_abort.WaitOne(0))
                return;

            Process[] procs = Process.GetProcesses();
            var simProc = (from p in procs
                           where p.ProcessName.ToLowerInvariant().Contains("thorussimulation")
                           select p).FirstOrDefault();

            if (simProc != null)
                simProc.Kill();

            // FileDelete("SST.nc");
            FileDelete("input.grib");

            Log($"Client side data deleted...");

            if (this.InvokeRequired)
                Invoke(new MethodInvoker(ValidateInitialConditionFiles));
            else
                ValidateInitialConditionFiles();
        }

        /*
        private async Task FetchSstFile(DateTime selDate)
        {
            try
            {
                if (_abort.WaitOne(0))
                    return;

                string sstRequestUrlFmt = ConfigurationManager.AppSettings["noaaSstGetUrl"];
                if (string.IsNullOrEmpty(sstRequestUrlFmt) == false)
                {
                    sstRequestUrlFmt = sstRequestUrlFmt
                        .Replace("##YEAR##", selDate.Year.ToString())
                        .Replace("##MONTH##", selDate.Month.ToString("d2"))
                        .Replace("##DAY##", selDate.Day.ToString("d2"));

                    Log($"Requesting SST data from: {sstRequestUrlFmt}");

                    if (_abort.WaitOne(0))
                        return;

                    byte[] data = null;

                    using (HttpClient cl = CreateNewClient())
                    {
                        data = await cl.GetByteArrayAsync(sstRequestUrlFmt).ConfigureAwait(false);
                    }

                    if (_abort.WaitOne(0))
                        return;

                    if (data?.Length > 0)
                    {
                        string file = Path.Combine(SimulationData.WorkFolder, "SST.nc");

                        if (File.Exists(file))
                            File.Delete(file);

                        File.WriteAllBytes(file, data);
                    }
                }
            }
            catch(Exception ex)
            {
                Log(ex.Message);
            }
        }
        */

        private async Task FetchGribFile()
        {
            try
            {
                if (_abort.WaitOne(0))
                    return;

                if (FileExists("SST.NC"))
                {
                    DateTime dtSst = NetCdfImporter.ImportDateTime("SST.NC");
                    string url = ConfigurationManager.AppSettings["noaaAwsGetUrl"];
                    url = url.Replace("##DATETIME_SST##", $"{dtSst:yyyyMMdd}");
                    Log($"Downloading GRIB data from: {url}");

                    if (_abort.WaitOne(0))
                        return;

                    byte[] data = null;

                    using (HttpClient cl = CreateNewClient())
                    {
                        data = await cl.GetByteArrayAsync(url).ConfigureAwait(false);
                    }

                    if (_abort.WaitOne(0))
                        return;

                    if (data?.Length > 0)
                    {
                        string file = Path.Combine(SimulationData.WorkFolder, "input.grib");

                        if (File.Exists(file))
                            File.Delete(file);

                        File.WriteAllBytes(file, data);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void ValidateInitialConditionFiles()
        {
            if (this.DesignMode)
                return;

            if (_tmrCheckFiles == null)
            {
                _tmrCheckFiles = new System.Windows.Forms.Timer();
                _tmrCheckFiles.Interval = 10000;
                _tmrCheckFiles.Tick += (s2, e2) =>
                {
                    ValidateInitialConditionFiles();
                };
            }

            bool allFilesPresent = true;

            try
            {
                _tmrCheckFiles.Stop();

                DateTime selDate = NetCdfImporter.ImportDateTime("SST.NC");
                label1.Text = $"SST date: {selDate:yyyy-MM-dd}";

                allFilesPresent = ValidateFiles();

                btnAbort.Visible = !allFilesPresent;
                btnDone.Visible = allFilesPresent;

                if (allFilesPresent)
                {
                    Log($"**********************************************************************");
                    Log($"Initial condition files are present.");
                    Log($"You can press Done to exit this dialog, or press Start to get a new set of files.");
                    Log($"**********************************************************************");
                }
            }
            finally
            {
                if (!allFilesPresent)
                    _tmrCheckFiles.Start();
            }
        }

        private bool ValidateFiles()
        {
            bool allFilesPresent = true;

            pbSST.Image = ValidateFile("SST.nc", ref allFilesPresent);
            pbGrib.Image = ValidateFile("input.grib", ref allFilesPresent);
            return allFilesPresent;
        }

        private Image ValidateFile(string file, ref bool canSimulate)
        {
            bool fileExists = FileExists(file);

            canSimulate &= fileExists;

            return fileExists ? Resources.OK : Resources.NG;
        }

        private bool FileExists(string file)
        {
            string path = Path.Combine(SimulationData.WorkFolder, file);
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Exists && fileInfo.Length > 0;
        }

        private void FileDelete(string file)
        {
            Log($"Deleting: {file}");

            string path = Path.Combine(SimulationData.WorkFolder, file);
            File.Delete(path);
        }

        private delegate void LogDG(string msg, params object[] args);

        private void Log(string msg, params object[] args)
        {
            if (InvokeRequired)
            {
                this.Invoke(new LogDG(Log), msg, args);
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    txtSimProcOut.Clear();
                    return;
                }

                string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {string.Format(msg, args)}";

                List<string> lines = txtSimProcOut.Lines.ToList();
                lines.Add(line);
                txtSimProcOut.Lines = lines.ToArray();

                txtSimProcOut.SelectionStart = txtSimProcOut.TextLength;
                txtSimProcOut.ScrollToCaret();
            }
            catch (Exception ex)
            {
                txtSimProcOut.Text = ex.Message;
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            _abort.Set();
        }
    }
}