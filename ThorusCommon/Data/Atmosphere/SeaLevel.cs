using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.MatrixExtensions;

namespace ThorusCommon.Data
{
    public class SeaLevel : AtmosphericLevel
    {
        public SeaLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, LevelType.SeaLevel, loadFromStateFiles, defaultValue)
        {
        }

        public override void RebuildState()
        {
        }

        public override void Advance()
        {
            _actualDev = Earth.ATM.MidLevel.AdvectionDev;
            ApplyAccumulatedDeviations();

            var applyDevs = new DenseMatrix[]
            {
                _actualDev[Direction.X].EQ(),
                _actualDev[Direction.Y].EQ(),
            };

            // Geopotential field: calculate
            // Temperature field: shift + apply seasonal warmup + apply advection
            // Humidity field: shift + apply advection

            var P0 = P.Clone() as DenseMatrix;
            var T0 = T.Clone() as DenseMatrix;

            // Temperature correction due to seasonal warmup/cooldown
            DenseMatrix warmup = (1 / 0.33f) * Earth.ATM.Warmup;

            // Shift temperature field and apply seasonal warmup corrections.
            var shiftedT = T.ApplyDeviations(applyDevs, null);

            var projT = (shiftedT + warmup).EQ();
            var projH = H.ApplyDeviations(applyDevs, null).EQ();

            ApplyAdvection(projT, projH);

            CalculatePressureField(applyDevs, T0, P0);
        }
    }
}
