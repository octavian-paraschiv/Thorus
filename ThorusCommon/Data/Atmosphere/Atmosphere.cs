using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.Statistics;
using ThorusCommon.Data;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;
using MathNet.Numerics;

namespace ThorusCommon.Engine
{
    public enum AirMassType
    {
        // Cold type air masses
        Arctic = -3,
        ContinentalPolar = -2,

        // Intermediate type air masses
        ColdMaritimePolar = -1,
        WarmMaritimePolar = 0,

        // Warm type air masses
        MaritimeTropical = 1,
        ContinentalTropical = 2
    }

    public class Atmosphere : IEarthFeature
    {
        public EarthModel Earth { get; set; }

        public AtmosphericLevel SeaLevel;
        public AtmosphericLevel MidLevel;
        public AtmosphericLevel TopLevel;
        public AtmosphericLevel JetLevel;

        public DenseMatrix Fronts = MatrixFactory.Init();
        public DenseMatrix AirMass = MatrixFactory.Init();

        private DenseMatrix _oldTMid = null;
        private DenseMatrix _oldAirMass = null;

        public DenseMatrix DeltaZ = null;

        private DenseMatrix _refTemp = null;
        public DenseMatrix Warmup = MatrixFactory.Init();

        public DenseMatrix MR = MatrixFactory.Init();
        public DenseMatrix ELR = MatrixFactory.Init();

        public Atmosphere(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0)
        {
            this.Earth = earth;

            SeaLevel = new SeaLevel(earth, loadFromStateFiles, defaultValue);
            MidLevel = new MidLevel(earth, loadFromStateFiles, defaultValue);
            TopLevel = new TopLevel(earth, loadFromStateFiles, defaultValue);
            JetLevel = new JetLevel(earth, loadFromStateFiles, defaultValue);

            if (loadFromStateFiles == false)
            {
                if (defaultValue == 0)
                {
                    TopLevel.LoadInitialConditions();

                    MidLevel.LoadInitialConditions();
                    CalculateAirMassType();

                    SeaLevel.LoadInitialConditions();
                }
                else
                {
                    AirMass = MatrixFactory.Init(defaultValue);
                }
            }
            else
            {
                AirMass = FileSupport.Load(Earth.UTC.Title, "M_00_MAP");
            }
        }

        public void RebuildState()
        {
            if (_refTemp == null)
                _refTemp = References.GetRefTemp(Earth);

            JetLevel.RebuildState();
            TopLevel.RebuildState();
            MidLevel.RebuildState();
            SeaLevel.RebuildState();

            CalculateAirMassType();

            CalculateMixingRatio();
            CalculateEnvironmentalLapseRate();
            CalculateFronts();
        }

        private void CalculateMixingRatio()
        {
            MR.Assign((r, c) =>
            {
                var tMid = MidLevel.T[r, c];
                var tSea = SeaLevel.T[r, c];
                var t = 0.5f * (tMid + tSea);

                var hMid = MidLevel.H[r, c];
                var hSea = SeaLevel.H[r, c];
                var h = 0.5f * (hMid + hSea);

                var pMid = MidLevel.P[r, c];
                var pSea = SeaLevel.P[r, c];
                var p = 0.5f * (pMid + pSea);

                var mr = LapseRate.MixingRatio(p, t, h);
                return mr;
            });
        }

        private void CalculateEnvironmentalLapseRate()
        {
            var midLevelHeight = SimConstants.LevelHeights[LevelType.MidLevel];

            var ADJ_LR = Earth.SFC.ADJ_LR;
            var ALBEDO = Earth.SFC.ALBEDO;
            var WL = Earth.SFC.WL;
            var ELV = Earth.SFC.Height;

            ELR.Assign((r, c) =>
            {
                var elv = ELV[r, c];

                if (elv > midLevelHeight)
                    // Assume dry air above the mid level boundary
                    return SimulationParameters.Instance.DryLapseRate;

                var wl = WL[r, c];
                var albedo = ALBEDO[r, c];
                var adj_lr = ADJ_LR[r, c];

                AirMassType amt = (AirMassType)AirMass[r, c];
                var mr = MR[r, c];

                var tMid = MidLevel.T[r, c];
                var tSea = SeaLevel.T[r, c];
                var t = 0.5f * (tMid + tSea);

                float lr = LapseRate.EnvironmentalLapseRate(amt, t, mr);

                if (adj_lr != 0)
                    lr += adj_lr;

                if (elv > 900)
                    lr -= (int)(elv / 900);

                lr -= 0.05f * albedo;

                // We verify lapse rate remains in atmospheric physical limnits
                lr = Math.Max(-SimulationParameters.Instance.DryLapseRate, Math.Min(SimulationParameters.Instance.DryLapseRate, lr));

                return lr;
            });
        }

        private void CalculateFronts()
        {
            if (_oldTMid == null)
            {
                _oldTMid = MidLevel.T.Clone() as DenseMatrix;
                _oldAirMass = AirMass.Clone() as DenseMatrix;

                // Fronts remain filled with zeros
                return;
            }

            var DT = (MidLevel.T - _oldTMid).EQ();
            var DM = (AirMass - _oldAirMass).EQ();

            _oldTMid = MidLevel.T.Clone() as DenseMatrix;
            _oldAirMass = AirMass.Clone() as DenseMatrix;

            Fronts.Assign((r, c) =>
            {
                var dt = DT[r, c];
                var dm = DM[r, c];

                var adt = Math.Abs(DT[r, c]);
                if (adt >= 0.1f)
                    adt = 1f;
                else
                    adt = (float)Math.Ceiling(adt);

                var f = Math.Sign(dm) * adt;

                return f;
            });
        }

        public void Advance()
        {
            var currentRefTemp = References.GetRefTemp(Earth);
            Warmup = Earth.SnapshotDivFactor * (currentRefTemp - _refTemp);
            _refTemp = currentRefTemp.Clone() as DenseMatrix;

            TopLevel.Advance();
            MidLevel.Advance();
            SeaLevel.Advance();
            JetLevel.Advance();
        }

        public void CalculateAirMassType()
        {
            AirMass.Assign((r, c) =>
            {
                var t01 = MidLevel.T[r, c];
                AirMassType amt = AirMassType.WarmMaritimePolar;

                // Arctic air mass: <= -11.5C
                if (t01 <= SimulationParameters.Instance.ArcticAirMassTemp)
                    amt = AirMassType.Arctic;

                // Continental Polar air mass: -11.5C .. -5C 
                else if (SimulationParameters.Instance.ArcticAirMassTemp < t01 && t01 <= SimulationParameters.Instance.ContinentalPolarAirMassTemp)
                    amt = AirMassType.ContinentalPolar;

                // Cold Maritime Polar air mass: -5C .. 5C
                else if (SimulationParameters.Instance.ContinentalPolarAirMassTemp < t01 && t01 <= SimulationParameters.Instance.MaritimePolarAirMassTemp)
                    amt = AirMassType.ColdMaritimePolar;

                // Warm Maritime Polar air mass: 5C .. 11.5C
                else if (SimulationParameters.Instance.MaritimePolarAirMassTemp < t01 && t01 <= SimulationParameters.Instance.MaritimeTropicalAirMassTemp)
                    amt = AirMassType.WarmMaritimePolar;

                // Maritime Tropical air mass: 11.5 .. 16.5C
                else if (SimulationParameters.Instance.MaritimeTropicalAirMassTemp < t01 && t01 <= SimulationParameters.Instance.TropicalContinentalAirMassTemp)
                    amt = AirMassType.MaritimeTropical;

                // Continental Tropical air mass: >= 16.5C
                else if (t01 > SimulationParameters.Instance.TropicalContinentalAirMassTemp)
                    amt = AirMassType.ContinentalTropical;

                return (float)amt;
            });
        }

        public void Add(Atmosphere atm)
        {
            SeaLevel.Add(atm.SeaLevel);
            MidLevel.Add(atm.MidLevel);
            TopLevel.Add(atm.TopLevel);
            JetLevel.Add(atm.JetLevel);

            AirMass.ADD(atm.AirMass);
            Fronts.ADD(atm.Fronts);
        }

        public void GetMin(Atmosphere atm)
        {
            SeaLevel.GetMin(atm.SeaLevel);
            MidLevel.GetMin(atm.MidLevel);
            TopLevel.GetMin(atm.TopLevel);
            JetLevel.GetMin(atm.JetLevel);
            
            AirMass.MIN(atm.AirMass);
            Fronts.MIN(atm.Fronts);
        }

        public void GetMax(Atmosphere atm)
        {
            SeaLevel.GetMax(atm.SeaLevel);
            MidLevel.GetMax(atm.MidLevel);
            TopLevel.GetMax(atm.TopLevel);
            JetLevel.GetMax(atm.JetLevel);
            
            AirMass.MAX(atm.AirMass);
            Fronts.MAX(atm.Fronts);
        }

        public void SaveStats(string title, string category)
        {
            SeaLevel.SaveStats(title, category);
            MidLevel.SaveStats(title, category);
            TopLevel.SaveStats(title, category);
            JetLevel.SaveStats(title, category);
            
            FileSupport.SaveAsStats(AirMass.EQ(), title, "M_00_MAP", category);
            FileSupport.SaveAsStats(Fronts.EQ(), title, "F_00_MAP", category);
        }

        public void Save(string title)
        {
            SeaLevel.Save(title);
            MidLevel.Save(title);
            TopLevel.Save(title);
            JetLevel.Save(title);

            FileSupport.Save(AirMass, title, "M_00_MAP");
            FileSupport.Save(Fronts, title, "F_00_MAP");
        }
    }
}
