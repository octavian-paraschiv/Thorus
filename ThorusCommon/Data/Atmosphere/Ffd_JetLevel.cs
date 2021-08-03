using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;
using FastFluidSolver;

namespace ThorusCommon.Data
{
    public abstract class Ffd_JetLevel : JetLevel
    {
        protected EthierSteinmanDomain _ffdDomain = null;
        protected FluidSolver.solver_struct _ffdSolverParams;
        protected FluidSolver _ffd = null;

        protected double[,,] _u0 = null;
        protected double[,,] _v0 = null;
        protected double[,,] _w0 = null;
                                   
        protected double[,,] _fx = null;
        protected double[,,] _fy = null;
        protected double[,,] _fz = null;

        public Ffd_JetLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, loadFromStateFiles, defaultValue)
        {
            int Nx = P.ColumnCount;
            int Ny = P.RowCount;
            
            int Nz = (int)Math.Floor(SimConstants.LevelHeights[LevelType.JetLevel] / (2 * SimConstants.LevelHeights[LevelType.MidLevel]));
            var zStep = SimConstants.LevelHeights[LevelType.JetLevel] / Nz;

            // Create domain
            _ffdDomain = new EthierSteinmanDomain(Nx + 2, Ny + 2, Nz + 2, 1, 1, 1);

            for (int c = 0; c < Nx; c++)
                for (int r = 0; r < Ny; r++)
                {
                    var wl = earth.SFC.WL[r, c];
                    var h = earth.SFC.Height[r, c];

                    if (wl == 0 && h >= zStep)
                    {
                        // This should add obstacles for all hills and mountains which are higher than zStep
                        var zmin = 0;
                        var zmax = (int)Math.Floor(h / zStep);

                        _ffdDomain.add_obstacle(c, c + 1, r, r + 1, zmin, zmax);
                    }
                }

            _ffdSolverParams = new FluidSolver.solver_struct
            {
                backtrace_order = 1,
                max_iter = 10,
                min_iter = 2,
                tol = 1,
#if DEBUG
                verbose = true
#else
                verbose = false
#endif
            };

            _u0 = new double[Nx + 1, Ny + 2, Nz + 2];
            _v0 = new double[Nx + 2, Ny + 1, Nz + 2];
            _w0 = new double[Nx + 2, Ny + 2, Nz + 1];
            
            _fx = new double[Nx + 1, Ny + 2, Nz + 2];
            _fy = new double[Nx + 2, Ny + 1, Nz + 2];
            _fz = new double[Nx + 2, Ny + 2, Nz + 1];
        }

        protected virtual FluidSolver RunFfdStep()
        {
            int Nx = P.ColumnCount;
            int Ny = P.RowCount;
            int Nz = 3;

            var grad = P.Gradient();
            var gu = grad[Direction.X];
            var gv = grad[Direction.Y];
            var elr = Earth.ATM.ELR;

            for (int c = 1; c < Nx + 1; c++)
                for (int r = 1; r < Ny + 1; r++)
                    _u0[c, r, 1] = gu[r - 1, c - 1];

            for (int c = 1; c < Nx + 1; c++)
                for (int r = 1; r < Ny + 1; r++)
                    _v0[c, r, 1] = gv[r - 1, c - 1];

            for (int c = 1; c < Nz + 1; c++)
                for (int r = 1; r < Nz + 1; r++)
                    _w0[c, r, 1] = elr[r - 1, c - 1];

            FluidSolver ffd = new FluidSolver(_ffdDomain, 1, 1, _u0, _v0, _w0, _ffdSolverParams);

            ffd.time_step(_fx, _fy, _fz);

            return ffd;
        }
    }
}
