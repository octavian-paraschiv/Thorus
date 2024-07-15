using System;
using System.Linq;
using System.Windows.Forms;
using ThorusViewer.Models;

namespace OPMedia.UI.Controls
{
    public partial class DataTypeSelectorCtl : UserControl
    {
        public DataTypeSelectorCtl()
        {
            InitializeComponent();
            FillDataTypes();
            ControlPanelModel.Instance.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
        }

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedCategory")
                FillDataTypes();
        }

        private void FillDataTypes()
        {
            try
            {
                cmbDataType.SelectedIndexChanged -= OnDataTypeChanged;

                bool isRaw = string.IsNullOrEmpty(ControlPanelModel.Instance.SelectedCategory);
                cmbDataType.DataSource = (from dt in ControlPanelModel.Instance.DataTypes
                                          where (isRaw || dt.IsStatsAvailable)
                                          select dt).ToList();

                cmbDataType.SelectedItem = ControlPanelModel.Instance.SelectedDataType;
            }
            finally
            {
                cmbDataType.SelectedIndexChanged += OnDataTypeChanged;
            }
        }

        void OnDataTypeChanged(object sender, EventArgs e)
        {
            DataType selDataType = cmbDataType.SelectedItem as DataType;
            ControlPanelModel.Instance.SelectedDataType = selDataType;
        }
    }
}
