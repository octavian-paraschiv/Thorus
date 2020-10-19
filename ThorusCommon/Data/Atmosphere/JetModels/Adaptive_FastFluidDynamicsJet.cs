using System;
using ThorusCommon.Thermodynamics;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.IO;
using FastFluidSolver;

namespace ThorusCommon.Data
{
    public class Adaptive_FastFluidDynamicsJet : JetLevel
    {
        bool _initial = true;

        public Adaptive_FastFluidDynamicsJet(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, loadFromStateFiles, defaultValue)
        {
        }

        protected override void RebuildJetState(DenseMatrix[] ridgePatternDevs, DenseMatrix BP, DenseMatrix FP,
            float daysElapsed, float dailyJetAdvance, float deltaLonRad)
        {
            int Nx = P.ColumnCount;
            int Ny = P.RowCount;
            int Nz = 1;

            // Create domain
            Domain omega = new EthierSteinmanDomain(Nx + 2, Ny + 2, Nz + 2, 1, 1, 1);

            FluidSolver.solver_struct solverParams = new FluidSolver.solver_struct
                {
                backtrace_order = 1,
                max_iter = 10,
                min_iter = 2,
                tol = 1,
                verbose = false
            };

            var u0 = new double[Nx + 1, Ny + 2, Nz + 2];
            var v0 = new double[Nx + 2, Ny + 1, Nz + 2];
            var w0 = new double[Nx + 2, Ny + 2, Nz + 1];

            var fx = new double[Nx + 1, Ny + 2, Nz + 2];
            var fy = new double[Nx + 2, Ny + 1, Nz + 2];
            var fz = new double[Nx + 2, Ny + 2, Nz + 1];

            var grad = P.Gradient();
            var gu = grad[Direction.X];
            var gv = grad[Direction.Y];

            for (int c = 1; c < Nx + 1; c++)
            {
                for (int r = 1; r < Ny + 1; r++)
                {
                    u0[c, r, 1] = gu[r - 1, c - 1];
                }
            }

            for (int c = 1; c < Nx + 1; c++)
            {
                for (int r = 1; r < Ny + 1; r++)
                {
                    v0[c, r, 1] = gv[r - 1, c - 1];
                }
            }

            FluidSolver ffd = new FluidSolver(omega, 1, 1, u0, v0, w0, solverParams);

            try
            {
                ffd.time_step(fx, fy, fz);

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


