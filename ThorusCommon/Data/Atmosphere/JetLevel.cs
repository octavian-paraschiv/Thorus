using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace ThorusCommon.Data
{
    public abstract class JetLevel : AtmosphericLevel
    {
        protected override float[] LevelPressureExtremes
            => ThorusCommon.LevelPressureExtremes.JetLevelExtremes;

        protected override float LevelPressure
            => ThorusCommon.LevelPressure.JetLevelPressure;

        public JetLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0) :
            base(earth, LevelType.JetLevel, loadFromStateFiles, defaultValue)
        {
        }

        public override void Advance()
        {
        }

        public override void RebuildState()
        {
            P = ((Earth.ATM.SeaLevel.P.Multiply(100 / ThorusCommon.LevelPressure.SeaLevelPressure) +
                  Earth.ATM.TopLevel.P.Multiply(100 / ThorusCommon.LevelPressure.TopLevelPressure) +
                  Earth.ATM.MidLevel.P.Multiply(100 / ThorusCommon.LevelPressure.MidLevelPressure)) as DenseMatrix).EQ();

            var BP = this.BP;
            var FP = this.FP;

            FileSupport.Save(BP, Earth.UTC.Title, "D_BP");
            FileSupport.Save(FP, Earth.UTC.Title, "D_FP");

            var ridgePatternDevs = P.ToWindComponents();

            float daysElapsed = (Earth.HoursElapsed / AbsoluteConstants.HoursPerDay);
            float dailyJetAdvance = SimulationParameters.Instance.JetStreamWaveSpeed;
            float deltaLonRad = daysElapsed * dailyJetAdvance * (float)Math.PI / 180;

            RebuildJetState(ridgePatternDevs, BP, FP, daysElapsed, dailyJetAdvance, deltaLonRad);
        }

        protected abstract void RebuildJetState(DenseMatrix[] ridgePatternDevs, DenseMatrix BP, DenseMatrix FP,
            float daysElapsed, float dailyJetAdvance, float deltaLonRad);


        public override void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(P, title, string.Format("P_{0:d2}_MAP", _levelType), category);

            var this_BP = this.BP;
            var this_FP = this.FP;

            FileSupport.SaveAsStats(this_BP, title, "D_BP_MAP", category);
            FileSupport.SaveAsStats(this_FP, title, "D_FP_MAP", category);
        }
    }
}
