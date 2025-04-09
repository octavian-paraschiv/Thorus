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
        static readonly float dx = 0.5f;

        static readonly float JetDevFactor_X = dx;
        static readonly float JetDevFactor_Y = dx;

        static readonly float RidgeDevFactor_X = 1 - JetDevFactor_X;
        static readonly float RidgeDevFactor_Y = 1 - JetDevFactor_Y;

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

            P = ((Earth.ATM.SeaLevel.P.Multiply(45 / LevelPressure.SeaLevelPressure) +
                  Earth.ATM.TopLevel.P.Multiply(120 / LevelPressure.TopLevelPressure) +
                  Earth.ATM.MidLevel.P.Multiply(135 / LevelPressure.MidLevelPressure)) as DenseMatrix).EQ();

            var ridgePatternDevs = P.ToWindComponents();

            var this_BP = this.BP;
            var this_FP = this.FP;

            var waveNumbers = P.RossbyWaveNumbers();

            _actualDev.Assign2D
            (
                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    int waveNumber = lat >= 0 ? waveNumbers[0] : waveNumbers[1];
                    float waveSpeed = 360f / (waveNumber * waveNumber);

                    const float subtrop = 15f;
                    float latRad1 = (lat - subtrop) * (float)Math.PI / 180;
                    float latRad2 = (lat + subtrop) * (float)Math.PI / 180;

                    float f = 1f;// (float)(Math.Sin(latRad1) * Math.Sin(latRad2));

                    var devX1 = f * waveSpeed;

                    var devX = Earth.SnapshotDivFactor * (
                        JetDevFactor_X * devX1 +
                        RidgeDevFactor_X * ridgePatternDevs[Direction.X][r, c]);

                    return (devX % 360);
                },

                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    int waveNumber = lat >= 0 ? waveNumbers[0] : waveNumbers[1];
                    float waveSpeed = 360f / (waveNumber * waveNumber);
                    float deltaLonRad = daysElapsed * waveSpeed * (float)Math.PI / 180;

                    float lon = c - 180;

                    float lonRad = lon * (float)Math.PI / 180;
                    float latRad = lat * (float)Math.PI / 180;

                    float sinLat = (float)Math.Sin(latRad);
                    float cosLat = (float)Math.Cos(latRad);
                    float sinLon = (float)Math.Sin(waveNumber * (lonRad - deltaLonRad));

                    float f = (sinLat * cosLat * sinLon) * Math.Sign(lat);

                    var devY1 = f * waveSpeed;

                    var devY = Earth.SnapshotDivFactor * (
                        JetDevFactor_Y * devY1 +
                        RidgeDevFactor_Y * ridgePatternDevs[Direction.Y][r, c]);

                    return (devY % 180);
                }
            );
        }

        public override void Save(string title)
        {
            base.Save(title);
            FileSupport.Save(BP, Earth.UTC.Title, "D_BP_MAP");
            FileSupport.Save(FP, Earth.UTC.Title, "D_FP_MAP");
        }

        public override void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(P, title, string.Format("P_{0:d2}_MAP", _levelType), category);
            FileSupport.SaveAsStats(BP, title, "D_BP_MAP", category);
            FileSupport.SaveAsStats(FP, title, "D_FP_MAP", category);
        }
    }
}
