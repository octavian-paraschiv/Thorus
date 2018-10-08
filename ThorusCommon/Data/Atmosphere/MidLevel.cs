﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Thermodynamics;
using MathNet.Numerics;

namespace ThorusCommon.Data
{
    public class MidLevel : AtmosphericLevel
    {
        protected override float[] PressureExtremes
        {
            get
            {
                return LevelPressureExtremes.MidLevelExtremes;
            }
        }

        public MidLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, LevelType.MidLevel, loadFromStateFiles, defaultValue)
        {
        }

        private static readonly float _fNonAdvect = 0.75f;
        private static readonly float _fProAdvect = 1 - _fNonAdvect;

        private static readonly float _fScaleWindX = 0.5f;
        private static readonly float _fScaleWindY = 0.5f;

        private static readonly float _fNonHypso = 0.9f;
        private static readonly float _fProHypso = 1 - _fNonHypso;

        public override void RebuildState()
        {
        }

        public override void Advance()
        {
            _actualDev = Earth.ATM.TopLevel.ActualDev;
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
            DenseMatrix warmup = Earth.ATM.Warmup;

            // Shift temperature field and apply seasonal warmup corrections.
            var shiftedT = T.ApplyDeviations(applyDevs, null);

            var projT = (shiftedT + warmup).EQ();
            var projH = H.ApplyDeviations(applyDevs, null).EQ();

            DenseMatrix[] wind = P.ToWindComponents();
            DenseMatrix[] advDev = new DenseMatrix[]
            {
                _fScaleWindX * Earth.SnapshotDivFactor * wind[Direction.X],
                _fScaleWindY * Earth.SnapshotDivFactor * wind[Direction.Y],
            };

            DenseMatrix projT_adv = T.ApplyDeviations(advDev, null).EQ();
            DenseMatrix projH_adv = H.ApplyDeviations(advDev, null).EQ();

            T = (_fNonAdvect * projT + _fProAdvect * projT_adv).EQ();

            H = (_fNonAdvect * projH + _fProAdvect * projH_adv).EQ()
                // Can't be lower than 0 or higher than 100
                .MAX(0).MIN(100);

            ApplyCyclogenesys(applyDevs, T0, P0);
        }
    }
}
