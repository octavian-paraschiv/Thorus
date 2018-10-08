using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThorusViewer.Models;

namespace ThorusViewer.Views
{
    /// <summary>
    /// Interaction logic for ViewportSelectionControl.xaml
    /// </summary>
    public partial class DataTypeSelectorView : UserControl
    {
        public DataTypeSelectorView()
        {
            InitializeComponent();

            FillDataTypes();
            

            App.ControlPanelModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
        }

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedCategory")
            {
                FillDataTypes();
            }
        }

        private void FillDataTypes()
        {
            try
            {
                cmbDataType.SelectionChanged -= new SelectionChangedEventHandler(cmbDataType_SelectionChanged);

                bool isRaw = string.IsNullOrEmpty(App.ControlPanelModel.SelectedCategory);
                cmbDataType.ItemsSource = (from dt in App.ControlPanelModel.DataTypes
                                           where (isRaw || dt.IsStatsAvailable)
                                           select dt).ToList();
                cmbDataType.SelectedItem = App.ControlPanelModel.SelectedDataType;
            }
            finally
            {
                cmbDataType.SelectionChanged += new SelectionChangedEventHandler(cmbDataType_SelectionChanged);
            }
        }

        void cmbDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataType selDataType = cmbDataType.SelectedItem as DataType;
            App.ControlPanelModel.SelectedDataType = selDataType;
        }
    }
}
