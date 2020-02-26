using System;
using ThorusCommon.Thermodynamics;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;


namespace ThorusCommon.Data
{
    public class ExperimentalJet : JetLevel
    {
        public ExperimentalJet(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
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
                    float lon = c - 180;
                    float latRad = lat * (float)Math.PI / 180;
                    float lonRad = lon * (float)Math.PI / 180;

                    float ridgeDevX = ridgePatternDevs[Direction.X][r, c];

                    var devX1 = dailyJetAdvance;

                    // Mean state: zonal extratropical jets
                    var devX2 = (float)(25 * Math.Cos(latRad)
                                - 30 * Math.Pow(Math.Cos(latRad), 3)
                                + 300 * Math.Pow(Math.Sin(latRad), 2) * Math.Pow(Math.Cos(latRad), 6));

                    var totalDevX = devX1 + devX2 + ridgeDevX;

                    return ((Earth.SnapshotDivFactor * totalDevX) % 360);
                },

                 // Y-Direction deviations (latitudinal)
                 (r, c) =>
                 {
                     float lat = EarthModel.MaxLat - r;
                     float lon = c - 180;
                     float latRad = lat * (float)Math.PI / 180;
                     float lonRad = lon * (float)Math.PI / 180;

                     // Perturbation: sinusoidal vorticity perturbations
                     const float A = (float)12e-5; // vorticity perturbation amplitude
                     const float m = 4f; // zonal wavenumber
                     const float lat_0 = 45f * (float)Math.PI / 180; // center lat = 45 N
                     const float lat_W = 15f * (float)Math.PI / 180; // aplitude = 15 deg

                     float devY2 = (float)(0.5f * A * Math.Cos(latRad) * Math.Pow(Math.Exp((lat_0 - latRad) / lat_W), 2) * Math.Cos(m * lonRad));

                     float ridgeDevY = ridgePatternDevs[Direction.Y][r, c];
                     float devY1 = JetModelFunctions.DevY_InvariableJet(Earth.HoursElapsed, Earth.SnapshotDivFactor,
                        ridgeDevY, lat, lon);

                     var totalDevY = devY1 + devY2;

                     return ((Earth.SnapshotDivFactor * totalDevY) % 180);
                 }
            );
        }
    }
}


