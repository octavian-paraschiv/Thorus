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
using ThorusCommon.Engine;
using ThorusViewer.Palettes;
using ThorusViewer.Pallettes;

namespace ThorusViewer.Views
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanelView : UserControl
    {
        public ControlPanelView()
        {
            InitializeComponent();
            App.ControlPanelModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
        }

        Type _baseDataType = typeof(TemperaturePalette);

        public void OnOffsetChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.ControlPanelModel.Offset = e.NewValue;
        }

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                SimDateTime snapshot = App.ControlPanelModel.SelectedSnapshot;
                if (snapshot == null)
                    return;

                string folder = SimulationData.DataFolder;
                if (string.IsNullOrEmpty(App.ControlPanelModel.SelectedCategory) == false)
                    folder = System.IO.Path.Combine(SimulationData.DataFolder, App.ControlPanelModel.SelectedCategory);

                string fieldDataFileName = string.Format("{0}_MAP_{1}.thd",
                    App.ControlPanelModel.SelectedDataType.Name,
                    snapshot.Title);

                string fieldDataFile = System.IO.Path.Combine(folder, fieldDataFileName);
                WeatherDataPalette wdp = WeatherDataPaletteFactory.GetPaletteForDataFile(fieldDataFile);

                Type baseDataType = null;
                switch (e.PropertyName)
                {
                    case "SelectedDataType":
                        {
                            if (wdp.GetType().IsSubclassOf(typeof(TemperaturePalette)))
                                baseDataType = typeof(TemperaturePalette);
                            else if (wdp.GetType().IsSubclassOf(typeof(PressurePalette)))
                                baseDataType = typeof(PressurePalette);
                            else if (wdp.GetType().IsSubclassOf(typeof(HumidityPalette)))
                                baseDataType = typeof(HumidityPalette);
                            else
                                baseDataType = wdp.GetType();
                        }
                        break;

                    default:
                        break;
                }

                this.sliderPrecipOffset.IsEnabled = wdp.ShowHeatmap;
                this.sliderPrecipOffset.Visibility = wdp.ShowHeatmap ? Visibility.Visible : Visibility.Hidden;
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}
