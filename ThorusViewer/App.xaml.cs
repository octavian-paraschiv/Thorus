using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using ThorusViewer.Models;
using System.Windows.Forms;
using ThorusCommon.Engine;
using System.IO;
using System.Diagnostics;

namespace ThorusViewer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
    public partial class App : System.Windows.Application
	{
        public const string AppName = "Weather Studio";

        public App()
        {
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
        }

        public static ControlPanelModel ControlPanelModel
        {
            get
            {
                return System.Windows.Application.Current.FindResource("controlPanelModel")
                    as ControlPanelModel;
            }
        }
	}
}
