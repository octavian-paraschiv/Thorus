using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThorusViewer.WinForms
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();

            pbProgress.Minimum = 0;
            pbProgress.Maximum = 100;
            pbProgress.Step = 1;

            this.ShowInTaskbar = false;
            this.TopLevel = true;
            this.TopMost = true;
        }

        private delegate void DisplayProgressDG(int current, int total, string desc);
        public void DisplayProgress(int current, int total, string desc)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DisplayProgressDG(DisplayProgress), current, total, desc);
                return;
            }

            try
            {
                if (total < 0)
                {
                    this.Show();
                    this.lblDesc.Text = desc;
                    pbProgress.Style = ProgressBarStyle.Marquee;
                }
                else if (total == 0)
                {
                    this.Hide();
                }
                else
                {
                    this.Show();
                    pbProgress.Style = ProgressBarStyle.Continuous;
                    var percent = (100 * current / total);
                    this.lblDesc.Text = $"{desc}: {current} of {total} steps ... {percent}% done";
                    pbProgress.Value = percent;
                }
            }
            catch { }
            finally
            {
                Application.DoEvents();
            }
        }
    }
}
