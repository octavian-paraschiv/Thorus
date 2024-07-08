using System;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusViewer.Models;

namespace OPMedia.UI.Controls
{
    public partial class CategorySelectorCtl : UserControl
    {
        public string Category { get; set; }

        public CategorySelectorCtl()
        {
            InitializeComponent();
            this.Load += CategorySelectorCtl_Load;

            SimulationData.SnapshotListChanged += new EventHandler(SimulationData_SnapshotListChanged);
        }

        void SimulationData_SnapshotListChanged(object sender, EventArgs e)
        {
            ActivateCategoryButtons();
        }

        private void CategorySelectorCtl_Load(object sender, EventArgs e)
        {
            ActivateCategoryButtons();
        }

        private void ActivateCategoryButtons()
        {
            rbAvg.Enabled = SimulationData.DataCategoryExists("stats/AVG");
            rbMin.Enabled = SimulationData.DataCategoryExists("stats/MIN");
            rbMax.Enabled = SimulationData.DataCategoryExists("stats/MAX");
            rbRaw.Enabled = SimulationData.DataCategoryExists("");

            string selectedCategory = ControlPanelModel.Instance.SelectedCategory;
            if (selectedCategory == null)
            {
                if (rbAvg.Enabled)
                {
                    this.Category = "stats/AVG";
                    rbAvg.Checked = true;
                }
                else if (rbRaw.Enabled)
                {
                    this.Category = "";
                    rbRaw.Checked = true;
                }
                else if (rbMin.Enabled)
                {
                    this.Category = "stats/MIN";
                    rbMin.Checked = true;
                }
                else if (rbMax.Enabled)
                {
                    this.Category = "stats/MAX";
                    rbMax.Checked = true;
                }
            }
        }

        private void OnCategoryChange(object sender, System.EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string tag = rb.Tag as string;
                if (tag != null)
                {
                    ControlPanelModel.Instance.SelectedCategory = tag;
                    SimulationData.LookupDataFiles(tag);
                }
            }
        }
    }
}
