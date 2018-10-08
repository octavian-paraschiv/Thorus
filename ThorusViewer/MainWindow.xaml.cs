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

namespace ThorusViewer
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
		{
			InitializeComponent();

            App.ControlPanelModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);

            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            this.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
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
                    {
                        SimDateTime snapshot = App.ControlPanelModel.SelectedSnapshot;
                        if (snapshot != null)
                            this.Title = string.Format("{0} [Data path: {1}], Snapshot: {2}",
                                App.AppName, SimulationData.DataFolder, snapshot);
                        else
                            this.Title = string.Format("{0} [Data path: {1}], No snapshot currently loaded.",
                                App.AppName, SimulationData.DataFolder);
                    }
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

        private void mnuSubmatrix_Click(object sender, RoutedEventArgs e)
        {
            var viewPort = App.ControlPanelModel.SelectedViewport;

            var allFiles = Directory.GetFiles(SimulationData.DataFolder);

            if (allFiles != null)
            {
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
                            break;

                        default:
                            continue;
                    }


                    string submatrixFolder = Path.Combine(SimulationData.WorkFolder, $"subMatrix_{viewPort.Name}");
                    string subMatrixPath = file.Replace(SimulationData.DataFolder, submatrixFolder);

                    var rawMatrix = FileSupport.LoadSubMatrixFromFile(file, viewPort.MinLon, viewPort.MaxLon, viewPort.MinLat, viewPort.MaxLat);

                    DenseMatrix output = null;

                    if (rawMatrix.RowCount < 10)
                        output = rawMatrix.Interpolate();
                    else
                        output = rawMatrix;

                    FileSupport.SaveMatrixToFile(output, subMatrixPath, false);
                }
            }

            System.Windows.MessageBox.Show("Done.");
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
            if (_simDlg != null)
            {
                System.Windows.MessageBox.Show(this, "Please close all open windows before exiting the application.");
                _simDlg.BringToFront();
                e.Cancel = true;
            }
        }
	}
}
