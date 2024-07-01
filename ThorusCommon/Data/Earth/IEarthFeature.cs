using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThorusCommon.Data
{
    public interface IEarthFeature
    {
        EarthModel Earth { get; set; }

        void RebuildState();

        void Save(string title);
        void SaveStats(string title, string category);
    }
}
