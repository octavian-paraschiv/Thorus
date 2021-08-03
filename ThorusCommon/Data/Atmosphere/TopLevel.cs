using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.IO;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Thermodynamics;
using MathNet.Numerics;

namespace ThorusCommon.Data
{
    public class TopLevel : AtmosphericLevel
    {
        public TopLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, LevelType.TopLevel, loadFromStateFiles, defaultValue)
        {
        }

        public override void RebuildState()
        {
        }

        public override void Advance()
        {
            _actualDev = Earth.ATM.JetLevel.AdvectionDev;
            ApplyAccumulatedDeviations();

            var applyDevs = new DenseMatrix[]
            {
                _actualDev[Direction.X].EQ(),
                _actualDev[Direction.Y].EQ(),
            };

            var tst = _actualDev[Direction.X][45, 82];

            // Pressure field: calculate
            // Temperature field: shift + apply seasonal warmup + apply advection
            // Humidity field: shift + apply advection

            var P0 = P.Clone() as DenseMatrix;
            var T0 = T.Clone() as DenseMatrix;

            // Temperature correction due to seasonal warmup/cooldown
            DenseMatrix warmup = 0.33f * Earth.ATM.Warmup;

            // Shift temperature field and apply seasonal warmup corrections.
            var shiftedT = T.ApplyDeviations(applyDevs, null);
            
            var projT = (shiftedT + warmup).EQ();
            var projH = H.ApplyDeviations(applyDevs, null).EQ();

            ApplyAdvection(projT, projH);

            CalculatePressureField(applyDevs, T0, P0);
        }
        
    }
}
