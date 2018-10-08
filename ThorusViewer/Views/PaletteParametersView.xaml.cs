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
using ThorusViewer.Palettes;
using ThorusCommon.Data;
using ThorusCommon.IO;

namespace ThorusViewer.Views
{
    /// <summary>
    /// Interaction logic for PaletteParametersView.xaml
    /// </summary>
    public partial class PaletteParametersView : UserControl
    {
        public PaletteParametersView()
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

            App.ControlPanelModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
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
            string dataType = App.ControlPanelModel.SelectedDataType.Name;
            WeatherDataPalette wdp = WeatherDataPaletteFactory.GetPaletteForDataType(dataType);
            if (wdp != null)
            {
                chkShowContours.IsEnabled = wdp.AcceptsContourLines;
                chkShowContours.IsChecked = wdp.ShowContours;

                var offsetSize = 0.5f * wdp.MinMax.Delta;

                nudMinValue.Minimum = wdp.MinMax.Min - offsetSize;
                nudMinValue.Maximum = wdp.MinMax.Min + offsetSize;
                nudMinValue.Value = wdp.MinMax.Min;

                nudMaxValue.Minimum = wdp.MinMax.Max - offsetSize;
                nudMaxValue.Maximum = wdp.MinMax.Max + offsetSize;
                nudMaxValue.Value = wdp.MinMax.Max;

                if (cmbLineWidth.Items.Contains(wdp.LineWidth) == false)
                    cmbLineWidth.Items.Add(wdp.LineWidth);
                cmbLineWidth.SelectedValue = wdp.LineWidth;

                if (cmbLevelSpacing.Items.Contains(wdp.LineSpacing) == false)
                    cmbLevelSpacing.Items.Add(wdp.LineSpacing);
                cmbLevelSpacing.SelectedValue = wdp.LineSpacing;

                if (cmbLineColor.Items.Contains(wdp.LineColor) == false)
                    cmbLineColor.Items.Add(wdp.LineColor);
                cmbLineColor.SelectedValue = wdp.LineColor;
            }
        }

        private void OnParamsChanged(object sender, EventArgs e)
        {
            if (_allowEvents)
            {
                bool showContours = false;

                if (chkShowContours.IsEnabled)
                    showContours = chkShowContours.IsChecked.GetValueOrDefault();

                string dataType = App.ControlPanelModel.SelectedDataType.Name;
                WeatherDataPalette wdp = WeatherDataPaletteFactory.GetPaletteForDataType(dataType);
                if (wdp != null)
                {
                    wdp.ShowContours = showContours;
                    wdp.LineWidth = (float)cmbLineWidth.SelectedValue;
                    wdp.LineSpacing = (float)cmbLevelSpacing.SelectedValue;

                    Range<float> range = new Range<float>(nudMinValue.Value.GetValueOrDefault(), nudMaxValue.Value.GetValueOrDefault());

                    wdp.MinMax.Min = range.Min;
                    wdp.MinMax.Max = range.Max;

                    wdp.LineColor = (LineColor)cmbLineColor.SelectedItem;
                }

                App.ControlPanelModel.FirePropertyChanged("PaletteParams");
            }
        }
    }

    public enum LineColorMode
    {
        FixedColor = 0,
        Best_Contrast,
        Black_And_White
    }

    public class LineColor
    {
        public OxyPlot.OxyColor Color { get; private set; }
        public LineColorMode ColorMode { get; private set; }

        public LineColor(OxyPlot.OxyColor color) : 
            this(color, LineColorMode.FixedColor)
        {
        }

        public LineColor(LineColorMode mode) :
            this(OxyPlot.OxyColors.Black, mode)
        {
        }

        public LineColor()
            : this(OxyPlot.OxyColors.Black)
        {
        }

        private LineColor(OxyPlot.OxyColor color, LineColorMode mode)
        {
            this.ColorMode = mode;

            if (mode == Views.LineColorMode.FixedColor)
                this.Color = color;
            else
                this.Color = OxyPlot.OxyColors.Transparent;
        }

        public override bool Equals(object obj)
        {
            LineColor lc = (obj as LineColor);

            if (lc != null)
            {
                return (lc.Color == this.Color && lc.ColorMode == this.ColorMode);
            }

            return false;
        }

        public override string ToString()
        {
            if (ColorMode == LineColorMode.FixedColor)
                return Color.ToString();

            return ColorMode.ToString();
        }
    }
}
