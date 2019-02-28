using Majestic12;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
            DateTime dtStart = dtEnd.AddYears(-10);
            object sel = -1;

            for (DateTime dt = dtStart; dt <= dtEnd; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek != DayOfWeek.Sunday)
                    continue;

                cmbSstDate.Items.Add(dt);
                if (dtEnd.Subtract(dt).TotalDays <= 3)
                    sel = dt;
            }

            if (sel != null)
                cmbSstDate.SelectedItem = sel;
        }

        private void btnFetchSstData_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this, "This will delete any existing initial condition files. Are you sure you want to proceed ?", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes)
                return;

            DateTime selDate = (DateTime)cmbSstDate.SelectedItem;

            Log($"Initial condition download started for date: {selDate:yyyy-MM-dd}");

            Task.Factory.StartNew(() => _downloadEmails.Reset())
                .ContinueWith(_ => DeleteClientSideData())
                .ContinueWith(_ => DeleteServerSideData())
                .ContinueWith(_ => FetchSstFile(selDate))
                .ContinueWith(_ => FetchOtherFiles())
                .ContinueWith(_ => _downloadEmails.Set());
        }

        private void DeleteClientSideData()
        {
            if (_abort.WaitOne(0))
                return;

            FileDelete("SST.nc");
            FileDelete("TMP_BGRND.nc");
            FileDelete("WEASD_SFC.nc");
            FileDelete("SPFH_PRES.nc");
            FileDelete("TMP_PRES.nc");
            FileDelete("HGT_PRES.nc");

            Log($"Client side data deleted...");

            if (this.InvokeRequired)
                Invoke(new MethodInvoker(ValidateInitialConditionFiles));
            else
                ValidateInitialConditionFiles();
        }

        private void DeleteServerSideData()
        {
            if (_abort.WaitOne(0))
                return;

            using (EmailClient ec = new EmailClient())
            {
                if (ec.Connect())
                {
                    ec.DeleteAllMessages();

                    Log($"Server side data deleted...");
                }
            }
        }

        private void FetchSstFile(DateTime selDate)
        {
            if (_abort.WaitOne(0))
                return;

            string vid_pid_tid = "";

            string sstSearchUrl = ConfigurationManager.AppSettings["noaaSstSearchUrl"];
            if (string.IsNullOrEmpty(sstSearchUrl) == false)
            {
                using (WebClientEx wc = new WebClientEx())
                {
                    Log($"Requesting VID_PID_TID data from: {sstSearchUrl}");

                    string response = wc.DownloadString(sstSearchUrl);
                    string lookupText = "/psd/cgi-bin/DataAccess.pl?DB_dataset=NOAA+Optimum+Interpolation+(OI)+SST+V2&DB_variable=Sea+Surface+Temperature&DB_statistic=Mean&";

                    using (HtmlLookup lookup = new HtmlLookup(response))
                    {
                        var elements = lookup.GetElements("a", "href", lookupText);
                        if (elements.Count > 0)
                        {
                            elements.Sort((s1, s2) =>
                            {
                                return string.Compare(s2, s1, true);
                            });

                            vid_pid_tid = elements[0].Replace(lookupText, string.Empty);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(vid_pid_tid))
            {
                Log($"Invalid VID_PID_TID data. Cannot continue.");
                return;
            }

            Log($"Using: {vid_pid_tid}");

            string sstRequestUrlFmt = ConfigurationManager.AppSettings["noaaSstGetUrl"];
            if (string.IsNullOrEmpty(sstRequestUrlFmt) == false)
            {
                sstRequestUrlFmt = sstRequestUrlFmt
                    .Replace("##YEAR##", selDate.Year.ToString())
                    .Replace("##MONTH##", selDate.ToString("MMM"))
                    .Replace("##DAY##", selDate.Day.ToString())
                    .Replace("##VID_PID_TID##", vid_pid_tid);

                using (WebClientEx wc = new WebClientEx())
                {
                    Log($"Requesting SST data from: {sstRequestUrlFmt}");

                    string response = wc.DownloadString(sstRequestUrlFmt);

                    using (HtmlLookup lookup = new HtmlLookup(response))
                    {
                        var elements = lookup.GetElements("a", "href", "ftp://");
                        if (elements.Count > 0)
                        {
                            string file = Path.Combine(SimulationData.WorkFolder, "SST.nc");

                            if (File.Exists(file))
                                File.Delete(file);

                            Log($"Downloading effective SST data from: {elements[0]}");

                            wc.DownloadFile(elements[0], file);
                        }
                    }
                }
            }
        }

        private void FetchOtherFiles()
        {
            if (_abort.WaitOne(0))
                return;

            if (FileExists("SST.NC"))
            {
                DateTime dtSst = NetCdfImporter.ImportDateTime("SST.NC");

                string[] dataTypes =
                {
                    "SPFH_PRES",
                    "TMP_PRES",
                    "HGT_PRES",
                    "WEASD_SFC",
                    "TMP_BGRND",
                };

                using (WebClientEx wc = new WebClientEx())
                {
                    string postUrl = ConfigurationManager.AppSettings["noaaServerPostUrl"];
                    string postContentType = ConfigurationManager.AppSettings["noaaPostContentType"];
                    string emailAccount = ConfigurationManager.AppSettings["emailAccount"];

                    wc.Headers.Add("Content-Type", postContentType);

                    foreach (string dataType in dataTypes)
                    {
                        if (_abort.WaitOne(0))
                            return;

                        string postBody = ConfigurationManager.AppSettings["noaaPostBody"];
                        postBody = postBody
                            .Replace("##SST_DATE##", dtSst.ToString("yyyy-MM-dd"))
                            .Replace("##PARAM_NAME##", dataType)
                            .Replace("##EMAIL_ACCOUNT##", emailAccount);

                        Log($"Requesting {dataType} data from: {postUrl}...");

                        string s = wc.UploadString(postUrl, postBody);
                        Log($"... response: {s}");
                    }
                }
            }
        }

        private object _emailDownloadInProgress = new object();
        
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
                    _downloadEmails.Reset();
                }

                if (_downloadEmails.WaitOne(0))
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (_abort.WaitOne(0))
                            return;

                        Log($"Querying email data ...");
                        if (Monitor.TryEnter(_emailDownloadInProgress))
                        {
                            using (EmailClient ec = new EmailClient())
                            {
                                if (ec.Connect())
                                {
                                    string msg = ec.FetchWeatherData(SimulationData.WorkFolder);

                                    Log($"...{msg}");
                                }
                                else
                                {
                                    Log($"Failed to connect to POP3 server");
                                }
                            }

                            Monitor.Exit(_emailDownloadInProgress);
                        }
                    });
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
            pbHH.Image = ValidateFile("SPFH_PRES.nc", ref allFilesPresent);
            pbTT.Image = ValidateFile("TMP_PRES.nc", ref allFilesPresent);
            pbZZ.Image = ValidateFile("HGT_PRES.nc", ref allFilesPresent);
            pbSNOW.Image = ValidateFile("WEASD_SFC.nc", ref allFilesPresent);
            pbSOIL.Image = ValidateFile("TMP_BGRND.nc", ref allFilesPresent);
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
            return File.Exists(path);
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
                string line = string.Format(msg, args);

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
