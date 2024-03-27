using System.Windows;
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
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
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
