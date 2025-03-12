using System.Collections.Generic;
using ThorusCommon.Data;

namespace ThorusCommon.Export
{
    public class Viewport
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public float MinLat { get; private set; }
        public float MaxLat { get; private set; }
        public float MinLon { get; private set; }
        public float MaxLon { get; private set; }
        public float DataStep { get; private set; }

        public float AspectRatio
        {
            get
            {
                float h = MaxLat - MinLat;
                float w = MaxLon - MinLon;

                if (h == 0)
                    h = 1;

                return w / h;
            }
        }

        public override string ToString()
            => $"{Name} [{Code}] Lat=[{MinLat}..{MaxLat}], Lon=[{MinLon}..{MaxLon}]";

        private static Viewport[] _viewports = LoadViewports();
        public static Viewport[] AllViewports => _viewports;

        private static Viewport[] LoadViewports()
        {
            var viewports = new List<Viewport>
            {
                new Viewport("Entire World", "EWR", EarthModel.MinLat, EarthModel.MaxLat, EarthModel.MinLon, EarthModel.MaxLon),
                new Viewport("Northern Hemisphere", "NHM", 0, EarthModel.MaxLat, EarthModel.MinLon, EarthModel.MaxLon),
                new Viewport("Southern Hemisphere", "SHM", EarthModel.MinLat, 0, EarthModel.MinLon, EarthModel.MaxLon),
                new Viewport("Europe", "EU", 25, EarthModel.MaxLat, -30, 65, 2),
                new Viewport("Romania", "RO", 43, 49, 20, 30, 2),
                new Viewport("Africa", "AF", -35, 40, -55, 95, 2),
                new Viewport("N America", "NAM", 0, EarthModel.MaxLat, EarthModel.MinLon, -30, 2),
                new Viewport("N Asia", "NAS", 0, EarthModel.MaxLat, 65, EarthModel.MaxLon, 2),
                new Viewport("N Atlantic", "NAT", 0, EarthModel.MaxLat, -75, 20, 2),
                new Viewport("E.Europe and Russia", "EER", 25, EarthModel.MaxLat, 20, 115, 2),
                new Viewport("S America", "SAM", -60, 15, -105, -25, 2),
                new Viewport("S Atlantic", "SAT", EarthModel.MinLat, 0, -70, 30, 2),
                new Viewport("Australia + Indonesia", "AUI", -50, 10, 90, EarthModel.MaxLon, 2)
            };

            return viewports.ToArray();

        }

        public Viewport(string name, string code, float minLat, float maxLat, float minLon, float maxLon, float dataStep = 5)
        {
            Name = name;
            Code = code;
            MinLat = minLat;
            MaxLat = maxLat;
            MinLon = minLon;
            MaxLon = maxLon;
            DataStep = dataStep;
        }
    }
}
