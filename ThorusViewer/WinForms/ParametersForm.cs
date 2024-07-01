using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThorusCommon;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ThorusViewer
{
    public partial class ParametersForm : Form
    {
        public ParametersForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(ParametersForm_Load);
        }

        void ParametersForm_Load(object sender, EventArgs e)
        {
            LoadParams();
            pgProperties.ResizeDescriptionArea(8);
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveParams();
        }


        private void LoadParams()
        {
            pgProperties.SelectedObject = SimulationParameters.Instance.Clone();
        }

        private void SaveParams()
        {
            SimulationParameters simp = (pgProperties.SelectedObject as SimulationParameters);
            if (simp != null)
            {
                SimulationParameters.Instance.LoadFromString(simp.ToString());
            }
        }

        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            pgProperties.Validate();
        }

        private void pgProperties_Click(object sender, EventArgs e)
        {

        }

        private void resetToDefaultValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pgProperties.SelectedGridItem != null)
            {
                PropertyDescriptor pd = pgProperties.SelectedGridItem.PropertyDescriptor;
                pd.ResetValue(pgProperties.SelectedObject);

                pgProperties.Refresh();
            }
        }

        private void cms_Opening(object sender, CancelEventArgs e)
        {
            var lItem = pgProperties.SelectedGridItem;

            var lComponent = lItem.GetType().GetProperty("Component").GetValue(lItem, null);

            tsmReset.Text = "Reset to default value";

            if (lComponent != null)
                tsmReset.Enabled = lItem.PropertyDescriptor.CanResetValue(lComponent);
            else
                tsmReset.Enabled = lItem.PropertyDescriptor.CanResetValue(pgProperties.SelectedObject);

            if (tsmReset.Enabled)
            {
                foreach (Attribute a in lItem.PropertyDescriptor.Attributes)
                {
                    DefaultValueAttribute dva = a as DefaultValueAttribute;
                    if (dva != null)
                    {
                        tsmReset.Text += string.Format(" ({0})", dva.Value);
                        break;
                    }
                }
            }
        }
    }
}
