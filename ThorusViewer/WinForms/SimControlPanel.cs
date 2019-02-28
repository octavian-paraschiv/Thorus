using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using ThorusCommon;
using ThorusCommon.Data;
using ThorusViewer.WinForms;

namespace ThorusViewer
{
    public partial class SimControlPanel : Form
    {
        private System.Windows.Forms.Timer _tmrClock = null;
        private int _simProcPid = -1;

        private DateTime _defaultDtStart = DateTime.MinValue;

        private ConcurrentQueue<string> _SimStdOut = new ConcurrentQueue<string>();

        public SimControlPanel()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            _tmrClock = new System.Windows.Forms.Timer();
            _tmrClock.Interval = 100;
            _tmrClock.Tick += new EventHandler(_tmrClock_Tick);
            _tmrClock.Enabled = true;

            DateTime dt = DateTime.Now;
            dtpSimStart.Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);

            dt = dt.AddDays(5);
            dtpSimStop.Value = new DateTime(dt.Year, dt.Month, dt.Day, 23, 0, 0);

            cmbRange.SelectedIndex = 0;
            cmbStepLen.SelectedIndex = 0;

            ValidateInitialConditionFiles(true);

            this.dtpSimStart.ValueChanged += dtpSimStart_ValueChanged;
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_simProcPid > 0)
            {
                Process.GetProcessById(_simProcPid).Kill();
            }
        }

        void _tmrClock_Tick(object sender, EventArgs e)
        {
            ValidateInitialConditionFiles(false);
            CheckSimProcess();
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

                string outLine = "";
                if (_SimStdOut.TryDequeue(out outLine) && outLine != null)
                {
                    EnableGroups(true);

                    int startIdx = outLine.IndexOf('[');
                    if (startIdx > 0 && startIdx < outLine.Length - 1)
                    {
                        int endIdx = outLine.IndexOf('%');
                        if (endIdx > startIdx + 1)
                        {
                            string progStr = outLine.Substring(startIdx + 1, endIdx - startIdx - 1);
                            int progVal = 0;
                            int.TryParse(progStr, out progVal);
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

        private void lnkSimParams_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
                
                string statRange = cmbRange.Text;

                string dtStart = dtpSimStart.Text;
                string dtStop = dtpSimStop.Text;
                string snapshotLen = cmbStepLen.Text;

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
            pbHH.Image = ValidateFile("SPFH_PRES.nc", ref canSimulate);
            pbTT.Image = ValidateFile("TMP_PRES.nc", ref canSimulate);
            pbZZ.Image = ValidateFile("HGT_PRES.nc", ref canSimulate);
            pbSNOW.Image = ValidateFile("WEASD_SFC.nc", ref canSimulate);
            pbSOIL.Image = ValidateFile("TMP_BGRND.nc", ref canSimulate);

            pbSST.Image = ValidateFile("SST.nc", ref canSimulate);

            DateTime? dt = null;

            try
            {
                dtpSimStart.ValueChanged -= dtpSimStart_ValueChanged;

                string dateTimeFile = "SST.nc";
                _defaultDtStart = NetCdfImporter.ImportDateTime(dateTimeFile);

                if (adjustStartDateBasedOnFiles)
                {
                    dtpSimStart.Value = _defaultDtStart;
                    dt = _defaultDtStart;
                }
                else
                {
                    dt = dtpSimStart.Value;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                dt = null;
            }
            finally
            {
                dtpSimStart.ValueChanged += dtpSimStart_ValueChanged;
            }

            btnSimStart.Enabled = canSimulate;
        }

        private bool FileExists(string file)
        {
            string path = Path.Combine(SimulationData.WorkFolder, file);
            return File.Exists(path);
        }

        private void FileDelete(string file)
        {
            string path = Path.Combine(SimulationData.WorkFolder, file);
            File.Delete(path);
        }

        private Image ValidateFile(string file, ref bool canSimulate)
        {
            bool fileExists = FileExists(file);

            canSimulate &= fileExists;

            return fileExists ? Resources.OK : Resources.NG;
        }

        private void cbRunStatsOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRunStatsOnly.Checked)
                cbRunStats.Checked = false;
        }

        private void cbRunStats_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRunStats.Checked)
                cbRunStatsOnly.Checked = false;
        }

        private void dtpSimStart_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = dtpSimStart.Value.AddDays(5);
            dtpSimStop.Value = new DateTime(dt.Year, dt.Month, dt.Day, 23, 0, 0);
        }

        private void btnFetchData_Click(object sender, EventArgs e)
        {
            DataFetcherDlg dlg = new DataFetcherDlg();
            dlg.ShowDialog();

            ValidateInitialConditionFiles(true);
        }
    }
}
