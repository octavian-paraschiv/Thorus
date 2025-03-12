using System.Windows.Forms;

namespace ThorusViewer.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        private delegate void DisplayProgressDG(Form owner, int current, int total, string desc);

        public void DisplayProgress(Form owner, int current, int total, string desc)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DisplayProgressDG(DisplayProgress), owner, current, total, desc);
                return;
            }

            try
            {
                if (total < 0)
                {
                    if (!this.Visible)
                    {
                        this.Show(owner);
                        this.CenterToParent();
                    }

                    this.lblDesc.Text = desc;
                    pbProgress.Style = ProgressBarStyle.Marquee;
                }
                else if (total == 0 && this.Visible)
                {
                    this.Hide();
                }
                else
                {
                    if (!this.Visible)
                    {
                        this.Show(owner);
                        this.CenterToParent();
                    }
                    pbProgress.Style = ProgressBarStyle.Continuous;
                    var percent = (100 * current / total);
                    this.lblDesc.Text = $"{desc}: {current} of {total} steps ... {percent}% done";
                    pbProgress.Value = percent;
                }
            }
            catch
            {
                // We don't care about the error message.
            }
            finally
            {
                Application.DoEvents();
            }
        }
    }
}
