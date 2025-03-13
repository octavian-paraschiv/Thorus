using MathNet.Numerics.LinearAlgebra.Single;
using System;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;

namespace ThorusCommon.Data
{
    public class JetLevel : AtmosphericLevel
    {
        static readonly float dx = 0.3f;
        static readonly float JetDevFactor_X = dx;
        static readonly float JetDevFactor_Y = dx;

        static readonly float mr = 5f;
        static readonly float RidgeDevFactor_X = mr * (1 - JetDevFactor_X);
        static readonly float RidgeDevFactor_Y = mr * (1 - JetDevFactor_Y);

        protected override float[] PressureExtremes
        {
            get
            {
                return LevelPressureExtremes.JetLevelExtremes;
            }
        }

        public JetLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, LevelType.JetLevel, loadFromStateFiles, defaultValue)
        {
        }

        public override void Advance()
        {
        }

        public override void RebuildState()
        {
            float sunLatRad = References.GetSunLatitude_Radians(Earth.UTC);
            float daysElapsed = (Earth.HoursElapsed / AbsoluteConstants.HoursPerDay);

            float dailyJetAdvance = SimulationParameters.Instance.JetStreamWaveSpeed;
            float deltaLonRad = daysElapsed * dailyJetAdvance * (float)Math.PI / 180;

            P = ((Earth.ATM.SeaLevel.P.Multiply(100 / LevelPressure.SeaLevelPressure) +
                  Earth.ATM.TopLevel.P.Multiply(100 / LevelPressure.TopLevelPressure) +
                  Earth.ATM.MidLevel.P.Multiply(100 / LevelPressure.MidLevelPressure)) as DenseMatrix).EQ();

            var _ridgePatternDevs = P.ToWindComponents();

            var this_BP = this.BP;
            var this_FP = this.FP;

            FileSupport.Save(this_BP, Earth.UTC.Title, "D_BP");
            FileSupport.Save(this_FP, Earth.UTC.Title, "D_FP");

            _actualDev.Assign2D
            (
                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    float lon = c - 180;
                    float latRad = lat * (float)Math.PI / 180;

                    float f = 1;
                    float devE = 0;

                    var devX1 = f * dailyJetAdvance;

                    var devX = Earth.SnapshotDivFactor * (
                        JetDevFactor_X * (devX1 + devE) +
                        RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);

                    return (devX % 360);
                },

                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    float lon = c - 180;

                    float lonRad = lon * (float)Math.PI / 180;
                    float latRad = lat * (float)Math.PI / 180;

                    float sinLat = (float)Math.Sin(latRad);
                    float cosLat = (float)Math.Cos(latRad);
                    float sinLon = (float)Math.Sin(SimulationParameters.Instance.JetStreamPeaks * (lonRad - deltaLonRad));

                    float f = sinLat * cosLat * sinLon;

                    var devY1 = f * SimulationParameters.Instance.JetStreamWaveSpeed;

                    var devY = Earth.SnapshotDivFactor * (
                        JetDevFactor_Y * devY1 +
                        RidgeDevFactor_Y * _ridgePatternDevs[Direction.Y][r, c]);

                    return (devY % 180);
                }
            );
        }

        public override void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(P, title, string.Format("P_{0:d2}_MAP", _levelType), category);
            FileSupport.SaveAsStats(BP, title, "D_BP_MAP", category);
            FileSupport.SaveAsStats(FP, title, "D_FP_MAP", category);
        }
    }
}
