using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ThorusViewer.Models;

namespace ThorusViewer
{
    public partial class SelectDatabaseDlg : Form
    {
        public List<MeteoDbInfo> Databases { get; set; }

        public MeteoDbInfo SelectedDatabase
        {
            get
            {
                if (lbDatabases.SelectedItem is MeteoDbInfo mdi)
                    return mdi;

                return null;
            }
        }

        public SelectDatabaseDlg()
        {
            InitializeComponent();
            this.Load += new EventHandler(ParametersForm_Load);
        }

        void ParametersForm_Load(object sender, EventArgs e)
        {
            lbDatabases.DataSource = Databases;
            lbDatabases.Format += (s, e) =>
            {
                if (e.ListItem is MeteoDbInfo mdi)
                    e.Value = mdi.ToString();
            };
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (SelectedDatabase != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
