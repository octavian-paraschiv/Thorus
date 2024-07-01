using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot.Series;

namespace ThorusViewer.Series
{
    public class HeatMapSeriesEx : HeatMapSeries
    {
        public float[,] OData { get; set; }
        
    }
}
