using Majestic12;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        private HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(60),
        };

        public DataFetcherDlg()
        {
            InitializeComponent();

            btnAbort.Visible = true;
            btnDone.Visible = false;

            this.Shown += (s1, e1) =>
            {
                PopulateSstDates();
                ValidateInitialConditionFiles();
            };
        }


        private void PopulateSstDates()
        {
            DateTime dtEnd = DateTime.Today;
            DateTime dtStart = DateTime.Parse("2000-01-01");
            object sel = -1;

            List<DateTime> ldt = new List<DateTime>();

            bool skip = true;

            for (DateTime dt = dtEnd; dt >= dtStart; dt = dt.AddDays(-1))
            {
                if (dt.DayOfWeek != DayOfWeek.Sunday)
                    continue;

                if (skip)
                {
                    skip = false;
                    continue;
                }

                ldt.Add(dt);
            }

            cmbSstDate.DataSource = ldt;
            cmbSstDate.SelectedIndex = 0;
            cmbSstDate.Format += CmbSstDate_Format;
        }

        private void CmbSstDate_Format(object sender, ListControlConvertEventArgs e)
        {
            var dt = (DateTime)e.ListItem;
            e.Value = dt.ToString("yyyy-MM-dd");
        }

        private void btnFetchSstData_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this, "This will delete any existing initial condition files. Are you sure you want to proceed ?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes)
                return;

            DateTime selDate = (DateTime)cmbSstDate.SelectedItem;



            Log($"Initial condition download started for date: {selDate:yyyy-MM-dd}");

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    DeleteClientSideData();
                    await FetchSstFile(selDate).ConfigureAwait(false);
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

        private async Task FetchSstFile(DateTime selDate)
        {
            try
            {
                if (_abort.WaitOne(0))
                    return;

                string vid = txtVID.Text;
                string did = txtDID.Text;
                string tid = txtTID.Text;

                string vid_did_tid = $"DB_tid={tid}&DB_did={did}&DB_vid={vid}";

                Log($"Using: {vid_did_tid}");

                string sstRequestUrlFmt = ConfigurationManager.AppSettings["noaaSstGetUrl"];
                if (string.IsNullOrEmpty(sstRequestUrlFmt) == false)
                {
                    sstRequestUrlFmt = sstRequestUrlFmt
                        .Replace("##YEAR##", selDate.Year.ToString())
                        .Replace("##MONTH##", selDate.ToString("MMM"))
                        .Replace("##DAY##", selDate.Day.ToString())
                        .Replace("##VID_PID_TID##", vid_did_tid);

                    Log($"Requesting SST data from: {sstRequestUrlFmt}");

                    if (_abort.WaitOne(0))
                        return;

                    string response = await _httpClient.GetStringAsync(sstRequestUrlFmt).ConfigureAwait(false);

                    using (HtmlLookup lookup = new HtmlLookup(response))
                    {
                        List<string> elements = lookup.GetElements("a", "href", ".nc");
                        if (elements?.Count > 0)
                        {
                            string file = Path.Combine(SimulationData.WorkFolder, "SST.nc");

                            if (File.Exists(file))
                                File.Delete(file);

                            Log($"Downloading effective SST data from: {elements[0]}");

                            if (_abort.WaitOne(0))
                                return;

                            byte[] data = await _httpClient.GetByteArrayAsync(elements[0]).ConfigureAwait(false);

                            if (_abort.WaitOne(0))
                                return;

                            File.WriteAllBytes(file, data);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log(ex.Message);
            }
        }

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

                    string file = Path.Combine(SimulationData.WorkFolder, "input.grib");
                    Log($"Downloading GRIB data from: {url}");

                    if (_abort.WaitOne(0))
                        return;

                    byte[] data = await _httpClient.GetByteArrayAsync(url).ConfigureAwait(false);

                    if (_abort.WaitOne(0))
                        return;

                    File.WriteAllBytes(file, data);
                }
            }
            catch(Exception ex)
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
            _httpClient.CancelPendingRequests();
        }
    }
}