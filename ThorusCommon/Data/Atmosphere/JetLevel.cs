using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace ThorusCommon.Data
{
    public class JetLevel : AtmosphericLevel
    {
        static readonly float JetDevFactor = 0.5f;
        static readonly float RidgeDevFactor = 1 - JetDevFactor;

        protected override float[] PressureExtremes
        {
            get
            {
                return LevelPressureExtremes.TopLevelExtremes;
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

            P = ((Earth.ATM.TopLevel.P.Multiply(100 / LevelPressure.TopLevelPressure) +
                                Earth.ATM.MidLevel.P.Multiply(100 / LevelPressure.MidLevelPressure) +
                                Earth.ATM.SeaLevel.P.Multiply(100 / LevelPressure.SeaLevelPressure)) as DenseMatrix).EQ();

            var _ridgePatternDevs = P.ToWindComponents();

            _actualDev.Assign2D
            (
                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    float lon = c - 180;
                    float lonRad = lon * (float)Math.PI / 180;
                    float latRad = lat * (float)Math.PI / 180;

                    float sinLon = (float)Math.Sin(lonRad);

                    float sinLat = (float)Math.Sin(latRad);
                    float cosLat = (float)Math.Cos(latRad);

                    float sin2Lat = (float)Math.Sin(2 * latRad);
                    float cos2Lat = (float)Math.Cos(2 * latRad);

                    float f = 0;

                    switch (SimulationParameters.Instance.JetStreamPattern)
                    {
                        case SimulationParameters.JetStreamPatterns.SingleJet_NoReversal:
                            // SIngle jet with no reversal. Theoretical model only.
                            // Should be used just for calibration.
                            f = (float)(Math.Abs(sinLat * cosLat));
                            break;

                        case SimulationParameters.JetStreamPatterns.DualJet_NoReversal:
                            // Dual jet with no reversal. Theoretical model only.
                            // Should be used just for calibration.
                            f = (float)Math.Abs(sin2Lat * cos2Lat);
                            break;

                        case SimulationParameters.JetStreamPatterns.SingleJet_WithReversal:
                            // Single jet with reversal at Poles and Ecuator
                            // This model should be used during cold season only.
                            // In cold season, tropical jets are extremely weak, only polar jets count.
                            f = 2 * (float)Math.Abs(sinLat * cosLat) - 0.5f;
                            break;

                        case SimulationParameters.JetStreamPatterns.DualJet_WithReversal:
                            // Dual jet with reversal at Tropics and Polar Circles
                            // This model should be used during warm season only.
                            // In warm season, tropical are comparable to polar jets.
                            f = -2 * (float)Math.Abs(sin2Lat * cos2Lat) + 0.5f;
                            break;

                        case SimulationParameters.JetStreamPatterns.Variable_WithReversal:
                            {
                                float varSeed = SimulationParameters.Instance.JetStreamVariabilitySeed;
                                float varPeriod = SimulationParameters.Instance.JetStreamVariabilityPeriod;
                                float fVariability = (float)Math.Sin((2 * (float)Math.PI * (varSeed / 4 + daysElapsed / varPeriod)));

                                var fSingle = 2 * (float)Math.Abs(sinLat * cosLat) - 0.5f;
                                var fDual = -2 * (float)Math.Abs(sin2Lat * cos2Lat) + 0.5f;

                                f = (1 - fVariability) * fSingle + fVariability * fDual;
                            }
                            break;
                    }

                    var devX1 = f * dailyJetAdvance;

                    var devX = Earth.SnapshotDivFactor * (
                        JetDevFactor * devX1 +
                        RidgeDevFactor * _ridgePatternDevs[Direction.X][r, c]);

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

                    float f = (float)(Math.Abs(sinLat * cosLat) * sinLon) * Math.Sign(lat);

                    var devY1 = f * SimulationParameters.Instance.JetStreamWaveSpeed;

                    var devY = Earth.SnapshotDivFactor * (
                        JetDevFactor * devY1 +
                        RidgeDevFactor * _ridgePatternDevs[Direction.Y][r, c]);

                    return (devY % 180);
                }
            );

        }

        public override void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(P, title, string.Format("P_{0:d2}_MAP", _levelType), category);
        }
    }
}
