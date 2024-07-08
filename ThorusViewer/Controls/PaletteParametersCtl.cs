using System;
using System.Windows.Forms;
using ThorusViewer.Models;
using ThorusViewer.Palettes;

namespace OPMedia.UI.Controls
{
    public partial class PaletteParametersCtl : UserControl
    {
        public PaletteParametersCtl()
        {
            InitializeComponent();

            cmbLineWidth.Items.Add(1f);
            cmbLineWidth.Items.Add(1.5f);
            cmbLineWidth.Items.Add(2f);
            cmbLineWidth.Items.Add(2.5f);
            cmbLineWidth.Items.Add(3f);
            cmbLineWidth.Items.Add(4f);
            cmbLineWidth.Items.Add(5f);

            cmbLevelSpacing.Items.Add(1f);
            cmbLevelSpacing.Items.Add(2f);
            cmbLevelSpacing.Items.Add(4f);
            cmbLevelSpacing.Items.Add(5f);
            cmbLevelSpacing.Items.Add(8f);
            cmbLevelSpacing.Items.Add(10f);

            cmbLineColor.Items.Add(new LineColor(LineColorMode.Black_And_White));
            cmbLineColor.Items.Add(new LineColor(LineColorMode.Best_Contrast));
            cmbLineColor.Items.Add(new LineColor(OxyPlot.OxyColors.Black));
            cmbLineColor.Items.Add(new LineColor(OxyPlot.OxyColors.Red));
            cmbLineColor.Items.Add(new LineColor(OxyPlot.OxyColors.Green));
            cmbLineColor.Items.Add(new LineColor(OxyPlot.OxyColors.Blue));
            cmbLineColor.Items.Add(new LineColor(OxyPlot.OxyColors.White));

            ControlPanelModel.Instance.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
        }

        bool _allowEvents = false;
        bool _initialLoad = true;

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {


            try
            {
                _allowEvents = false;

                if (_initialLoad || e.PropertyName == "SelectedDataType")
                {
                    _initialLoad = false;
                    LoadPaletteDefaultParams();
                }
            }
            catch { }
            finally
            {
                _allowEvents = true;
            }
        }

        private void LoadPaletteDefaultParams()
        {
            string dataType = ControlPanelModel.Instance.SelectedDataType.Name;
            WeatherDataPalette wdp = WeatherDataPaletteFactory.GetPaletteForDataType(dataType);
            if (wdp != null)
            {
                chkShowContours.Enabled = wdp.AcceptsContourLines;
                chkShowContours.Checked = wdp.ShowContours;

                if (cmbLineWidth.Items.Contains(wdp.LineWidth) == false)
                    cmbLineWidth.Items.Add(wdp.LineWidth);

                cmbLineWidth.SelectedItem = wdp.LineWidth;

                if (cmbLevelSpacing.Items.Contains(wdp.LineSpacing) == false)
                    cmbLevelSpacing.Items.Add(wdp.LineSpacing);

                cmbLevelSpacing.SelectedItem = wdp.LineSpacing;

                if (cmbLineColor.Items.Contains(wdp.LineColor) == false)
                    cmbLineColor.Items.Add(wdp.LineColor);

                cmbLineColor.SelectedItem = wdp.LineColor;
            }
            else
            {
                cmbLineWidth.SelectedIndex = 0;
                cmbLevelSpacing.SelectedIndex = 0;
                cmbLineColor.SelectedIndex = 0;
            }
        }

        private void OnParamsChanged(object sender, EventArgs e)
        {
            if (_allowEvents)
            {
                bool showContours = false;

                if (chkShowContours.Enabled)
                    showContours = chkShowContours.Checked;

                string dataType = ControlPanelModel.Instance.SelectedDataType.Name;
                WeatherDataPalette wdp = WeatherDataPaletteFactory.GetPaletteForDataType(dataType);
                if (wdp != null)
                {
                    wdp.ShowContours = showContours;
                    wdp.LineWidth = (float)cmbLineWidth.SelectedItem;
                    wdp.LineSpacing = (float)cmbLevelSpacing.SelectedItem;
                    wdp.LineColor = (LineColor)cmbLineColor.SelectedItem;
                }

                ControlPanelModel.Instance.FirePropertyChanged("PaletteParams");
            }
        }
    }
}
