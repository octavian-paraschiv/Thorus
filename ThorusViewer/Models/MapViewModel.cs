using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Series;
using ThorusCommon.IO;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.MatrixExtensions;
using System.Windows.Media;
using System.IO;
using System.Windows;
using OxyPlot.Annotations;
using ThorusViewer.Palettes;
using ThorusCommon.Engine;
using ThorusCommon;
using ThorusCommon.Thermodynamics;
using ThorusViewer.Pallettes;
using ThorusCommon.Data;
using Microsoft.Win32;
using ThorusViewer.Series;

namespace ThorusViewer.Models
{
    /// <summary>
    /// Represents the view-model for the map window.
    /// </summary>
    public class MapViewModel
    {
        /// <summary>
        /// Gets the plot model.
        /// </summary>
        public PlotModel Model { get; private set; }

        public string FileTitle { get; private set; }

        ImageAnnotation _roCounties = null;

        public void LoadWeatherFieldData(string fieldFileName)
        {
            ReloadModel(fieldFileName, false);
            this.Model.InvalidatePlot(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MapViewModel()
        {
            App.ControlPanelModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
            PlotModel model = new PlotModel();
            this.Model = model;
        }

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                //case "SelectedViewport":
                //    {
                //        this.Model.Axes[0].Minimum = App.ControlPanelModel.SelectedViewport.MinLat;
                //        this.Model.Axes[0].Maximum = App.ControlPanelModel.SelectedViewport.MaxLat;
                //        this.Model.Axes[1].Minimum = App.ControlPanelModel.SelectedViewport.MinLon;
                //        this.Model.Axes[1].Maximum = App.ControlPanelModel.SelectedViewport.MaxLon;
                //        this.Model.Title = App.ControlPanelModel.SelectedViewport.Name;

                //        if (App.ControlPanelModel.SelectedViewport.Name == "Romania" &&
                //            this.Model.Annotations.Contains(_roCounties) == false)
                //            this.Model.Annotations.Add(_roCounties);
                //        else if (this.Model.Annotations.Contains(_roCounties))
                //            this.Model.Annotations.Remove(_roCounties);

                //        this.Model.InvalidatePlot(true);
                //    }
                //    break;

                case "ShowContours":
                case "SelectedViewport":
                case "SelectedSnapshot":
                case "SelectedCategory":
                case "SelectedDataType":
                case "PaletteParams":
                case "Offset":
                    {
                        if (App.ControlPanelModel.SelectedSnapshot != null)
                        {
                            SimDateTime snapshot = App.ControlPanelModel.SelectedSnapshot;
                            string folder = SimulationData.DataFolder;
                            if (string.IsNullOrEmpty(App.ControlPanelModel.SelectedCategory) == false)
                                folder = Path.Combine(SimulationData.DataFolder, App.ControlPanelModel.SelectedCategory);

                            string fieldDataFileName = string.Format("{0}_MAP_{1}.thd", 
                                App.ControlPanelModel.SelectedDataType.Name,
                                snapshot.Title);

                            string fieldDataFile = System.IO.Path.Combine(folder, fieldDataFileName);

                            ReloadModel(fieldDataFile, App.ControlPanelModel.SelectedDataType.IsWindMap);
                            this.Model.InvalidatePlot(true);

                            App.ControlPanelModel.DoAutoSave();
                        }
                    }
                    break;
            }
            
        }

        private void ReloadModel(string fieldDataFile, bool isWindMap)
        {
            try
            {
                DoReloadModel(fieldDataFile, isWindMap);
                RefitMap();
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }

        public void RefitMap()
        {
            Viewport v = App.ControlPanelModel.SelectedViewport;

            const float AspectRatioThreshold = 2.5f;

            float MaxWidth = (float)App.Current.MainWindow.ActualWidth - 15f;
            float MaxHeight = (float)App.Current.MainWindow.ActualHeight - 180f; 


            if (v.AspectRatio > AspectRatioThreshold)
            {
                (this.Model.PlotView as OxyPlot.Wpf.PlotView).Height = MaxWidth / v.AspectRatio;
                (this.Model.PlotView as OxyPlot.Wpf.PlotView).Width = MaxWidth;
            }
            else
            {
                (this.Model.PlotView as OxyPlot.Wpf.PlotView).Height = MaxHeight;
                (this.Model.PlotView as OxyPlot.Wpf.PlotView).Width = MaxHeight * v.AspectRatio;
            }
        }

        private void DoReloadModel(string fieldDataFile, bool isWindMap)
        {
            // Create the plot model
            PlotModel model = this.Model;

            model.PlotMargins = new OxyThickness(30, 0, 60, 30);

            model.Axes.Clear();
            model.Series.Clear();
            model.Annotations.Clear();

            string fileTitle = Path.GetFileNameWithoutExtension(fieldDataFile);
            this.FileTitle = fileTitle;

            WeatherDataPalette wdp = WeatherDataPaletteFactory.GetPaletteForDataFile(fieldDataFile);
            bool contours = wdp.ShowContours;
            bool heatmap = wdp.ShowHeatmap;
            bool precipitationMap = false;

            model.Title = string.Format("{0}: {1} [{2}]{3}",
                App.ControlPanelModel.SelectedViewport.Name,
                App.ControlPanelModel.SelectedDataType.Value,
                fileTitle,
                App.ControlPanelModel.SelectedDataType.Comments);

            DenseMatrix m = null;

            int minLon = App.ControlPanelModel.SelectedViewport.MinLon.Round();
            int maxLon = App.ControlPanelModel.SelectedViewport.MaxLon.Round();
            int minLat = App.ControlPanelModel.SelectedViewport.MinLat.Round();
            int maxLat = App.ControlPanelModel.SelectedViewport.MaxLat.Round();

            var fieldMatrix = FileSupport.LoadSubMatrixFromFile(fieldDataFile, minLon, maxLon, minLat, maxLat);

            bool interpolate = (fieldMatrix.RowCount < 10);

            if (interpolate)
                fieldMatrix = fieldMatrix.Interpolate();

            float[,] data = null;
            float[,] data2 = null;

            float actualOffset = 0;
            if (wdp.ShowHeatmap)
            {
                float fOffset = (float)App.ControlPanelModel.Offset / 100;
                float delta = wdp.MinMax.Delta;

                if (wdp.GetType() == typeof(C_00_Palette))
                    delta = 100;

                actualOffset = delta * fOffset;
            }

            if (wdp.GetType() == typeof(C_00_Palette))
            {
                // It's a map of the precipitation
                precipitationMap = true;

                string t01File = fieldDataFile.Replace("C_00", "T_01");
                string teFile = fieldDataFile.Replace("C_00", "T_TE");
                string tsFile = fieldDataFile.Replace("C_00", "T_TS");

                DenseMatrix T01 = FileSupport.LoadSubMatrixFromFile(t01File, minLon, maxLon, minLat, maxLat);
                DenseMatrix TE = FileSupport.LoadSubMatrixFromFile(teFile, minLon, maxLon, minLat, maxLat);
                DenseMatrix TS = FileSupport.LoadSubMatrixFromFile(tsFile, minLon, maxLon, minLat, maxLat);
                DenseMatrix C00 = fieldMatrix;

                if (interpolate)
                {
                    T01 = T01.Interpolate();
                    TE = TE.Interpolate();
                    TS = TS.Interpolate();
                }

                float sRain = 0;
                float sSnow = 300;
                float sSleet = 600;
                float sFreezingRain = 900;
                
                data2 = C00.Transpose().ToArray();

                m = DenseMatrix.Create(C00.RowCount, C00.ColumnCount, (r, c) =>
                    {
                        float cl = Math.Abs(C00[r, c]) + actualOffset;

                        if (cl <= 0)
                            cl = 0;
                        if (cl >= 100)
                            cl = 100;


                        float t01 = T01[r, c];
                        float te = TE[r, c];
                        float ts = TS[r, c];

                        float precipClThreshold = 10f;
                        float actualPrecipRate = (cl - precipClThreshold);
                        if (actualPrecipRate >= 0)
                        {
                            return PrecipTypeComputer<float>.Compute(

                                // Actual temperatures
                                te, ts, t01,

                                // Boundary temperatures as read from simulation parameters
                                SimulationParameters.Instance,

                                // Computed precip type: snow
                                () => (cl + sSnow),

                                // Computed precip type: rain
                                () => (cl + sRain),

                                // Computed precip type: freezing rain
                                () => (cl + sFreezingRain),

                                // Computed precip type: sleet
                                () => (cl + sSleet)
                            );
                        }
                        else if (cl > 5)
                            // Cloudy but without precipitation.
                            return 5;

                        // Sunny
                        return 0;
                    });
            }
            else
            {
                m = DenseMatrix.Create(fieldMatrix.RowCount, fieldMatrix.ColumnCount, (r, c) =>
                        fieldMatrix[r, c]);

                m.ADD(actualOffset);
            }

            Range<float> minMax = wdp.MinMax;
            float lineSpacing = wdp.LineSpacing;

            m = m.MIN(minMax.Max).MAX(minMax.Min);
            data = m.Transpose().ToArray();

            float step = interpolate ? 0.5f : 1;

            List<float> cols = new List<float>();
            for (float i = minLon; i <= maxLon; i += step)
                cols.Add(i);

            List<float> rows = new List<float>();
            for (float i = maxLat; i >= minLat; i -= step)
                rows.Add(i);


            List<float> levels = new List<float>();
            for (float i = wdp.MinMax.Min; i <= wdp.MinMax.Max; i += wdp.LineSpacing)
                levels.Add(i);

            var pal = OxyPalette.Interpolate(levels.Count, wdp.ColorSteps.ToArray());

            List<OxyColor> lineColors = new List<OxyColor>();
            foreach (OxyColor c in wdp.ColorSteps)
            {
                if (heatmap)
                {
                    switch (wdp.LineColor.ColorMode)
                    {
                        case Views.LineColorMode.FixedColor:
                            lineColors.Add(wdp.LineColor.Color);
                            break;

                        case Views.LineColorMode.Best_Contrast:
                            lineColors.Add(c.Complementary());
                            break;

                        case Views.LineColorMode.Black_And_White:
                            {
                                System.Drawing.Color cw = System.Drawing.Color.FromArgb(c.R, c.G, c.B);
                                float br = cw.GetBrightness();

                                if (br < 0.5f)
                                    lineColors.Add(OxyColors.White);
                                else
                                    lineColors.Add(OxyColors.Black);
                            }
                            break;
                    }
                }
                else
                    lineColors.Add(c);
            }

            if (isWindMap)
            {
                this.FileTitle += "_WINDMAP";

                var D = (App.ControlPanelModel.SelectedViewport.MaxLon -
                    App.ControlPanelModel.SelectedViewport.MinLon);

                float hf = 1;
                if (D > 200)
                    hf = 0.45f;
                else if (D > 20)
                    hf = 0.55f;
                else
                    hf = 1;

                float sf = 0.9f * hf;

                DenseMatrix[] gr = fieldMatrix.ToWindComponents();
                float[,] dataX = gr[Direction.X].ToArray();
                float[,] dataY = gr[Direction.Y].ToArray();

                int rowCount = dataX.GetLength(0);
                int colCount = dataX.GetLength(1);

                for (int r = 0; r < rowCount; r++)
                {
                    for (int c = 0; c < colCount; c++)
                    {
                        float x = c + App.ControlPanelModel.SelectedViewport.MinLon;
                        float y = App.ControlPanelModel.SelectedViewport.MaxLat - r;

                        float dx = dataX[r, c];
                        float dy = -dataY[r, c];
                        
                        int mod = (int)Math.Sqrt(dx*dx + dy*dy);

                        LineSeries line = new LineSeries();
                        line.Points.Add(new DataPoint(x, y));
                        line.Points.Add(new DataPoint(x + dx, y + dy));
                        line.StrokeThickness = 1;
                        

                        if (mod < 2)
                        {
                            line.Color = OxyColors.Green;
                            line.StrokeThickness = 2 * hf;
                        }
                        else if (mod < 5)
                        {
                            line.Color = OxyColors.Red;
                            line.StrokeThickness = 2.5 * hf;
                        }
                        else
                        {
                            line.Color = OxyColors.Magenta;
                            line.StrokeThickness = 3 * hf;
                        }

                        model.Series.Add(line);

                        ArrowAnnotation arrow = new ArrowAnnotation();
                        var edy = Math.Min(dy, 2);
                        arrow.StartPoint = new DataPoint(x + sf * dx, y + sf * edy);
                        arrow.EndPoint = new DataPoint(x + dx, y + edy);
                        arrow.Color = line.Color;
                        arrow.StrokeThickness = line.StrokeThickness;
                        arrow.HeadWidth = 1.5 * line.StrokeThickness;
                        arrow.HeadLength = 3 * line.StrokeThickness;

                        model.Annotations.Add(arrow);

                        //goto MapFeatures;
                    }
                }
            }
            else
            {
                if (heatmap)
                {
                    if (precipitationMap)
                    {
                        HeatMapSeriesEx cloudMapSeries = new HeatMapSeriesEx
                        {
                            Data = data.ToDoubleArray(),
                            X0 = cols[0],
                            X1 = cols[cols.Count - 1],
                            Y0 = rows[0],
                            Y1 = rows[rows.Count - 1],
                        };
                        model.Series.Add(cloudMapSeries);
                    }
                    else
                    {
                        OxyPlot.Series.HeatMapSeries heatmapSeries = new OxyPlot.Series.HeatMapSeries
                        {
                            Data = data.ToDoubleArray(),
                            X0 = cols[0],
                            X1 = cols[cols.Count - 1],
                            Y0 = rows[0],
                            Y1 = rows[rows.Count - 1],
                        };
                        model.Series.Add(heatmapSeries);
                    }
                }

                if (contours)
                {
                    OxyPlot.Series.ContourSeries contour = new OxyPlot.Series.ContourSeries
                    {
                        ColumnCoordinates = cols.ToArray().ToDoubleArray(),
                        RowCoordinates = rows.ToArray().ToDoubleArray(),
                        ContourLevels = levels.ToArray().ToDoubleArray(),
                        
                        ContourColors = lineColors.ToArray(), // Same # elements as the levels' array
                        
                        Data = (data2 == null) ? data.ToDoubleArray() : data2.ToDoubleArray(),

                        LabelBackground = OxyColors.Transparent,
                        ContourLevelStep = wdp.LineSpacing,
                        StrokeThickness = wdp.LineWidth,
                        FontSize = 15,
                        FontWeight = 500,

                        
                    };


                    model.Series.Add(contour);
                }
            }

MapFeatures:

            // Always do this last.
            AddMapFeatures(model, wdp, pal, isWindMap);

            
        }

        #region Map Features (coastlines, country borders etc)
        private void AddMapFeatures(PlotModel model, WeatherDataPalette wdp, OxyPalette pal, bool isWindMap)
        {
            LineSeries line = null;

            string[] lines = File.ReadAllLines("Coastline.thd");
            foreach (string s in lines)
            {
                if (s.StartsWith("#"))
                    continue;

                if (s.StartsWith(">>"))
                {
                    if (line != null)
                        model.Series.Add(line);

                    line = new LineSeries();
                    line.CanTrackerInterpolatePoints = true;
                    line.Color = OxyColors.Gray;
                    continue;
                }

                Point pt = Point.Parse(s);
                line.Points.Add(new DataPoint(pt.X, pt.Y));
            }

            if (line != null)
                model.Series.Add(line);
            line = null;

            line = null;

            lines = File.ReadAllLines("ContourRO.thd");
            foreach (string s in lines)
            {
                if (s.StartsWith("#"))
                    continue;

                if (s.StartsWith(">>"))
                {
                    if (line != null)
                        model.Series.Add(line);

                    line = new LineSeries();
                    line.CanTrackerInterpolatePoints = true;
                    line.Color = OxyColors.Maroon;
                    line.StrokeThickness = 4;
                    continue;
                }

                Point pt = Point.Parse(s);
                line.Points.Add(new DataPoint(pt.X, pt.Y));
            }

            if (line != null)
                model.Series.Add(line);


            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Unit = "° Latitude",
                Position = OxyPlot.Axes.AxisPosition.Left,
                FilterMinValue = App.ControlPanelModel.SelectedViewport.MinLat,
                FilterMaxValue = App.ControlPanelModel.SelectedViewport.MaxLat,
                AxisTitleDistance = 10,
                
                //MajorGridlineStyle = LineStyle.Solid,
                //MajorStep = 0.5,
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Unit = "° Longitude",
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                FilterMinValue = App.ControlPanelModel.SelectedViewport.MinLon,
                FilterMaxValue = App.ControlPanelModel.SelectedViewport.MaxLon,
                AxisTitleDistance = 10,
                AxislineStyle = LineStyle.Solid,

                //MajorGridlineStyle = LineStyle.Solid,
                //MajorStep = 0.5,
            });

            model.Axes[0].Minimum = App.ControlPanelModel.SelectedViewport.MinLat;
            model.Axes[0].Maximum = App.ControlPanelModel.SelectedViewport.MaxLat;
            model.Axes[1].Minimum = App.ControlPanelModel.SelectedViewport.MinLon;
            model.Axes[1].Maximum = App.ControlPanelModel.SelectedViewport.MaxLon;

            //if (isWindMap)
            //{
            //}
            //else
            {
                model.Axes.Add(new OxyPlot.Axes.LinearColorAxis
                {
                    Unit = wdp.Unit,
                    Position = OxyPlot.Axes.AxisPosition.Right,
                    Palette = pal,
                    HighColor = OxyColors.Black,
                    LowColor = OxyColors.Black,
                    FilterMinValue = wdp.MinMax.Min,
                    FilterMaxValue = wdp.MinMax.Max,
                    Minimum = wdp.MinMax.Min,
                    Maximum = wdp.MinMax.Max,
                    AxisDistance = 5,
                    AxisTitleDistance = 5,
                    AxislineThickness = 100,
                });
            }

            model.PlotMargins = new OxyThickness(40, 0, 50, 40);
            model.TitleFontSize = 14;

            //model.PlotType = PlotType.Polar;

            if (App.ControlPanelModel.SelectedViewport.Name == "Romania")
            {
                if (_roCounties == null)
                {
                    _roCounties = new ImageAnnotation
                    {
                        ImageSource = new OxyImage(File.ReadAllBytes("RO4.png")),

                        X = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                        Width = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),

                        Y = new PlotLength(0.51, PlotLengthUnit.RelativeToPlotArea),
                        Height = new PlotLength(0.8, PlotLengthUnit.RelativeToPlotArea),

                        HorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
                        VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                    };
                }

                model.Annotations.Add(_roCounties);
            }

            
        }
        #endregion
    }

    public static class ArrayExtensions
    {
        public static double[] ToDoubleArray(this float[] input)
        {
            return Array.ConvertAll(input, x => (double)x);
        }

        public static double[,] ToDoubleArray(this float [,] input)
        {
            double[,] output = new double[input.GetLength(0), input.GetLength(1)];
            for (int r = 0; r < input.GetLength(0); r++)
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    output[r, c] = (double)input[r, c];
                }

            return output;
        }
    }
}
