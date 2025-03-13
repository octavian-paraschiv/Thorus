using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;

namespace ThorusCommon.Data
{
    public class SeaLevel : AtmosphericLevel
    {
        protected override float[] PressureExtremes
        {
            get
            {
                return LevelPressureExtremes.SeaLevelExtremes;
            }
        }

        public SeaLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, LevelType.SeaLevel, loadFromStateFiles, defaultValue)
        {
            _fNonAdvect = 0.5f;
            _fProAdvect = 1 - _fNonAdvect;

            _fScaleWindX = 0.75f;
            _fScaleWindY = 0.75f;
        }

        public override void RebuildState()
        {
        }

        public override void Advance()
        {
            _actualDev = Earth.ATM.MidLevel.ActualDev;
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
            DenseMatrix warmup = (1f + 1f / 3f) * Earth.ATM.Warmup;

            // Shift temperature field and apply seasonal warmup corrections.
            var shiftedT = T.ApplyDeviations(applyDevs);

            var projT = (shiftedT + warmup).EQ();
            var projH = H.ApplyDeviations(applyDevs);

            ApplyAdvection(projT, projH);

            ApplyCyclogenesys(applyDevs, T0, P0);
        }
    }
}
