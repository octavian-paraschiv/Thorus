﻿using System;
using ThorusCommon.Thermodynamics;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace ThorusCommon.Data
{
    public class VariableJet_SeasonalReversal : JetLevel
    {
        public VariableJet_SeasonalReversal(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, loadFromStateFiles, defaultValue)
        {
        }

        protected override void RebuildJetState(DenseMatrix[] ridgePatternDevs, DenseMatrix BP, DenseMatrix FP,
            float daysElapsed, float dailyJetAdvance, float deltaLonRad)
        {
            _actualDev.Assign2D
            (
                // X-Direction deviations (longitudinal)
                (r, c) =>
                {
                    float lat = EarthModel.MaxLat - r;
                    float latRad = lat * (float)Math.PI / 180;

                    var fSingle = JetModelFunctions.SingleJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);
                    var fDual = JetModelFunctions.DualJet_SeasonalReversal(Earth.UTC.DayOfYear, latRad);

                    var fVar = JetModelFunctions.GetVariability(daysElapsed);
                    var f = (1 - fVar) * fSingle + fVar * fDual;

                    var devX1 = f * dailyJetAdvance;


                    float ridgeDevX = ridgePatternDevs[Direction.X][r, c];
                    var devX = JetModelFunctions.ComposeDevs(Direction.X, devX1, ridgeDevX);
                    return ((Earth.SnapshotDivFactor * devX) % 360);
                },

                 // Y-Direction deviations (latitudinal)
                 (r, c) =>
                 {
                     float lat = EarthModel.MaxLat - r;
                     float lon = c - 180;
                     float ridgeDevY = ridgePatternDevs[Direction.Y][r, c];

                     return JetModelFunctions.DevY_VariableJet(Earth.HoursElapsed, Earth.SnapshotDivFactor,
                         ridgeDevY, lat, lon);
                 }
            );
        }
    }
}
