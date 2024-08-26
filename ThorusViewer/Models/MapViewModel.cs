using MathNet.Numerics.LinearAlgebra.Single;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ThorusCommon;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusViewer.Palettes;
using ThorusViewer.Pallettes;
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

        private readonly Control _parent = null;

        public void LoadWeatherFieldData(string fieldFileName)
        {
            ReloadModel(fieldFileName, false);
            this.Model.InvalidatePlot(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MapViewModel(Control parent)
        {
            _parent = parent;
            ControlPanelModel.Instance.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ControlPanelModel_PropertyChanged);
            this.Model = new PlotModel();
        }

        void ControlPanelModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case "SelectedViewport":
                //    {
                //        this.Model.Axes[0].Minimum = ControlPanelModel.Instance.SelectedViewport.MinLat;
                //        this.Model.Axes[0].Maximum = ControlPanelModel.Instance.SelectedViewport.MaxLat;
                //        this.Model.Axes[1].Minimum = ControlPanelModel.Instance.SelectedViewport.MinLon;
                //        this.Model.Axes[1].Maximum = ControlPanelModel.Instance.SelectedViewport.MaxLon;
                //        this.Model.Title = ControlPanelModel.Instance.SelectedViewport.Name;

                //        if (ControlPanelModel.Instance.SelectedViewport.Name == "Romania" &&
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
                        if (ControlPanelModel.Instance.SelectedSnapshot != null)
                        {
                            SimDateTime snapshot = ControlPanelModel.Instance.SelectedSnapshot;
                            string folder = SimulationData.DataFolder;
                            if (string.IsNullOrEmpty(ControlPanelModel.Instance.SelectedCategory) == false)
                                folder = Path.Combine(SimulationData.DataFolder, ControlPanelModel.Instance.SelectedCategory);

                            string fieldDataFileName = string.Format("{0}_MAP_{1}.thd",
                                ControlPanelModel.Instance.SelectedDataType.Name,
                                snapshot.Title);

                            string fieldDataFile = System.IO.Path.Combine(folder, fieldDataFileName);

                            ReloadModel(fieldDataFile, ControlPanelModel.Instance.SelectedDataType.IsWindMap);
                            this.Model.InvalidatePlot(true);

                            ControlPanelModel.Instance.DoAutoSave();
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
            catch
            {
            }
        }

        public void RefitMap()
        {
            Viewport v = ControlPanelModel.Instance.SelectedViewport;

            const float AspectRatioThreshold = 2.5f;

            float MaxWidth = (float)_parent.Width;
            float MaxHeight = (float)_parent.Height;

            if (v.AspectRatio > AspectRatioThreshold)
            {
                (this.Model.PlotView as OxyPlot.WindowsForms.PlotView).Height = (int)(MaxWidth / v.AspectRatio);
                (this.Model.PlotView as OxyPlot.WindowsForms.PlotView).Width = (int)MaxWidth;
            }
            else
            {
                (this.Model.PlotView as OxyPlot.WindowsForms.PlotView).Height = (int)MaxHeight;
                (this.Model.PlotView as OxyPlot.WindowsForms.PlotView).Width = (int)(MaxHeight * v.AspectRatio);
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
                ControlPanelModel.Instance.SelectedViewport.Name,
                ControlPanelModel.Instance.SelectedDataType.Value,
                fileTitle,
                ControlPanelModel.Instance.SelectedDataType.Comments);

            DenseMatrix m = null;

            int minLon = ControlPanelModel.Instance.SelectedViewport.MinLon.Round();
            int maxLon = ControlPanelModel.Instance.SelectedViewport.MaxLon.Round();
            int minLat = ControlPanelModel.Instance.SelectedViewport.MinLat.Round();
            int maxLat = ControlPanelModel.Instance.SelectedViewport.MaxLat.Round();

            var fieldMatrix = FileSupport.LoadSubMatrixFromFile(fieldDataFile, minLon, maxLon, minLat, maxLat);

            bool interpolate = (fieldMatrix.RowCount < 10);

            if (interpolate)
                fieldMatrix = fieldMatrix.Interpolate();

            float[,] data = null;
            float[,] data2 = null;

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
                        float cl = Math.Abs(C00[r, c]);

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
                            return PrecipTypeComputer.Compute(

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
            }

            Range<float> minMax = wdp.MinMax;

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
                        case LineColorMode.FixedColor:
                            lineColors.Add(wdp.LineColor.Color);
                            break;

                        case LineColorMode.Best_Contrast:
                            lineColors.Add(c.Complementary());
                            break;

                        case LineColorMode.Black_And_White:
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

                var D = (ControlPanelModel.Instance.SelectedViewport.MaxLon -
                    ControlPanelModel.Instance.SelectedViewport.MinLon);

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
                        float x = step * c + ControlPanelModel.Instance.SelectedViewport.MinLon;
                        float y = ControlPanelModel.Instance.SelectedViewport.MaxLat - step * r;

                        float dx = step * dataX[r, c];
                        float dy = step * -dataY[r, c];

                        int mod = (int)Math.Sqrt(dx * dx + dy * dy);

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
                        CloudMapSeries cloudMapSeries = new CloudMapSeries
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

            // Always do this last.
            AddMapFeatures(model, wdp, pal, isWindMap);


        }

        #region Map Features (coastlines, country borders etc)
        private void AddMapFeatures(PlotModel model, WeatherDataPalette wdp, OxyPalette pal, bool isWindMap)
        {
            _ = isWindMap;

            LineSeries line = null;

            string[] lines = File.ReadAllLines("Data/Coastline.thd");
            foreach (string s in lines)
            {
                if (s.StartsWith("#"))
                    continue;

                if (s.StartsWith(">>"))
                {
                    if (line != null)
                        model.Series.Add(line);

                    line = new LineSeries
                    {
                        CanTrackerInterpolatePoints = true,
                        Color = OxyColors.Black,
                        StrokeThickness = 1
                    };

                    continue;
                }

                line.Points.Add(s.ToDataPoint());
            }

            if (line != null)
                model.Series.Add(line);

            line = null;

            lines = File.ReadAllLines("Data/ContourRO.thd");
            foreach (string s in lines)
            {
                if (s.StartsWith("#"))
                    continue;

                if (s.StartsWith(">>"))
                {
                    if (line != null)
                        model.Series.Add(line);

                    line = new LineSeries
                    {
                        CanTrackerInterpolatePoints = true,
                        Color = OxyColors.Maroon,
                        StrokeThickness = 2
                    };

                    continue;
                }

                line.Points.Add(s.ToDataPoint());
            }

            if (line != null)
                model.Series.Add(line);


            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Unit = "° Latitude",
                Position = OxyPlot.Axes.AxisPosition.Left,
                FilterMinValue = ControlPanelModel.Instance.SelectedViewport.MinLat,
                FilterMaxValue = ControlPanelModel.Instance.SelectedViewport.MaxLat,
                AxisTitleDistance = 3,

                //MajorGridlineStyle = LineStyle.Solid,
                //MajorStep = 0.5,
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Unit = "° Longitude",
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                FilterMinValue = ControlPanelModel.Instance.SelectedViewport.MinLon,
                FilterMaxValue = ControlPanelModel.Instance.SelectedViewport.MaxLon,
                AxisTitleDistance = 3,
                AxislineStyle = LineStyle.Solid,

                //MajorGridlineStyle = LineStyle.Solid,
                //MajorStep = 0.5,
            });

            model.Axes[0].Minimum = ControlPanelModel.Instance.SelectedViewport.MinLat;
            model.Axes[0].Maximum = ControlPanelModel.Instance.SelectedViewport.MaxLat;
            model.Axes[1].Minimum = ControlPanelModel.Instance.SelectedViewport.MinLon;
            model.Axes[1].Maximum = ControlPanelModel.Instance.SelectedViewport.MaxLon;

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
                    AxisTitleDistance = 3,
                    AxislineThickness = 100,
                });
            }

            model.PlotMargins = new OxyThickness(40, 0, 50, 40);
            model.TitleFontSize = 14;

            //model.PlotType = PlotType.Polar;

            if (ControlPanelModel.Instance.SelectedViewport.Name == "Romania")
            {
                if (_roCounties == null)
                {
                    _roCounties = new ImageAnnotation
                    {
                        ImageSource = new OxyImage((byte[])new ImageConverter().ConvertTo(Resources.RO4, typeof(byte[]))),

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

        public static double[,] ToDoubleArray(this float[,] input)
        {
            double[,] output = new double[input.GetLength(0), input.GetLength(1)];
            for (int r = 0; r < input.GetLength(0); r++)
                for (int c = 0; c < input.GetLength(1); c++)
                {
                    output[r, c] = (double)input[r, c];
                }

            return output;
        }

        public static DataPoint ToDataPoint(this string str)
        {
            try
            {
                string[] coords = str.Split(',');
                return new DataPoint(double.Parse(coords[0]), double.Parse(coords[1]));
            }
            catch
            {
                return new DataPoint(0, 0);
            }
        }
    }
}
