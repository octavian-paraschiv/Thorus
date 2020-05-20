using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;

namespace ThorusCommon.Data
{
    public class SurfaceLevel : IEarthFeature
    {
        public EarthModel Earth { get; set; }

        static int _gridRowCount = EarthModel.MaxLat - EarthModel.MinLat + 1;
        static int _gridColCount = EarthModel.MaxLon - EarthModel.MinLon + 1;

        public static int GridRowCount
        { get { return _gridRowCount; } }

        public static int GridColumnCount
        { get { return _gridColCount; } }

        public DenseMatrix Height = MatrixFactory.Init();
        public DenseMatrix TE = MatrixFactory.Init();
        public DenseMatrix WL = MatrixFactory.Init();
        public DenseMatrix ADJ_LR = MatrixFactory.Init();
        public DenseMatrix TW = MatrixFactory.Init();
        public DenseMatrix TL = MatrixFactory.Init();
        public DenseMatrix TS = MatrixFactory.Init();
        public DenseMatrix SNOW = MatrixFactory.Init();
        public DenseMatrix BLIZZARD = MatrixFactory.Init();
        public DenseMatrix DEF_ALBEDO = MatrixFactory.Init();
        public DenseMatrix ALBEDO = MatrixFactory.Init();

        public DenseMatrix Precip = MatrixFactory.Init();
        public DenseMatrix LIDX = MatrixFactory.Init();
        public DenseMatrix TLow = MatrixFactory.Init();
        public DenseMatrix THigh = MatrixFactory.Init();

        public DenseMatrix TNormLow = MatrixFactory.Init();
        public DenseMatrix TNormHigh = MatrixFactory.Init();

        public DenseMatrix RAIN = MatrixFactory.Init();
        public DenseMatrix FOG = MatrixFactory.Init();

        public DenseMatrix DaysSinceLastSnowFall = MatrixFactory.Init();
        public DenseMatrix DaysSinceLastRainFall = MatrixFactory.Init();

        public SurfaceLevel(EarthModel earth, bool loadFromStateFiles, float defaultValue = 0)
        {
            this.Earth = earth;
            InitGeographicalParams();

            if (loadFromStateFiles == false)
            {
                if (defaultValue == 0)
                {
                    // ------------
                    // Soil temperature where applicable
                    string filePath = Path.Combine(SimulationData.WorkFolder, "SOIL.thd");
                    if (File.Exists(filePath))
                    {
                        var tl = FileSupport.LoadMatrixFromFile(filePath);
                        TL.Assign((r, c) =>
                        {
                            if (WL[r, c] == 0)
                                return tl[r, c];

                            return 0;

                        });
                    }

                    // ------------
                    // Sea temperature where applicable
                    filePath = Path.Combine(SimulationData.WorkFolder, "SST.thd");
                    if (File.Exists(filePath))
                    {
                        var tw = FileSupport.LoadMatrixFromFile(filePath);
                        TW.Assign((r, c) =>
                        {
                            if (WL[r, c] != 0)
                                return tw[r, c];

                            return 0;

                        });
                    }

                    // Combined surface temperature
                    TS.Assign((r, c) =>
                    {
                        if (WL[r, c] != 0)
                            return TW[r, c];

                        return TL[r, c];

                    });

                    // ------------
                    // Snow cover
                    filePath = Path.Combine(SimulationData.WorkFolder, "SNOW.thd");
                    if (File.Exists(filePath))
                        this.SNOW = FileSupport.LoadMatrixFromFile(filePath);
                }
                else
                {
                    TE = MatrixFactory.Init(defaultValue);
                    TW = MatrixFactory.Init(defaultValue);
                    TL = MatrixFactory.Init(defaultValue);
                    TS = MatrixFactory.Init(defaultValue);
                    SNOW = MatrixFactory.Init(defaultValue);
                    RAIN = MatrixFactory.Init(defaultValue);
                    BLIZZARD = MatrixFactory.Init(defaultValue);
                    ALBEDO = MatrixFactory.Init(defaultValue);

                    Precip = MatrixFactory.Init(defaultValue);
                    TLow = MatrixFactory.Init(defaultValue);
                    THigh = MatrixFactory.Init(defaultValue);
                    LIDX = MatrixFactory.Init(defaultValue);

                    FOG = MatrixFactory.Init(defaultValue);

                    TNormLow = MatrixFactory.Init(defaultValue);
                    TNormHigh = MatrixFactory.Init(defaultValue);
                }
            }
            else
            {
                TE = FileSupport.Load(Earth.UTC.Title, "T_TE_MAP");
                TW = FileSupport.Load(Earth.UTC.Title, "T_TW_MAP");
                TL = FileSupport.Load(Earth.UTC.Title, "T_TL_MAP");
                TS = FileSupport.Load(Earth.UTC.Title, "T_TS_MAP");

                SNOW = FileSupport.Load(Earth.UTC.Title, "N_00_MAP");
                RAIN = FileSupport.Load(Earth.UTC.Title, "R_00_MAP");

                BLIZZARD = FileSupport.Load(Earth.UTC.Title, "B_00_MAP");
                ALBEDO = FileSupport.Load(Earth.UTC.Title, "A_00_MAP");

                Precip = FileSupport.Load(Earth.UTC.Title, "C_00_MAP");
                TLow = FileSupport.Load(Earth.UTC.Title, "T_SL_MAP");
                THigh = FileSupport.Load(Earth.UTC.Title, "T_SH_MAP");
                LIDX = FileSupport.Load(Earth.UTC.Title, "L_00_MAP");

                FOG = FileSupport.Load(Earth.UTC.Title, "F_SI_MAP");

                TNormLow = FileSupport.Load(Earth.UTC.Title, "T_NL_MAP");
                TNormHigh = FileSupport.Load(Earth.UTC.Title, "T_NH_MAP");

            }
        }

        private void InitGeographicalParams()
        {
            // ------------
            string filePath = ".\\ElevationMap.thd";
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            Height = FileSupport.LoadMatrixFromFile(filePath);

            // ------------
            filePath = ".\\LandWaterMask.thd";
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            var wlMask = FileSupport.LoadMatrixFromFile(filePath);
            var he = Height.EQ(8);

            WL.Assign((r, c) =>
            {
                var sgn = 0f;

                sgn = Math.Sign(wlMask[r, c]);
                if (sgn == 1)
                  return 1;

                if (he[r, c] >= 50f)
                    return 0;

                return 1;
            });


            // ------------
            filePath = ".\\ADJ_LR.thd";
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            this.ADJ_LR = FileSupport.LoadMatrixFromFile(filePath);

            // ------------
            filePath = ".\\albedo.thd";
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            this.ALBEDO = FileSupport.LoadMatrixFromFile(filePath);
            this.DEF_ALBEDO = FileSupport.LoadMatrixFromFile(filePath);
        }

        #region Statistics and related

        public void Add(SurfaceLevel sfc)
        {
            TE.ADD(sfc.TE);
            TL.ADD(sfc.TL);
            TS.ADD(sfc.TS);
            TW.ADD(sfc.TW);

            SNOW.ADD(sfc.SNOW);
            RAIN.ADD(sfc.RAIN);

            BLIZZARD.ADD(sfc.BLIZZARD);
            ALBEDO.ADD(sfc.ALBEDO);

            Precip.ADD(sfc.Precip);
            TLow.ADD(sfc.TLow);
            THigh.ADD(sfc.THigh);
            LIDX.ADD(sfc.LIDX);

            FOG.ADD(sfc.FOG);

            TNormLow.ADD(sfc.TNormLow);
            TNormHigh.ADD(sfc.TNormHigh);
        }

        public void GetMin(SurfaceLevel sfc)
        {
            TE.MIN(sfc.TE);
            TL.MIN(sfc.TL);
            TS.MIN(sfc.TS);
            TW.MIN(sfc.TW);

            SNOW.MIN(sfc.SNOW);
            RAIN.MIN(sfc.RAIN);

            BLIZZARD.MIN(sfc.BLIZZARD);
            ALBEDO.MIN(sfc.ALBEDO);

            Precip.MIN(sfc.Precip);

            TLow.MIN(sfc.TLow);
            THigh.MIN(sfc.THigh);
            LIDX.MIN(sfc.LIDX);

            FOG.MIN(sfc.FOG);

            TNormLow.MIN(sfc.TNormLow);
            TNormHigh.MIN(sfc.TNormHigh);

        }

        public void GetMax(SurfaceLevel sfc)
        {
            TE.MAX(sfc.TE);
            TL.MAX(sfc.TL);
            TS.MAX(sfc.TS);
            TW.MAX(sfc.TW);

            SNOW.MAX(sfc.SNOW);
            RAIN.MAX(sfc.RAIN);

            BLIZZARD.MAX(sfc.BLIZZARD);
            ALBEDO.MAX(sfc.ALBEDO);

            Precip.MAX(sfc.Precip);

            TLow.MAX(sfc.TLow);
            THigh.MAX(sfc.THigh);
            LIDX.MAX(sfc.LIDX);

            FOG.MAX(sfc.FOG);

            TNormLow.MAX(sfc.TNormLow);
            TNormHigh.MAX(sfc.TNormHigh);
        }

        public void Save(string title)
        {
            FileSupport.Save(TE, title, "T_TE_MAP");
            FileSupport.Save(TW, title, "T_TW_MAP");
            FileSupport.Save(TL, title, "T_TL_MAP");
            FileSupport.Save(TS, title, "T_TS_MAP");

            FileSupport.Save(SNOW, title, "N_00_MAP");
            FileSupport.Save(RAIN, title, "R_00_MAP");

            FileSupport.Save(BLIZZARD, title, "B_00_MAP");
            FileSupport.Save(ALBEDO, title, "A_00_MAP");

            FileSupport.Save(Precip, title, "C_00_MAP");
            FileSupport.Save(TLow, title, "T_SL_MAP");
            FileSupport.Save(THigh, title, "T_SH_MAP");
            FileSupport.Save(LIDX, title, "L_00_MAP");

            FileSupport.Save(FOG, title, "F_SI_MAP");

            FileSupport.Save(TNormLow, title, "T_NL_MAP");
            FileSupport.Save(TNormHigh, title, "T_NH_MAP");

            FileSupport.Save(TLow - TNormLow, title, "T_DL_MAP");
            FileSupport.Save(THigh - TNormHigh, title, "T_DH_MAP");
            FileSupport.Save(0.5f * (TLow - TNormLow + THigh - TNormHigh), title, "T_DA_MAP");
        }

        public void SaveStats(string title, string category)
        {
            FileSupport.SaveAsStats(TE, title, "T_TE_MAP", category);
            FileSupport.SaveAsStats(TW, title, "T_TW_MAP", category);
            FileSupport.SaveAsStats(TL, title, "T_TL_MAP", category);
            FileSupport.SaveAsStats(TS, title, "T_TS_MAP", category);

            FileSupport.SaveAsStats(SNOW, title, "N_00_MAP", category);
            FileSupport.SaveAsStats(RAIN, title, "R_00_MAP", category);

            FileSupport.SaveAsStats(BLIZZARD, title, "B_00_MAP", category);
            FileSupport.SaveAsStats(ALBEDO, title, "A_00_MAP", category);

            FileSupport.SaveAsStats(Precip, title, "C_00_MAP", category);
            FileSupport.SaveAsStats(TLow, title, "T_SL_MAP", category);
            FileSupport.SaveAsStats(THigh, title, "T_SH_MAP", category);
            FileSupport.SaveAsStats(LIDX, title, "L_00_MAP", category);

            FileSupport.SaveAsStats(FOG, title, "F_SI_MAP", category);

            FileSupport.SaveAsStats(TNormLow, title, "T_NL_MAP", category);
            FileSupport.SaveAsStats(TNormHigh, title, "T_NH_MAP", category);

            FileSupport.SaveAsStats((TLow - TNormLow), title, "T_DL_MAP", category);
            FileSupport.SaveAsStats((THigh - TNormHigh), title, "T_DH_MAP", category);

            FileSupport.SaveAsStats(0.5f * (TLow - TNormLow + THigh - TNormHigh), title, "T_DA_MAP", category);
        }

        #endregion


        public void RebuildState()
        {
            CalculateFogIndex();
            CalculateTotalPrecipitation();
            CalculateMeanAirTempAtSurface();
            CalculateDailyTempExtremes();
        }

        bool _initialLIDXCalculation = true;
        
        public void CalculateInstabilityIndex(DenseMatrix eqFronts)
        {
            if (_initialLIDXCalculation)
            {
                _initialLIDXCalculation = false;
                return;
            }

            LIDX.Assign((r, c) =>
            {
                var te = THigh[r, c];

                var height = Height[r, c];

                var mr = Earth.ATM.MR[r, c];
                
                var vt = te + (1000f * mr / 6f);

                var f = eqFronts[r, c];

                var elr = Earth.ATM.ELR[r, c];

                var lifted_t = 0f;

                if (height > SimConstants.LevelHeights[LevelType.MidLevel])
                {
                    // Surface is above mid level boundary => assume dry lapse rate
                    var dh = height - SimConstants.LevelHeights[LevelType.TopLevel];
                    lifted_t = vt + SimulationParameters.Instance.DryLapseRate * dh / 1000f;
                }
                else
                {
                    // Surface is below mid level boundary
                    
                    // Between surface and mid level boundary => environmental lapse rate
                    var dhToMid = height - SimConstants.LevelHeights[LevelType.MidLevel];
                    var liftedToMid_t = vt + elr * dhToMid / 1000f;

                    // Above mid boundary  => dry lapse rate
                    var dhMidToTop = SimConstants.LevelHeights[LevelType.MidLevel] - SimConstants.LevelHeights[LevelType.TopLevel];
                    lifted_t = liftedToMid_t + SimulationParameters.Instance.DryLapseRate * dhMidToTop / 1000f;
                }
                                
                var actual_t = Earth.ATM.TopLevel.T[r, c];

                var dT = (actual_t - lifted_t);

                // and some corrections for fronts and elevation
                var h = height / 1000;

                if (f < 0)
                    dT += f;

                if (3f > h && h > 1f)
                    dT -= 2 * h;
                else if (h > 0.5f)
                    dT -= h;

                return dT;
            });
        }

        public void CalculateTotalPrecipitation()
        {
            var GP = Earth.ATM.MidLevel.P.Gradient2().Rescale(new float[] { 0, 100 });
            var GT = Earth.ATM.MidLevel.T.Gradient2().Rescale(new float[] { 0, 100 });
            var TS = Earth.SFC.TS;
            var TE = Earth.SFC.TE;

            DenseMatrix WIND = MatrixFactory.Init();
            if (Earth.ATM.SeaLevel.P != null)
                WIND = Earth.ATM.SeaLevel.P.Gradient2();

            var HMid = Earth.ATM.MidLevel.H;
            var HSea = Earth.ATM.SeaLevel.H;

            if (HSea == null)
                HSea = HMid;

            var eqFronts = Earth.ATM.Fronts;//.EQ();

            // Calculate convective precipitation (thunderstorms)
            CalculateInstabilityIndex(eqFronts);

            var FP = Earth.ATM.JetLevel.FP;

            Precip.Assign((r, c) =>
            {
                var hgt = Earth.SFC.Height[r, c];
                float h = 0;

                var hTop = Earth.ATM.TopLevel.H[r, c];
                var hMid = Earth.ATM.MidLevel.H[r, c];
                var hSea = Earth.ATM.SeaLevel.H[r, c];

                if (hgt > SimConstants.LevelHeights[LevelType.TopLevel])
                    h = 0.5f * hTop;
                else if (hgt > SimConstants.LevelHeights[LevelType.MidLevel])
                    h = 0.5f * (hTop + hMid);
                else
                    h = 0.5f * (hSea + hMid);
                
                var lidx = LIDX[r, c];
                var gp = GP[r, c];
                var gt = GT[r, c];
                var fp = FP[r, c];

                // Baric gradient precipitation
                var pRate = 0.5f * Math.Abs(gp);

                  // Frontal precipitation
                var f = eqFronts[r, c];
                var dx = (f > 0) ? 0.75f : 1.5f;
                var fRate = dx * 20 * Math.Abs(f);

                // Orographic precipitation
                var oRate = 0.3f * ((hgt >= 900) ? h : 0);

                // Convective precipitation
                var cRate = 0f;
                if (lidx < 0)
                {
                    var lidxRate = Math.Abs(lidx);

                    var mul = 1;
                    if (lidx > 3)
                        mul = 2;
                    if (lidx > 5)
                        mul = 3;
                    if (lidx > 7)
                        mul = 4;
                    if (lidx > 9)
                        mul = 5;

                    cRate = fp * mul * Math.Abs(lidxRate);
                }

                var totalRate = (pRate + fRate + oRate + cRate) * h / 100;

                return Math.Min(300, totalRate);
            });


            DenseMatrix SolidPrecip = MatrixFactory.Init();

            for (int r = 0; r < SolidPrecip.RowCount; r++)
            for (int c = 0; c < SolidPrecip.ColumnCount; c++)
            {
                var wl = WL[r, c];
                var ts = TS[r, c];
                var te = TE[r, c];
                var cl = Precip[r, c];
                var t01 = Earth.ATM.MidLevel.T[r, c];

                var totalRain = RAIN[r, c];
                var totalSnow = SNOW[r, c];

                var fogMeltFactor = Math.Min(1, 1 - FOG[r, c] / 100);

                const float precipClThreshold = 10f;

                if (te >= -10f || cl <= precipClThreshold)
                {
                    if (te >= 0)
                    {
                        // Accumulated soil moisture evaporation
                        totalRain -= 0.5f * te * Earth.SnapshotLength;
                        if (totalRain < 0)
                            totalRain = 0;

                        // Old snow cover is melting and transforming into water
                        // OBS: Snow melts faster when we have fog 
                        var meltedSnow = Math.Min(totalSnow, 0.2f * (1 + fogMeltFactor) * te * Earth.SnapshotLength);
                        totalSnow -= meltedSnow;
                        if (totalSnow < 0)
                            totalSnow = 0;

                        // Consider melted snow as rain because it contributes to the total soil moisture
                        totalRain += meltedSnow;
                    }
                    else
                    {
                        // Old snow cover is slowly compacting due to daytime melt / night time freeze
                        totalSnow -= 0.025f * (1 + fogMeltFactor) * Earth.SnapshotLength;

                        if (totalSnow < 0)
                            totalSnow = 0;
                    }
                }

                DaysSinceLastRainFall[r, c] = (DaysSinceLastRainFall[r, c] + Earth.SnapshotDivFactor);
                DaysSinceLastSnowFall[r, c] = (DaysSinceLastSnowFall[r, c] + Earth.SnapshotDivFactor);

                float actualPrecipRate = (cl - precipClThreshold);
                if (actualPrecipRate > 0)
                {
                    var unitPrecipFall = actualPrecipRate * Earth.SnapshotDivFactor;

                    PrecipTypeComputer<float>.Compute(
                        
                        // Actual temperatures
                        te, ts, t01,

                        // Boundary temperatures as read from simulation parameters
                        SimulationParameters.Instance,

                        // Computed precip type: snow
                        () =>
                        {
                            totalSnow += 0.3f * unitPrecipFall;
                            SolidPrecip[r, c] = 1;
                            DaysSinceLastSnowFall[r, c] = 0;
                            return 0;
                        },

                        // Computed precip type: rain
                        () =>
                        {
                            totalRain += unitPrecipFall;
                            DaysSinceLastRainFall[r, c] = 0;
                            return 0;
                        },

                        // Computed precip type: freezing rain
                        () =>
                        {
                            totalSnow += 0.1f * unitPrecipFall;
                            totalRain += 0.9f * unitPrecipFall;
                            SolidPrecip[r, c] = 1;
                            DaysSinceLastSnowFall[r, c] = 0;
                            return 0;
                        },

                        // Computed precip type: sleet
                        () =>
                        {
                            totalSnow += 0.2f * unitPrecipFall;
                            totalRain += 0.8f * unitPrecipFall;
                            SolidPrecip[r, c] = 1;
                            DaysSinceLastSnowFall[r, c] = 0;
                            return 0;
                        }
                    );
                }

                if (totalSnow < 0 || 
                    // Snow does not accumulate on a water surface if water is not frozen
                    (wl != 0 && ts > 0))
                    totalSnow = 0;

                if (totalRain < 0 ||
                    // Rain water does not accumulate on a water surface
                    wl != 0)
                    totalRain = 0;

                RAIN[r, c] = totalRain;
                SNOW[r, c] = totalSnow;
            };

            Earth.SFC.BLIZZARD.Assign((r, c) =>
            {
                var wind = WIND[r, c];
                var cl = Precip[r, c] / 20f;
                bool solidPrecip = (SolidPrecip[r, c] != 0);

                if (solidPrecip)
                    return Math.Abs(cl * wind);

                return 0;
            });

            Earth.SFC.ALBEDO.Assign((r, c) =>
            {
                var wl = WL[r, c];
                var defAlbedo = DEF_ALBEDO[r, c];

                //if (wl == 0)
                {
                    var snowAlbedo = 0f;
                    var rainAlbedo = 0f;

                    if (SNOW[r, c] > 3f)
                        snowAlbedo = SNOW[r, c] + 20f * Math.Max(0, 5 - DaysSinceLastSnowFall[r, c]);

                    if (RAIN[r, c] >= 10f)
                        rainAlbedo = 0.5f * RAIN[r, c] + 10f * Math.Max(0, 3 - DaysSinceLastRainFall[r, c]);

                    var total = Math.Min(100f, defAlbedo + snowAlbedo + rainAlbedo);

                    return total;

                    //return Math.Max(total, defAlbedo);
                }

                //return defAlbedo;
            });
        }
        
        public void CalculateMeanAirTempAtSurface()
        {
            var refTemp = References.GetRefTemp(Earth);

            TE.Assign((r, c) =>
            {
                float wl = WL[r, c];
                float lr = Earth.ATM.ELR[r, c];

                float t1 = Earth.ATM.MidLevel.T[r, c];
                float t2 = refTemp[r, c];

                float height = Earth.SFC.Height[r, c];
                    
                float dh = height - SimConstants.LevelHeights[LevelType.MidLevel];

                var te1 = (0.5f * (t1 + t2) - lr * dh / 1000);
                //var te1 = (t1 - lr * dh / 1000);

                if (SimConstants.SimBreakPoint(r, c, Earth))
                {
                    int ss = 0;
                }

                var te =  
                    SimulationParameters.Instance.AirTempContribution * te1 + 
                    SimulationParameters.Instance.SurfaceTempContribution * TS[r, c];

                return te;
            });

            var oldTS = TS.Clone() as DenseMatrix;
            TS.Assign((r, c) =>
            {
                float wl = WL[r, c];
                float old_ts = oldTS[r, c];
                float te = TE[r, c];
                float dt = (te - old_ts);
                float ts = old_ts;

                // Frozen surfaces warm up waay slower
                // And also - heaten surfaces cool down waay slower :)
                float extremeTempSurfaceFactor = 1;

                if (old_ts <= 0)
                    extremeTempSurfaceFactor = (dt > 0) ? 0.5f : 1f;
                else if (old_ts >= 30)
                    extremeTempSurfaceFactor = (dt > 0) ? 1f : 0.5f;
                    
                if (dt > 0)
                {
                    if (wl == 0)
                    {
                        // soil warms up
                        var dts = SimulationParameters.Instance.SoilTempChangeFactor * extremeTempSurfaceFactor * dt * Earth.SnapshotDivFactor;
                        ts += dts;
                    }
                    else
                    {
                        // water warms up
                        var dts = SimulationParameters.Instance.WaterTempChangeFactor * extremeTempSurfaceFactor * dt * Earth.SnapshotDivFactor;
                        ts += dts;
                        // We can't have water colder than -5 C
                        ts = Math.Max(-5, ts);
                    }
                }
                else if (dt < 0)
                {
                    if (wl == 0)
                    {
                        // soil cools down
                        var dts = SimulationParameters.Instance.SoilTempChangeFactor * extremeTempSurfaceFactor * dt * Earth.SnapshotDivFactor;
                        ts += dts;
                    }
                    else
                    {
                        // water cools down
                        var dts = SimulationParameters.Instance.WaterTempChangeFactor * extremeTempSurfaceFactor * dt * Earth.SnapshotDivFactor;
                        ts += dts;

                        // We can't have water colder than -5 C
                        ts = Math.Max(-5, ts);
                    }
                }

                return ts;

            });

            TW.Assign((r, c) =>
            {
                if (WL[r, c] != 0)
                    return TS[r, c];

                return 0;

            });

            TL.Assign((r, c) =>
            {
                if (WL[r, c] == 0)
                    return TS[r, c];

                return 0;

            });
        }

        public void CalculateDailyTempExtremes()
        {
            if (Precip == null)
                CalculateTotalPrecipitation();

            float sunLatRad = References.GetSunLatitude_Radians(Earth.UTC);
            var refTemp = References.GetRefTemp(Earth);

            TNormHigh.Assign((r, c) =>
            {
                // latitude
                float lat = EarthModel.MaxLat - r;

                float height = Earth.SFC.Height[r, c];

                float dh = height - SimConstants.LevelHeights[LevelType.MidLevel];

                // virtual temp at surface (called also equilibrium temp)
                float te = refTemp[r, c] - SimulationParameters.Instance.HumidLapseRate * dh / 1000;

                // daylength at specified time of year and latitude
                float dl = References.GetDayLength_ByRowIndex(Earth.UTC, r);

                // A factor that represents the angle of sun at high noon
                // True sun angle at high noon is: Actual latitude of the place - Sun current latitude
                // For example, At 45 grd N, on June 22nd this angle is: 45 - 23 = 22 grd = 0.384 rad
                // And the sun angle factor is hence Math.Cos(0.384) = 0.99
                float sunAngle = (float)Math.PI * lat / 180 - sunLatRad;
                float sf = (float)Math.Cos(sunAngle);

                float albedoFactor = DEF_ALBEDO[r, c] / 100;

                float max_t = te + dl * 0.5f * sf * (1 - albedoFactor);

                return max_t;
            });

            TNormLow.Assign((r, c) =>
            {
                // latitude
                float lat = EarthModel.MaxLat - r;

                float height = Earth.SFC.Height[r, c];

                float dh = height - SimConstants.LevelHeights[LevelType.MidLevel];

                // virtual temp at surface (called also equilibrium temp)
                float te = refTemp[r, c] - SimulationParameters.Instance.HumidLapseRate * dh / 1000;

                // daylength at specified time of year and latitude
                float dl = References.GetDayLength_ByRowIndex(Earth.UTC, r);

                float albedoFactor = DEF_ALBEDO[r, c] / 100;

                float min_t = te - (AbsoluteConstants.HoursPerDay - dl) * 0.5f * (albedoFactor);

                return min_t;
            });


            THigh.Assign((r, c) =>
            {
                // latitude
                float lat = EarthModel.MaxLat - r;
                // virtual temp at surface (called also equilibrium temp)
                float te = TE[r, c];
                // daylength at specified time of year and latitude
                float dl = References.GetDayLength_ByRowIndex(Earth.UTC, r);

                // A factor that represents the angle of sun at high noon
                // True sun angle at high noon is: Actual latitude of the place - Sun current latitude
                // For example, At 45 grd N, on June 22nd this angle is: 45 - 23 = 22 grd = 0.384 rad
                // And the sun angle factor is hence Math.Cos(0.384) = 0.99
                float sunAngle = (float)Math.PI * lat / 180 - sunLatRad;
                float sf = (float)Math.Cos(sunAngle);

                float albedoFactor = Earth.SFC.ALBEDO[r, c] / 100;

                // Total cloudiness
                float nn = CalculateTotalCloudiness(Precip[r, c], FOG[r, c]);

                // Factors that affect maximum temp:

                // 1. Precipitable clouds: More clouds during day time mean less energy transmitted to near-surface air => Consider (1 - cl)
                // (this is by the cloudy days are cooler even during summer)
                // OBS: Precipitable clouds are included in the total cloudiness factor

                // 2. Sun angle: The upper the sun, the more energy is transmitted => Consider Math.Cos(sunAngle)
                // (This is why the days are warmer at lower latitudes)

                // 3. Albedo: The more reflective is the surface, the less energy is transmitted => Consider (1 - albedoFactor)
                // (This is why a winter day is colder when there is a fresh snow cover, as opposite to when there is old snow or no snow at all)

                // 4. Day length: The longer the day, the more enery is tranmitted

                // 5: Fog: in a foggy day the earth sfc does not receive the same amount of energy

                // Finally the max temp can be calculated as being roughly the equilibrium temp (te) 
                // plus an amount that represents the extra energy transmitted to the near-surface air during day time.
                float max_t = te + dl * (1 - nn) * sf * (1 - albedoFactor);

                float wl = WL[r, c];
                if (wl != 0)
                {
                    float max_t_norm = TNormHigh[r, c];
                    max_t = 0.6f * max_t + 0.4f * max_t_norm;
                }

                return max_t;
            });

            TLow.Assign((r, c) =>
            {
                // latitude
                float lat = EarthModel.MaxLat - r;
                // virtual temp at surface (called also equilibrium temp)
                float te = TE[r, c];
                // daylength at specified time of year and latitude
                float dl = References.GetDayLength_ByRowIndex(Earth.UTC, r);

                // Total cloudiness
                float nn = CalculateTotalCloudiness(Precip[r, c], FOG[r, c]);

                float albedoFactor = Earth.SFC.ALBEDO[r, c] / 100;

                // Factors that affect minimum temp:

                // 1. Precipitable clouds: More clouds during night time mean less energy lost by near-surface air => Consider (1 - cl)
                // (this is why the winter nights are colder when the sky is clear)
                // OBS: Precipitable clouds are included in the total cloudiness factor

                // 2. Sun angle: There is no sun during the night; therefore the sun angle does not affect the minimum temp.

                // 3. Albedo: The more reflective is the surface, the more energy is lost => Consider (albedoFactor)
                // (this is why the winter nights are bitterly colder when there is a fresh snow cover, as opposite to when there is old snow or no snow at all)

                // 4. Night length: The longer the night, the more energy is lost. Obviously, night length = (24 hrs - day length)
                // (This is the main reason why the warmest winter nights are usually colder than the coldest summer nights ... unless other conditions are involved)

                // 5. Fog: In a foggy night, the earth sfc loses less energy than in a clear night
                // OBS: the fog is included in the total cloudiness factor

                // Finally the min temp can be calculated as being roughly the equilibrium temp (te) 
                // minus an amount that represents the extra energy lost by the near-surface air during night time.
                float min_t = te - (AbsoluteConstants.HoursPerDay - dl) * (1 - nn) * (albedoFactor);

                float wl = WL[r, c];
                if (wl != 0)
                {
                    float min_t_norm = TNormLow[r, c];
                    min_t = 0.6f * min_t + 0.4f * min_t_norm;
                }

                return min_t;
            });
        }

        private float CalculateTotalCloudiness(float precip, float fog)
        {
            // Precipitable clouds
            float cl = Math.Min(1, precip / 100);

            if (fog < 0)
                fog = 0;
            else if (fog > 50)
                fog = Math.Min(100f, 50f + 2 * (fog - 50f));

            // Low (non-precipitable) clouds
            float fl = 1 - fog / 100;

            // Total cloudiness
            float nn = (float)Math.Min(1, cl + 0.15 * fl);

            return nn;
        }

        public void CalculateFogIndex()
        {
            // Based on FSI formula: fsi = 4ts - 2(t850 + tds) + w850

            var W850 = Earth.ATM.MidLevel.P.Gradient2();
            var DIV = (Earth.ATM.MidLevel.P.Divergence()).EQ(4);

            FOG.Assign((r, c) =>
            {
                var div = DIV[r, c];
                if (div <= 0)
                {
                    var ts = TS[r, c];
                    var t850 = Earth.ATM.MidLevel.T[r, c];
                    var w850 = W850[r, c];

                    var t = TE[r, c];

                    var hMid = Earth.ATM.MidLevel.H[r, c];
                    var hSea = Earth.ATM.SeaLevel.H[r, c];
                    var h = 0.5f * (hMid + hSea);

                    var pMid = Earth.ATM.MidLevel.P[r, c];
                    var pSea = Earth.ATM.SeaLevel.P[r, c];
                    var dp = (pSea - pMid) / SimConstants.LevelHeights[LevelType.MidLevel];
                    var p = pSea - dp * Height[r, c];

                    var tds = LapseRate.DewPoint(p, t, h);

                    var fsi = 2 * (4 * ts - 2 * (t850 - tds) + w850);

                    if (fsi > 100)
                        return 100;

                    if (fsi < 0)
                        return 0;

                    return fsi;
                }

                return 100;
            });
        }
    }
}
