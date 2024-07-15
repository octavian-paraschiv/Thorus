using System;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusViewer.Forms;

namespace ThorusViewer
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                SimulationDataUtility.Init();

                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            finally
            {
                SimulationData.SaveSettings();
            }
        }
    }
}
