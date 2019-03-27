﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Plugins;
using Thorus.PluginsApi;

namespace ThorusCommon.Data
{
    public class JetLevel : AtmosphericLevel
    {
        static readonly float dx = 0.5f;
        static readonly float JetDevFactor_X = dx;
        static readonly float JetDevFactor_Y = dx;

        static readonly float RidgeDevFactor_X = 1 - JetDevFactor_X;
        static readonly float RidgeDevFactor_Y = 1 - JetDevFactor_Y;

        IJetPlugin plugin = null;

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
            if (SimulationParameters.Instance.JetStreamPattern == SimulationParameters.JetStreamPatterns.Plugin)
                plugin = JetPluginInstance.Build(SimulationParameters.Instance.JetStreamPluginName);
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

            FileSupport.Save(BP, Earth.UTC.Title, "D_BP");
            FileSupport.Save(FP, Earth.UTC.Title, "D_FP");

            DenseMatrix[] extDev = null;

            var pattern = SimulationParameters.Instance.JetStreamPattern;
            if (SimulationParameters.Instance.JetStreamPattern == SimulationParameters.JetStreamPatterns.Plugin)
            {
                extDev = CalcDevs_ByPlugin();
                if (extDev == null)
                {
                    // fallback
                    Console.WriteLine("Jet stream model plugin is failing => fallback to Variable_SeasonalReversal model...");
                    pattern = SimulationParameters.JetStreamPatterns.Variable_SeasonalReversal;
                }
            }

            _actualDev.Assign2D
            (
                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    float lon = c - 180;
                    float latRad = lat * (float)Math.PI / 180;

                    float f = 0;
                    float devE = 0;

                    var devX = 0f;

                    switch (pattern)
                    {
                        case SimulationParameters.JetStreamPatterns.SingleJet_WithReversal:
                            {
                                f = SingleJet_WithReversal(Earth.UTC.DayOfYear, latRad);
                                var devX1 = f * dailyJetAdvance;
                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);

                            }
                            break;

                        case SimulationParameters.JetStreamPatterns.DualJet_WithReversal:
                            {
                                f = DualJet_WithReversal(Earth.UTC.DayOfYear, latRad);
                                var devX1 = f * dailyJetAdvance;
                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);
                            }
                            break;

                        case SimulationParameters.JetStreamPatterns.SingleJet_SeasonalReversal:
                            {
                                f = SingleJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);
                                var devX1 = f * dailyJetAdvance;
                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);
                            }
                            break;

                        case SimulationParameters.JetStreamPatterns.DualJet_SeasonalReversal:
                            {
                                f = DualJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);
                                var devX1 = f * dailyJetAdvance;
                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);
                            }
                            break;

                        case SimulationParameters.JetStreamPatterns.Plugin:
                            devX = extDev[Direction.X][r, c];
                            break;

                        case SimulationParameters.JetStreamPatterns.Variable_WithReversal:
                            {
                                var fSingle = SingleJet_WithReversal(Earth.UTC.DayOfYear, latRad);
                                var fDual =   DualJet_WithReversal(Earth.UTC.DayOfYear, latRad);

                                var fVar = GetVariability(daysElapsed);
                                f = (1 - fVar) * fSingle + fVar * fDual;

                                var devX1 = f * dailyJetAdvance;
                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);
                            }
                            break;


                        case SimulationParameters.JetStreamPatterns.Variable_SeasonalAndBlock_Reversal:
                            {
                                var fSingle = SingleJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);
                                var fDual = DualJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);

                                var fVar = GetVariability(daysElapsed);
                                f = (1 - fVar) * fSingle + fVar * fDual;

                                var bp = BP[r, c];

                                var devX1 = (f - 0.5f * bp) * dailyJetAdvance;

                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);
                            }
                            break;

                        case SimulationParameters.JetStreamPatterns.Variable_SeasonalReversal:
                        default:
                            {
                                var fSingle = SingleJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);
                                var fDual = DualJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);

                                var fVar = GetVariability(daysElapsed);
                                f = (1 - fVar) * fSingle + fVar * fDual;

                                var devX1 = f * dailyJetAdvance;
                                devX = Earth.SnapshotDivFactor * (
                                    JetDevFactor_X * (devX1 + devE) +
                                    RidgeDevFactor_X * _ridgePatternDevs[Direction.X][r, c]);
                            }
                            break;
                    }

                    return (devX % 360);
                },

                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    float lon = c - 180;

                    float lonRad = lon * (float)Math.PI / 180;

                    float latRad = lat * (float)Math.PI / 180;
                    //float latRad = lat * (float)Math.PI / 180 + References.GetSunLatitude_Radians(Earth.UTC);

                    float sinLat = (float)Math.Sin(latRad);
                    float cosLat = (float)Math.Cos(latRad);
                    float sinLon = (float)Math.Sin(SimulationParameters.Instance.JetStreamPeaks * (lonRad - deltaLonRad));

                    var devY = 0f;

                    switch (pattern)
                    {
                        case SimulationParameters.JetStreamPatterns.Plugin:
                            devY = extDev[Direction.Y][r, c];
                            break;

                        default:
                            {
                                float f = sinLat * cosLat * sinLon * GetVariability(daysElapsed);

                                var devY1 = f * SimulationParameters.Instance.JetStreamWaveSpeed;

                                devY = Earth.SnapshotDivFactor * (
                                    JetDevFactor_Y * devY1 +
                                    RidgeDevFactor_Y * _ridgePatternDevs[Direction.Y][r, c]);
                            }
                            break;
                    }

                    return (devY % 180);
                }
            );
        }

        private DenseMatrix[] CalcDevs_ByPlugin()
        {
            DenseMatrix[] ret = null;

            try
            {
                if (plugin != null)
                {
                    var grad = P.Gradient();
                    ret = plugin.GetJetDeviations(grad[Direction.X], grad[Direction.Y]);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        private static float SingleJet_WithReversal(int dayOfYear, float latRad)
        {
            float sinLat = (float)Math.Sin(latRad);
            float cosLat = (float)Math.Cos(latRad);
            return 2 * (float)Math.Abs(sinLat * cosLat) - 0.5f;
        }

        private static float DualJet_WithReversal(int dayOfYear, float latRad)
        {
            float sin2Lat = (float)Math.Sin(2 * latRad);
            float cos2Lat = (float)Math.Cos(2 * latRad);
            return -2 * (float)Math.Abs(sin2Lat * cos2Lat) + 0.5f;
        }



        private static float SingleJet_SeasonalReversal(int dayOfYear, float latRad)
        {
            float seasonDelta = 18f * (float)Math.Cos(2 * Math.PI * ((double)dayOfYear + 10) / (double)365);
            float seasonDeltaRad = (float)(seasonDelta * Math.PI / (double)180);
            return (float)(Math.Sin(2.5f * (latRad + seasonDeltaRad)) * Math.Sign(latRad));
        }

        private static float DualJet_SeasonalReversal(int dayOfYear, float latRad)
        {
            float seasonDelta = 18f * (float)Math.Cos(2 * Math.PI * ((double)dayOfYear + 10) / (double)365);
            float seasonDeltaRad = (float)(seasonDelta * Math.PI / (double)180);
            return -(float)Math.Cos(5 * (latRad + seasonDeltaRad));
        }

        private static float GetVariability(float daysElapsed)
        {
            float varSeed = SimulationParameters.Instance.JetStreamVariabilitySeed;
            float varPeriod = SimulationParameters.Instance.JetStreamVariabilityPeriod;
            return (float)Math.Sin((2 * (float)Math.PI * (varSeed / 4 + daysElapsed / varPeriod)));
        }

        public override void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(P, title, string.Format("P_{0:d2}_MAP", _levelType), category);
            FileSupport.SaveAsStats(BP, title, "D_BP_MAP", category);
            FileSupport.SaveAsStats(FP, title, "D_FP_MAP", category);
        }
    }
}
