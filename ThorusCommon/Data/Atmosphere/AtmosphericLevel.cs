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
        protected DenseMatrix[] _advDev = MatrixFactory.Init2D();

        public DenseMatrix[] ActualDev
        {
            get
            {
                return _actualDev;
            }
        }

        protected abstract float[] PressureExtremes { get; }

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
            FileSupport.Save(P.EQ(), title, string.Format("P_{0:d2}_MAP", _levelType));
            FileSupport.Save(T.EQ(), title, string.Format("T_{0:d2}_MAP", _levelType));
            FileSupport.Save(H.EQ(), title, string.Format("H_{0:d2}_MAP", _levelType));
        }
        #endregion

        public abstract void Advance();
        public abstract void RebuildState();


        protected void ApplyAccumulatedDeviations()
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

        protected void ApplyCyclogenesys(DenseMatrix[] applyDevs, DenseMatrix T0, DenseMatrix P0)
        {
            DenseMatrix rawP = P0.Clone() as DenseMatrix;

            DenseMatrix deltaT = null;
            DenseMatrix deltaP = null;

            float thickness = 0;
            float levelPressure = 0;

            switch (_levelType)
            {
                case LevelType.TopLevel:
                    deltaT = (Earth.ATM.TopLevel.T - MatrixFactory.Init(-55));
                    deltaP = (Earth.ATM.TopLevel.P - MatrixFactory.Init(300));
                    thickness = -4.5f;
                    levelPressure = LevelPressure.TopLevelPressure;
                    break;
                case LevelType.MidLevel:
                    deltaT = (Earth.ATM.MidLevel.T - Earth.ATM.TopLevel.T);
                    deltaP = (Earth.ATM.MidLevel.P - Earth.ATM.TopLevel.P);
                    thickness = -4f;
                    levelPressure = LevelPressure.MidLevelPressure;
                    break;
                case LevelType.SeaLevel:
                    deltaT = (Earth.ATM.SeaLevel.T - Earth.ATM.MidLevel.T);
                    deltaP = (Earth.ATM.SeaLevel.P - Earth.ATM.MidLevel.P);
                    thickness = -1.5f;
                    levelPressure = LevelPressure.SeaLevelPressure;
                    break;
            }

            rawP.Assign((r, c) =>
            {
                var front = Earth.ATM.Fronts[r, c];
                var pJetLevel = Earth.ATM.JetLevel.P[r, c];

                var t = T[r, c] + AbsoluteConstants.WaterFreezePoint;
                var t0 = T0[r, c] + AbsoluteConstants.WaterFreezePoint;
                var p0 = P0[r, c];

                var lapseRate = Math.Abs(deltaT[r, c] / thickness);

                var unitDp = levelPressure * Math.Abs((t - t0) / AbsoluteConstants.WaterFreezePoint);

                var cf = -SimulationParameters.Instance.CyclogeneticFactor;
                var acf = SimulationParameters.Instance.AntiCyclogeneticFactor;

                const float strong = 1.1f;
                const float weak = 0.3f;
                const float pseudoStationary = 0.1f;

                bool stable = ((lapseRate > SimulationParameters.Instance.HumidLapseRate) || (pJetLevel < 295));
                bool unstable = ((lapseRate < SimulationParameters.Instance.HumidLapseRate) || (pJetLevel > 305));

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

                if (p < PressureExtremes[0])
                    p = PressureExtremes[0];
                if (p > PressureExtremes[1])
                    p = PressureExtremes[1];

                return p;
            });

            var pNorth = rawP.RegionSubMatrix(-180, 179, 0, 89);
            var pSouth = rawP.RegionSubMatrix(-180, 179, -89, -1);

            var projPNorth = pNorth.Divide(pNorth.Mean()).Multiply(levelPressure) as DenseMatrix;
            var projPSouth = pSouth.Divide(pSouth.Mean()).Multiply(levelPressure) as DenseMatrix;

            var projP = MatrixFactory.Init();

            projP.SetSubMatrix(0, pNorth.RowCount, 0, pNorth.ColumnCount, projPNorth);
            projP.SetSubMatrix(pNorth.RowCount, pSouth.RowCount, 0, pSouth.ColumnCount, projPSouth);

            P = projP.EQ(4).ApplyDeviations(applyDevs, null).EQ(4);
        }

        //protected void ApplyCyclogenesys(DenseMatrix[] applyDevs, DenseMatrix T0, DenseMatrix P0)
        //{
        //    DenseMatrix rawP = P0.Clone() as DenseMatrix;

        //    DenseMatrix deltaT = null;
        //    DenseMatrix deltaP = null;

        //    float thickness = 0;
        //    float levelPressure = 0;

        //    switch (_levelType)
        //    {
        //        case LevelType.TopLevel:
        //            deltaT = (Earth.ATM.TopLevel.T - MatrixFactory.Init(-55));
        //            deltaP = (Earth.ATM.TopLevel.P - MatrixFactory.Init(300));
        //            thickness = -4500;
        //            levelPressure = LevelPressure.TopLevelPressure;
        //            break;
        //        case LevelType.MidLevel:
        //            deltaT = (Earth.ATM.MidLevel.T - Earth.ATM.TopLevel.T);
        //            deltaP = (Earth.ATM.MidLevel.P - Earth.ATM.TopLevel.P);
        //            thickness = -4000;
        //            levelPressure = LevelPressure.MidLevelPressure;
        //            break;
        //        case LevelType.SeaLevel:
        //            deltaT = (Earth.ATM.SeaLevel.T - Earth.ATM.MidLevel.T);
        //            deltaP = (Earth.ATM.SeaLevel.P - Earth.ATM.MidLevel.P);
        //            thickness = -1500;
        //            levelPressure = LevelPressure.SeaLevelPressure;
        //            break;
        //    }

        //    rawP.Assign((r, c) =>
        //    {
        //        var pJetLevel = Earth.ATM.JetLevel.P[r, c];

        //        bool isRidge = (pJetLevel > 305);
        //        bool isTrough = (pJetLevel < 295);

        //        var t = T[r, c] + AbsoluteConstants.WaterFreezePoint;
        //        var t0 = T0[r, c] + AbsoluteConstants.WaterFreezePoint;
        //        var p0 = P0[r, c];

        //        var vGradP = deltaP[r, c] / thickness;
        //        var vGradT = deltaT[r, c] / thickness;

        //        var dryGradT = -SimulationParameters.Instance.DryLapseRate / 1000;

        //        var sgn = Math.Sign(vGradP * vGradT);

        //        var dtr = (t - t0) / t0;

        //        var cf = SimulationParameters.Instance.CyclogeneticFactor;
        //        var acf = SimulationParameters.Instance.AntiCyclogeneticFactor;
        //        float dp = 0;

        //        if (sgn > 0 || isRidge)
        //        {
        //            // Stable (barotropic) atmosphere

        //            if (dtr > 0)
        //                // Positive thermal leap in stable atmosphere => anticyclogenesys
        //                dp = acf * 2f * p0 * dtr;
        //            else
        //                // Negative thermal leap in stable atmosphere => weak cyclogenesys
        //                dp = cf * 0.5f * p0 * dtr;
        //        }

        //        if (sgn < 0 || isTrough)
        //        {
        //            // Unstable (baroclinic) atmosphere layer

        //            if (dtr > 0)
        //                // Positive thermal leap in unstable atmosphere => weak anticyclogenesys
        //                dp = acf * 0.5f * p0 * dtr;
        //            else
        //                // Negative thermal leap in unstable atmosphere => cyclogenesys
        //                dp = cf * 2f * p0 * dtr;
        //        }

        //        var p = p0 + Earth.SnapshotDivFactor * dp;

        //        if (p < PressureExtremes[0])
        //            p = PressureExtremes[0];
        //        if (p > PressureExtremes[1])
        //            p = PressureExtremes[1];

        //        return p;
        //    });

        //    var pNorth = rawP.RegionSubMatrix(-180, 179, 0, 89);
        //    var pSouth = rawP.RegionSubMatrix(-180, 179, -89, -1);

        //    var projPNorth = pNorth.Divide(pNorth.Mean()).Multiply(levelPressure) as DenseMatrix;
        //    var projPSouth = pSouth.Divide(pSouth.Mean()).Multiply(levelPressure) as DenseMatrix;

        //    var projP = MatrixFactory.Init();

        //    projP.SetSubMatrix(0, pNorth.RowCount, 0, pNorth.ColumnCount, projPNorth);
        //    projP.SetSubMatrix(pNorth.RowCount, pSouth.RowCount, 0, pSouth.ColumnCount, projPSouth);

        //    P = projP.EQ(4).ApplyDeviations(applyDevs, null).EQ(4);
        //}

        //protected void ApplyCyclogenesys(DenseMatrix[] applyDevs, DenseMatrix T0, DenseMatrix P0)
        //{
        //    DenseMatrix rawP = P0.Clone() as DenseMatrix;

        //    DenseMatrix deltaT = null;
        //    DenseMatrix deltaP = null;

        //    float thickness = 0;
        //    float levelPressure = 0;

        //    switch (_levelType)
        //    {
        //        case LevelType.TopLevel:
        //            deltaT = (Earth.ATM.TopLevel.T - MatrixFactory.Init(-55));
        //            deltaP = (Earth.ATM.TopLevel.P - MatrixFactory.Init(300));
        //            thickness = -4500;
        //            levelPressure = LevelPressure.TopLevelPressure;
        //            break;
        //        case LevelType.MidLevel:
        //            deltaT = (Earth.ATM.MidLevel.T - Earth.ATM.TopLevel.T);
        //            deltaP = (Earth.ATM.MidLevel.P - Earth.ATM.TopLevel.P);
        //            thickness = -4000;
        //            levelPressure = LevelPressure.MidLevelPressure;
        //            break;
        //        case LevelType.SeaLevel:
        //            deltaT = (Earth.ATM.SeaLevel.T - Earth.ATM.MidLevel.T);
        //            deltaP = (Earth.ATM.SeaLevel.P - Earth.ATM.MidLevel.P);
        //            thickness = -1500;
        //            levelPressure = LevelPressure.SeaLevelPressure;
        //            break;
        //    }

        //    // TODO: apply effective cyclogenesys

        //    var pNorth = rawP.RegionSubMatrix(-180, 179, 0, 89);
        //    var pSouth = rawP.RegionSubMatrix(-180, 179, -89, -1);

        //    var projPNorth = pNorth.Divide(pNorth.Mean()).Multiply(levelPressure) as DenseMatrix;
        //    var projPSouth = pSouth.Divide(pSouth.Mean()).Multiply(levelPressure) as DenseMatrix;

        //    var projP = MatrixFactory.Init();

        //    projP.SetSubMatrix(0, pNorth.RowCount, 0, pNorth.ColumnCount, projPNorth);
        //    projP.SetSubMatrix(pNorth.RowCount, pSouth.RowCount, 0, pSouth.ColumnCount, projPSouth);

        //    P = projP.EQ(4).ApplyDeviations(applyDevs, null).EQ(4);
        //}
    }
}

