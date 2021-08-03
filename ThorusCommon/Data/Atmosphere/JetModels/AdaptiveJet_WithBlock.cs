using System;
using ThorusCommon.Thermodynamics;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace ThorusCommon.Data
{
    public class AdaptiveJet_WithBlock : JetLevel
    {
        public AdaptiveJet_WithBlock(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
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
                    var bp = BP[r, c];
                    var devX1 = (1 - 0.5f * bp) * dailyJetAdvance;

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

                     return JetModelFunctions.DevY_InvariableJet(Earth.HoursElapsed, Earth.SnapshotDivFactor,
                         ridgeDevY, lat, lon);
                 }
            );
        }
    }
}
