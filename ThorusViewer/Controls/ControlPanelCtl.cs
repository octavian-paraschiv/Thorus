using System;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusViewer.Models;
using ThorusViewer.Palettes;
using ThorusViewer.Pallettes;

namespace OPMedia.UI.Controls
{
    public partial class ControlPanelCtl : UserControl
    {
        public ControlPanelCtl()
        {
            InitializeComponent();
            ControlPanelModel.Instance.PropertyChanged += ControlPanelModel_PropertyChanged;
        }

        Type _baseDataType = typeof(TemperaturePalette);

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                SimDateTime snapshot = ControlPanelModel.Instance.SelectedSnapshot;
                if (snapshot == null)
                    return;

                string folder = SimulationData.DataFolder;
                if (string.IsNullOrEmpty(ControlPanelModel.Instance.SelectedCategory) == false)
                    folder = System.IO.Path.Combine(SimulationData.DataFolder, ControlPanelModel.Instance.SelectedCategory);

                string fieldDataFileName = string.Format("{0}_MAP_{1}.thd",
                    ControlPanelModel.Instance.SelectedDataType.Name,
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
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}
