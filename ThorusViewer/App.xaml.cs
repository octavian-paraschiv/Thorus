using System.Windows;
using ThorusCommon.Engine;
using ThorusViewer.Models;

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
            SimulationData.Init();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            SimulationData.SaveSettings();
        }

        public static ControlPanelModel ControlPanelModel
        {
            get
            {
                try
                {
                    return System.Windows.Application.Current.FindResource("controlPanelModel")
                        as ControlPanelModel;
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
