﻿using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusViewer.Models;

namespace ThorusViewer
{
    public static class SimulationDataUtility
    {
        static bool _init = false;
        public static void Init()
        {
            if (_init)
                return;

            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

                SelectWorkingFolder(false);
                SelectDataFolder();

                _init = true;
            }
            catch
            {
                _init = false;
            }
        }

        public static void SelectWorkingFolder(bool reselect)
        {
        begin:
            if (reselect || string.IsNullOrEmpty(SimulationData.WorkFolder))
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog
                {
                    AutoUpgradeEnabled = false,
                    SelectedPath = SimulationData.WorkFolder,

                    Description = string.IsNullOrEmpty(SimulationData.WorkFolder) ?
                    $"Please select the global working folder (where the simulation engine will write its files)" :
                    $"Please select the global working folder (current value: {SimulationData.WorkFolder ?? ""})\r\n" +
                    $"Click Cancel or hit Escape to leave it as-is.",

                    ShowNewFolderButton = true
                };

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    SimulationData.WorkFolder = dlg.SelectedPath;
            }

            if (string.IsNullOrEmpty(SimulationData.WorkFolder))
            {
                if (MessageBox.Show("The application cannot continue without a working folder.\r\n" +
                    "Do you want to try again? If you choose NO, the application will exit.",
                    Constants.Product, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    goto begin;

                Process.GetCurrentProcess().Kill();
            }

        }

        public static void SelectDataFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog
            {
                AutoUpgradeEnabled = false,
                SelectedPath = SimulationData.DataFolder,
                Description = $"Please select the dataset root path (current value: {SimulationData.DataFolder ?? ""})\r\n" +
                $"Click Cancel or hit Escape to leave it as-is."
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ControlPanelModel.Instance.SelectedCategory = "";
                SimulationData.SetNewDataFolder(dlg.SelectedPath);
            }
        }
    }
}
