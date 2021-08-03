﻿using System;
using ThorusCommon.Thermodynamics;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.IO;
using FastFluidSolver;

namespace ThorusCommon.Data
{
    public class Adaptive_FastFluidDynamicsJet : Ffd_JetLevel
    {
        public Adaptive_FastFluidDynamicsJet(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, loadFromStateFiles, defaultValue)
        {
        }

        protected override void RebuildJetState(DenseMatrix[] ridgePatternDevs, DenseMatrix BP, DenseMatrix FP,
            float daysElapsed, float dailyJetAdvance, float deltaLonRad)
        {
            try
            {
                var ffd = RunFfdStep();

                _actualDev.Assign2D
                (
                    // X-Direction deviations (longitudinal)
                    (r, c) =>
                    {
                        var devX1 = dailyJetAdvance;

                        float ridgeDevX = (float)ffd.u[c + 1, r + 1, 1] + ridgePatternDevs[Direction.X][r, c];

                        var devX = JetModelFunctions.ComposeDevs(Direction.X, devX1, ridgeDevX);
                        return ((Earth.SnapshotDivFactor * devX) % 360);
                },

                 // Y-Direction deviations (latitudinal)
                 (r, c) =>
                 {
                     float lat = EarthModel.MaxLat - r;
                     float lon = c - 180;

                     float ridgeDevY = (float)ffd.v[c + 1, r + 1, 1] + ridgePatternDevs[Direction.Y][r, c];

                     return JetModelFunctions.DevY_InvariableJet(Earth.HoursElapsed, Earth.SnapshotDivFactor,
                        ridgeDevY, lat, lon);
                 }
            );
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}


