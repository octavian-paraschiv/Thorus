using System;
using ThorusCommon.Engine;
using ThorusViewer.Forms;

namespace ThorusViewer
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SimulationData.Init();

            try
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new MainForm());
            }
            catch { }
            finally
            {
                SimulationData.SaveSettings();
            }
        }
    }
}
