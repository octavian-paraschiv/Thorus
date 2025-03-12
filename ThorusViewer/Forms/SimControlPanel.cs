using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ThorusCommon;
using ThorusCommon.Data;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusViewer.Forms;

namespace ThorusViewer
{
    public partial class SimControlPanel : Form
    {
        private readonly System.Windows.Forms.Timer _tmrClock = null;
        private int _simProcPid = -1;

        private DateTime _defaultDtStart = DateTime.MinValue;

        private ConcurrentQueue<string> _SimStdOut = new ConcurrentQueue<string>();

        const int SimStopDaysToAdd = 7 * 14;

        public SimControlPanel()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            _tmrClock = new System.Windows.Forms.Timer
            {
                Interval = 100
            };

            _tmrClock.Tick += (s, e) =>
            {
                ValidateInitialConditionFiles(false);
                CheckSimProcess();
            };

            _tmrClock.Start();

            DateTime dt = DateTime.Now;
            dtpSimStart.Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);

            dt = dt.AddDays(SimStopDaysToAdd);
            dtpSimStop.Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);

            cmbRange.SelectedIndex = 0;
            cmbStepLen.SelectedIndex = 0;

            ValidateInitialConditionFiles(true);

            this.dtpSimStart.ValueChanged += OnSimStartValueChanged;


        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_simProcPid > 0)
            {
                var res = MessageBox.Show("Closing this window will also terminate the simulation in progress. Do you want to proceed?",
                    Constants.Product, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                    Process.GetProcessById(_simProcPid).Kill();
                else
                    e.Cancel = true;
            }
        }

        private void EnableGroups(bool isSimProcRunning)
        {
            gbInitialConditions.Enabled = !isSimProcRunning;
            gbSimParams.Enabled = !isSimProcRunning;
        }

        bool _wasSimProcessRunning = false;

        private void CheckSimProcess()
        {
            Process[] procs = Process.GetProcesses();
            var simProc = (from p in procs
                           where p.ProcessName.ToLowerInvariant().Contains("thorussimulation")
                           select p).FirstOrDefault();

            _simProcPid = (simProc != null) ? simProc.Id : -1;

            btnSimStart.Text = _simProcPid > 0 ? "Stop simulation" : "Start simulation";

            if (_simProcPid > 0 && _SimStdOut != null)
            {
                _wasSimProcessRunning = true;

                if (_SimStdOut.TryDequeue(out string outLine) && outLine != null)
                {
                    EnableGroups(true);

                    int startIdx = outLine.IndexOf('[');
                    if (startIdx > 0 && startIdx < outLine.Length - 1)
                    {
                        int endIdx = outLine.IndexOf('%');
                        if (endIdx > startIdx + 1)
                        {
                            string progStr = outLine.Substring(startIdx + 1, endIdx - startIdx - 1);
                            int.TryParse(progStr, out int progVal);
                            pbSimProgress.Value = Math.Max(0, Math.Min(100, progVal));
                        }
                    }

                    try
                    {
                        List<string> lines = txtSimProcOut.Lines.ToList();
                        lines.Add(outLine);
                        txtSimProcOut.Lines = lines.ToArray();

                        txtSimProcOut.SelectionStart = txtSimProcOut.TextLength;
                        txtSimProcOut.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        txtSimProcOut.Text = ex.Message;
                    }
                }
            }
            else
            {
                if (_wasSimProcessRunning && cbAutoClose.Checked)
                {
                    this.Close();
                    return;
                }

                _wasSimProcessRunning = false;

                EnableGroups(false);
                txtSimProcOut.Text = "";
                pbSimProgress.Value = 0;
            }
        }

        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ParametersForm frm = new ParametersForm();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                SimulationParameters.Instance.SaveToFile();
            }
        }

        private void OnStartStopSimulation(object sender, EventArgs e)
        {
            if (_simProcPid > 0)
            {
                Process.GetProcessById(_simProcPid).Kill();
            }
            else
            {
                pbSimProgress.Value = 0;

                bool runSim = !cbRunStatsOnly.Checked;
                bool runStats = cbRunStats.Checked || cbRunStatsOnly.Checked;
                bool runViewer = cbAutoClose.Checked;
                bool autoExport = cbAutoExport.Checked;

                string statRange = cmbRange.Text;

                string dtStart = dtpSimStart.Text;
                string dtStop = dtpSimStop.Text;
                string snapshotLen = cmbStepLen.Text;

                if (runStats)
                    pbSimProgress.Style = ProgressBarStyle.Marquee;
                else
                    pbSimProgress.Style = ProgressBarStyle.Continuous;

                _SimStdOut = new ConcurrentQueue<string>();

                ThreadPool.QueueUserWorkItem((c) =>
                    {
                        string args = "";

                        ProcessStartInfo psi = new ProcessStartInfo(".\\ThorusSimulation.exe");

                        if (runSim)
                            args += string.Format("{0} {1} ", dtStart, dtStop);
                        else
                            args += "nosim ";

                        //args += snapshotLen;

                        if (runStats)
                            args += string.Format("stat {0}", statRange);
                        else
                            args += " -regen";

                        if (autoExport)
                            args += " -export";

                        psi.Arguments = args;
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        psi.RedirectStandardOutput = true;
                        psi.RedirectStandardError = true;
                        psi.RedirectStandardInput = true;
                        Process simProc = Process.Start(psi);

                        string s = simProc.StandardOutput.ReadLine();
                        while (string.IsNullOrEmpty(s) == false)
                        {
                            _SimStdOut?.Enqueue(s);
                            s = simProc.StandardOutput.ReadLine();
                        }

                        simProc.WaitForExit();
                    });
            }
        }


        private void ValidateInitialConditionFiles(bool adjustStartDateBasedOnFiles)
        {
            if (this.DesignMode)
                return;

            bool canSimulate = true;

            pbGrib.Image = ValidateFile("input.grib", ref canSimulate);
            pbSST.Image = ValidateFile("SST.nc", ref canSimulate);

            try
            {
                dtpSimStart.ValueChanged -= OnSimStartValueChanged;

                _defaultDtStart = NetCdfImporter.SstDateTime();

                if (adjustStartDateBasedOnFiles)
                    dtpSimStart.Value = _defaultDtStart;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            finally
            {
                dtpSimStart.ValueChanged += OnSimStartValueChanged;
            }

            btnSimStart.Enabled = canSimulate;
        }

        private bool FileExists(string file)
        {
            string path = Path.Combine(SimulationData.WorkFolder, file);
            return File.Exists(path);
        }

        private Image ValidateFile(string file, ref bool canSimulate)
        {
            bool fileExists = FileExists(file);

            canSimulate &= fileExists;

            return fileExists ? Resources.OK : Resources.Error;
        }

        private void OnRunStatsOnlyCheckedChanged(object sender, EventArgs e)
        {
            if (cbRunStatsOnly.Checked)
                cbRunStats.Checked = false;
        }

        private void OnRunStatsCheckedChanged(object sender, EventArgs e)
        {
            if (cbRunStats.Checked)
                cbRunStatsOnly.Checked = false;
        }

        private void OnSimStartValueChanged(object sender, EventArgs e)
        {
            DateTime dt = dtpSimStart.Value.AddDays(SimStopDaysToAdd);
            dtpSimStop.Value = new DateTime(dt.Year, dt.Month, dt.Day, 23, 0, 0);
        }

        private void OnFetchDataClick(object sender, EventArgs e)
        {
            DataFetcherDlg dlg = new DataFetcherDlg();
            dlg.ShowDialog();
            ValidateInitialConditionFiles(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
