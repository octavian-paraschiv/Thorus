using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Data;

namespace ThorusCommon.Engine
{
    public class SimDateTimeRangeStats
    {
        public Atmosphere MeanValues { get; set; }
        public Atmosphere MinValues { get; set; }
        public Atmosphere MaxValues { get; set; }

        public SurfaceLevel MeanValuesSFC { get; set; }
        public SurfaceLevel MinValuesSFC { get; set; }
        public SurfaceLevel MaxValuesSFC { get; set; }

        public SimDateTimeRangeStats(EarthModel earth)
        {
            MeanValues = new Atmosphere(earth, false, float.Epsilon);
            MinValues = new Atmosphere(earth, false, float.MaxValue);
            MaxValues = new Atmosphere(earth, false, float.MinValue);

            MeanValuesSFC = new SurfaceLevel(earth, false, float.Epsilon);
            MinValuesSFC = new SurfaceLevel(earth, false, float.MaxValue);
            MaxValuesSFC = new SurfaceLevel(earth, false, float.MinValue);
        }

        public void AdjustMeanValues(int rangeSize)
        {
            MeanValues.SeaLevel.P.DIV(rangeSize);
            MeanValues.SeaLevel.T.DIV(rangeSize);
            MeanValues.SeaLevel.H.DIV(rangeSize);

            MeanValues.MidLevel.P.DIV(rangeSize);
            MeanValues.MidLevel.T.DIV(rangeSize);
            MeanValues.MidLevel.H.DIV(rangeSize);

            MeanValues.TopLevel.P.DIV(rangeSize);
            MeanValues.TopLevel.T.DIV(rangeSize);
            MeanValues.TopLevel.H.DIV(rangeSize);

            MeanValues.JetLevel.P.DIV(rangeSize);
            MeanValues.JetLevel.T.DIV(rangeSize);
            MeanValues.JetLevel.H.DIV(rangeSize);

            MeanValues.AirMass.DIV(rangeSize);

            MeanValuesSFC.TE.DIV(rangeSize);
            MeanValuesSFC.TW.DIV(rangeSize);
            MeanValuesSFC.TL.DIV(rangeSize);
            MeanValuesSFC.TS.DIV(rangeSize);

            MeanValuesSFC.SNOW.DIV(rangeSize);
            MeanValuesSFC.RAIN.DIV(rangeSize);
            
            MeanValuesSFC.BLIZZARD.DIV(rangeSize);

            MeanValuesSFC.ALBEDO.DIV(rangeSize);

            MeanValuesSFC.Precip.DIV(rangeSize);

            MeanValuesSFC.LIDX.DIV(rangeSize);

            MeanValuesSFC.THigh.DIV(rangeSize);
            MeanValuesSFC.TLow.DIV(rangeSize);

            MeanValuesSFC.TNormHigh.DIV(rangeSize);
            MeanValuesSFC.TNormLow.DIV(rangeSize);
        }

        public void ProcessAtmSnapshot(Atmosphere atm)
        {
            MeanValues.Add(atm);
            MinValues.GetMin(atm);
            MaxValues.GetMax(atm);
        }

        public void Save(string title)
        {
            MeanValues.SaveStats(title, "AVG");
            MinValues.SaveStats(title,  "MIN");
            MaxValues.SaveStats(title,  "MAX");

            MeanValuesSFC.SaveStats(title, "AVG");
            MinValuesSFC.SaveStats(title, "MIN");
            MaxValuesSFC.SaveStats(title, "MAX");
        }

        public void ProcessSfcSnapshot(SurfaceLevel sfc)
        {
            MeanValuesSFC.Add(sfc);
            MinValuesSFC.GetMin(sfc);
            MaxValuesSFC.GetMax(sfc);
        }
    }
}
