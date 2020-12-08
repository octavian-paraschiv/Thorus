using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;
using ThorusCommon.Data;
using System.IO;
using static ThorusCommon.SimulationParameters;

namespace ThorusCommon.Engine
{
    public abstract class AtmosphericLevel : IEarthFeature
    {
        public EarthModel Earth { get; set; }

        public DenseMatrix P = MatrixFactory.Init();
        public DenseMatrix H = MatrixFactory.Init();
        public DenseMatrix T = MatrixFactory.Init();
                
        protected int _levelType = -1;

        protected DenseMatrix[] _accumulatedFieldDevs = MatrixFactory.Init2D();
        protected DenseMatrix[] _actualDev = MatrixFactory.Init2D();

        public virtual DenseMatrix[] AdvectionDev
        {
            get
            {
                //var advDev = P.ToWindComponents();
                //return MatrixFactory.New2D(
                //    (r, c) => 10f * advDev[Direction.X][r, c],
                //    (r, c) => 10f * advDev[Direction.Y][r, c]
                //).EQ2D();
                return _actualDev;
            }
        }

        protected float[] LevelPressureExtremes
        {
            get
            {
                switch(_levelType)
                {
                    case LevelType.SeaLevel:
                        return ThorusCommon.LevelPressureExtremes.SeaLevelExtremes;

                    case LevelType.MidLevel:
                        return ThorusCommon.LevelPressureExtremes.MidLevelExtremes;

                    case LevelType.TopLevel:
                        return ThorusCommon.LevelPressureExtremes.TopLevelExtremes;

                    case LevelType.JetLevel:
                    default:
                        return ThorusCommon.LevelPressureExtremes.JetLevelExtremes;
                }
            }
        }

        protected float LevelPressure
        {
            get
            {
                switch (_levelType)
                {
                    case LevelType.SeaLevel:
                        return ThorusCommon.LevelPressure.SeaLevelPressure;

                    case LevelType.MidLevel:
                        return ThorusCommon.LevelPressure.MidLevelPressure;

                    case LevelType.TopLevel:
                        return ThorusCommon.LevelPressure.TopLevelPressure;

                    case LevelType.JetLevel:
                    default:
                        return ThorusCommon.LevelPressure.JetLevelPressure;
                }
            }
        }

        public AtmosphericLevel(EarthModel earth, int levelType, bool loadFromStateFiles, float defaultValue = 0)
        {
            this.Earth = earth;
            _levelType = levelType;

            if (loadFromStateFiles)
            {
                P = FileSupport.Load(Earth.UTC.Title, string.Format("P_{0:d2}_MAP", _levelType)); 
                T = FileSupport.Load(Earth.UTC.Title, string.Format("T_{0:d2}_MAP", _levelType));
                H = FileSupport.Load(Earth.UTC.Title, string.Format("H_{0:d2}_MAP", _levelType));
            }
            else if (defaultValue != 0)
            {
                P = MatrixFactory.Init(defaultValue);
                T = MatrixFactory.Init(defaultValue);
                H = MatrixFactory.Init(defaultValue);
            }
        }

        public void LoadInitialConditions()
        {
            string pressureFile = string.Format("P{0:d2}.thd", _levelType);
            string temperatureFile = string.Format("T{0:d2}.thd", _levelType);
            string humidityFile = string.Format("H{0:d2}.thd", _levelType);

            string filePath = Path.Combine(SimulationData.WorkFolder, temperatureFile);
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            DenseMatrix t = FileSupport.LoadMatrixFromFile(filePath);

            filePath = Path.Combine(SimulationData.WorkFolder, pressureFile);
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            DenseMatrix p = FileSupport.LoadMatrixFromFile(filePath);

            filePath = Path.Combine(SimulationData.WorkFolder, humidityFile);
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            DenseMatrix h = FileSupport.LoadMatrixFromFile(filePath);

            H = h.EQ();
            P = p.EQ();
            T = t.EQ();
        }

        #region Statistic-related
        public virtual void Add(AtmosphericLevel atmLevel)
        {
            P.ADD(atmLevel.P); 
            T.ADD(atmLevel.T);
            H.ADD(atmLevel.H);
        }

        public virtual void GetMin(AtmosphericLevel atmLevel)
        {
            P.MIN(atmLevel.P);
            T.MIN(atmLevel.T);
            H.MIN(atmLevel.H);
        }

        public virtual void GetMax(AtmosphericLevel atmLevel)
        {
            P.MAX(atmLevel.P);
            T.MAX(atmLevel.T);
            H.MAX(atmLevel.H);
        }

        public virtual void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(P, title, string.Format("P_{0:d2}_MAP", _levelType), category);
            FileSupport.SaveAsStats(T, title, string.Format("T_{0:d2}_MAP", _levelType), category);
            FileSupport.SaveAsStats(H, title, string.Format("H_{0:d2}_MAP", _levelType), category);
        }

        public virtual void Save(string title)
        {
            FileSupport.Save(P, title, string.Format("P_{0:d2}_MAP", _levelType));
            FileSupport.Save(T, title, string.Format("T_{0:d2}_MAP", _levelType));
            FileSupport.Save(H, title, string.Format("H_{0:d2}_MAP", _levelType));
        }
        #endregion

        public abstract void Advance();
        public abstract void RebuildState();

        protected virtual void ApplyAccumulatedDeviations()
        {
            for (int r = 0; r < SurfaceLevel.GridRowCount; r++)
                for (int c = 0; c < SurfaceLevel.GridColumnCount; c++)
                {
                    _accumulatedFieldDevs[Direction.C][r, c] += _actualDev[Direction.C][r, c];
                    _accumulatedFieldDevs[Direction.R][r, c] += _actualDev[Direction.R][r, c];

                    // We round the current accumulated deviations to the nearest integers.
                    // By means of this we test whether the current accumulated deviations have significant values.
                    int actualDevC = (int)_accumulatedFieldDevs[Direction.C][r, c].Round();
                    int actualDevR = (int)_accumulatedFieldDevs[Direction.R][r, c].Round();

                    _actualDev[Direction.C][r, c] = actualDevC;
                    _actualDev[Direction.R][r, c] = actualDevR;

                    _accumulatedFieldDevs[Direction.C][r, c] -= actualDevC;
                    _accumulatedFieldDevs[Direction.R][r, c] -= actualDevR;
                }
        }

        protected virtual void ApplyAdvection(DenseMatrix projT, DenseMatrix projH)
        {
            DenseMatrix[] wind = P.ToWindComponents();

            DenseMatrix projT_adv = projT.Clone() as DenseMatrix;
            DenseMatrix projH_adv = projH.Clone() as DenseMatrix;

            float mul = 10 * (1 / (Earth.SnapshotDivFactor * AbsoluteConstants.HoursPerDay));
            int count = (int)(SimulationParameters.Instance.StepsPerDay * Earth.SnapshotDivFactor);
            
            float fNonAdvect = 0.5f;
            float fProAdvect = 1 - fNonAdvect;

            DenseMatrix[] advDev = new DenseMatrix[]
            {
                mul * wind[Direction.X],
                mul * wind[Direction.Y],
            };

            for (int i = 0; i < count; i++)
            {
                projT_adv = projT_adv.ApplyDeviations(advDev, null);
                projH_adv = projH_adv.ApplyDeviations(advDev, null);
            }

            T = (fNonAdvect * projT + fProAdvect * projT_adv).EQ(4);

            H = (fNonAdvect * projH + fProAdvect * projH_adv)
                // Can't be lower than 0 or higher than 100
                .MAX(0).MIN(100)
                .EQ(4);
        }

        protected virtual void CalculatePressureField(DenseMatrix[] applyDevs, DenseMatrix T0, DenseMatrix P0)
        {
            DenseMatrix rawP = P0.Clone() as DenseMatrix;
            DenseMatrix deltaT = null;
            DenseMatrix deltaP = null;

            float thickness = 0;
            
            switch (_levelType)
            {
                case LevelType.TopLevel:
                    deltaT = (Earth.ATM.TopLevel.T - MatrixFactory.Init(-55));
                    deltaP = (Earth.ATM.TopLevel.P - MatrixFactory.Init(300));
                    thickness = -4.5f;
                    break;
                case LevelType.MidLevel:
                    deltaT = (Earth.ATM.MidLevel.T - Earth.ATM.TopLevel.T);
                    deltaP = (Earth.ATM.MidLevel.P - Earth.ATM.TopLevel.P);
                    thickness = -4f;
                    break;
                case LevelType.SeaLevel:
                    deltaT = (Earth.ATM.SeaLevel.T - Earth.ATM.MidLevel.T);
                    deltaP = (Earth.ATM.SeaLevel.P - Earth.ATM.MidLevel.P);
                    thickness = -1.5f;
                    break;
            }

            var weakBlock = Earth.ATM.JetLevel.WeakBlockTH;
            var strongBlock = Earth.ATM.JetLevel.StrongBlockTH;

            var DIV = (10 * Earth.ATM.JetLevel.P.Divergence()).EQ(4);

            rawP.Assign((r, c) =>
            {
                var front = Earth.ATM.Fronts[r, c];
                var pJetLevel = Earth.ATM.JetLevel.P[r, c];

                var t = T[r, c] + AbsoluteConstants.WaterFreezePoint;
                var t0 = T0[r, c] + AbsoluteConstants.WaterFreezePoint;
                var p0 = P0[r, c];

                var lapseRate = Math.Abs(deltaT[r, c] / thickness);

                var unitDp = this.LevelPressure * Math.Abs((t - t0) / AbsoluteConstants.WaterFreezePoint);

                var cf = -SimulationParameters.Instance.CyclogeneticFactor;
                var acf = SimulationParameters.Instance.AntiCyclogeneticFactor;

                var div = DIV[r, c];

                const float strong = 2.5f;
                const float weak = 0.5f;
                const float pseudoStationary = 0.1f;

                bool stable = ((div <= -1));
                bool unstable = ((div >= 1));

                float actualDp = 0;


                if (front < 0)
                {
                    if (stable)
                        actualDp = cf * pseudoStationary * unitDp;
                    else if (unstable)
                        actualDp = cf * strong * unitDp;
                    else
                        actualDp = cf * weak * unitDp;
                }
                else if (front > 0)
                {
                    actualDp = acf * pseudoStationary * unitDp;
                }
                else
                {
                    if (stable)
                        actualDp = acf * strong * unitDp;
                    else if (unstable)
                        actualDp = acf * pseudoStationary * unitDp;
                    else
                        actualDp = acf * weak * unitDp;
                }


                var p = p0 + Earth.SnapshotDivFactor * actualDp;

                if (p < LevelPressureExtremes[0])
                    p = LevelPressureExtremes[0];
                if (p > LevelPressureExtremes[1])
                    p = LevelPressureExtremes[1];

                return p;
            });

            //DenseMatrix projP = rawP;

            DenseMatrix projP = MatrixFactory.Init();

            var pNorth = rawP.RegionSubMatrix(-180, 179, 0, 89);
            var pSouth = rawP.RegionSubMatrix(-180, 179, -89, -1);

            var projPNorth = pNorth.Divide(pNorth.Mean()).Multiply(this.LevelPressure) as DenseMatrix;
            var projPSouth = pSouth.Divide(pSouth.Mean()).Multiply(this.LevelPressure) as DenseMatrix;
            
            projP.SetSubMatrix(0, pNorth.RowCount, 0, pNorth.ColumnCount, projPNorth);
            projP.SetSubMatrix(pNorth.RowCount, pSouth.RowCount, 0, pSouth.ColumnCount, projPSouth);

            P = projP.EQ(4).ApplyDeviations(applyDevs, null).EQ(4);
        }

        float _weakBlock = -1;
        public float WeakBlockTH
        {
            get
            {
                if (_weakBlock < 0)
                {
                    float[] range = this.LevelPressureExtremes;
                    _weakBlock = 0.5f * (range[0] + range[1]);
                }

                return _weakBlock;
            }
        }

        float _strongBlock = -1;
        public float StrongBlockTH
        {
            get
            {
                if (_strongBlock < 0)
                    _strongBlock = 1.025f * WeakBlockTH;

                return _strongBlock;
            }
        }

        public DenseMatrix BP
        {
            get
            {
                return MatrixFactory.New((r, c) =>
                {
                    var p = P[r, c];
                    var bp = (p - WeakBlockTH) / (StrongBlockTH - WeakBlockTH);
                    return Math.Max(0f, bp);
                });
            }
        }

        public DenseMatrix FP
        {
            get
            {
                return MatrixFactory.New((r, c) =>
                {
                    var p = P[r, c];
                    var fp = 1f - (p - WeakBlockTH) / (StrongBlockTH - WeakBlockTH);
                    return Math.Min(1f, Math.Max(0f, fp));
                });
            }
        }
    }
}

