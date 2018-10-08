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
    public partial class ViewportSelectorView : UserControl
    {
        public ViewportSelectorView()
        {
            InitializeComponent();
            
            cmbViewport.ItemsSource = App.ControlPanelModel.Viewports;
            cmbViewport.SelectedItem = App.ControlPanelModel.SelectedViewport;
            cmbViewport.SelectionChanged += new SelectionChangedEventHandler(cmbViewporT_TElectionChanged);

            
        }

        void cmbViewporT_TElectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Viewport selViewport = cmbViewport.SelectedItem as Viewport;
            App.ControlPanelModel.SelectedViewport = selViewport;
        }
    }
}
