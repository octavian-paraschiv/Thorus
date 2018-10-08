using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Data;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace ThorusCommon.Thermodynamics
{
    public static class References
    {
        #region RefTemp

        static Dictionary<string, float> _refTemp = new Dictionary<string, float>();
        static Dictionary<string, float> _dayLengthByLatitude = new Dictionary<string, float>();
        static Dictionary<string, float> _refTempYearlyDelta = new Dictionary<string, float>();
        static Dictionary<int, float> _sunLatitude = new Dictionary<int, float>();


        public static DenseMatrix GetRefTemp(EarthModel earth)
        {
            return MatrixFactory.New((r, c) => GetRefTemp(earth, r, c));
        }

        public static float GetRefTemp(EarthModel earth, int r, int c)
        {
            return GetRefTemp(earth, earth.UTC, r, c);
        }

        public static float GetRefTemp(EarthModel earth, SimDateTime sdt, int r, int c)
        {
            string key = string.Format("{0}_{1}_{2}", sdt.DayOfYear, r, c);
            lock (_refTemp)
            {
                if (_refTemp.ContainsKey(key) == false)
                {
                    int lat = EarthModel.MaxLat - r;

                    float rt = GetRefTemp_ByLatitude_MidLevel(sdt, lat) + AbsoluteConstants.WaterFreezePoint;

                    float dt = rt - AbsoluteConstants.WaterFreezePoint;
                    float f = 1.2f;

                    var wl = earth.SFC.WL[r, c];
                    if (wl == 0)
                        rt = AbsoluteConstants.WaterFreezePoint + dt * f;
                    else
                        rt = AbsoluteConstants.WaterFreezePoint + dt / f;

                    _refTemp.Add(key, rt - AbsoluteConstants.WaterFreezePoint);
                }

                return _refTemp[key];
            }
        }

        public static float GetRefTemp_ByLatitude_MidLevel(SimDateTime sdt, float lat)
        {
            float init = 0;

            float latRad = lat * (float)Math.PI / 180;

            float sunLatRad = References.GetSunLatitude_Radians(sdt);

            float sunLat = sunLatRad * 180 / (float)Math.PI;

            float deltaLatRad = Math.Abs(latRad - sunLatRad);
            float deltaLat = Math.Abs(lat - sunLat);

            float dayLen = GetDayLength_ByLatitude(sdt, lat);
            float tm = SimulationParameters.Instance._DefaultRefTemp;

            init = tm - 0.5f * deltaLat;

            float absCosLat = (float)Math.Abs(Math.Cos(deltaLatRad));

            float deltaByNight = 4f * absCosLat * (dayLen / AbsoluteConstants.HoursPerDay - 0.5f);

            float te = init + deltaByNight;

            return te;
        }
        
        #endregion

        #region DayLength_ByLatitude

        public static float GetDayLength_ByLatitude(SimDateTime sdt, float lat)
        {
            string key = string.Format("{0}_{1}", sdt.DayOfYear, lat);

            lock (_dayLengthByLatitude)
            {
                if (_dayLengthByLatitude.ContainsKey(key) == false)
                {
                    float dayLen = 0;

                    float latRan = lat * (float)Math.PI / 180;
                    float p = GetSunLatitude_Radians(sdt);

                    float sin1 = (float)Math.Sin(0.8333f * (float)Math.PI / 180);
                    float prod_sin = (float)Math.Sin(latRan) * (float)Math.Sin(p);
                    float prod_cos = (float)Math.Cos(latRan) * (float)Math.Cos(p);

                    float arg_acos = (sin1 + prod_sin) / prod_cos;
                    if (arg_acos < -1)
                        arg_acos = -1;
                    if (arg_acos > 1)
                        arg_acos = 1;

                    float acos_val = (float)Math.Acos(arg_acos);

                    float nightLen = AbsoluteConstants.HoursPerDay / (float)Math.PI * acos_val;

                    if (nightLen > AbsoluteConstants.HoursPerDay)
                        dayLen = 0;
                    else if (nightLen < 0)
                        dayLen = AbsoluteConstants.HoursPerDay;
                    else
                        dayLen = AbsoluteConstants.HoursPerDay - nightLen;

                    _dayLengthByLatitude.Add(key, dayLen);
                }

                return _dayLengthByLatitude[key];
            }
        }

        public static float GetDayLength_ByRowIndex(SimDateTime sdt, int r)
        {
            int lat = EarthModel.MaxLat - r;
            return GetDayLength_ByLatitude(sdt, lat);
        }


        #endregion

        #region SunLatitude

        public static float GetSunLatitude_Radians(SimDateTime sdt)
        {
            float delay = SimulationParameters.Instance.Delay * AbsoluteConstants.HoursPerDay;

            SimDateTime actualSdt = sdt.AddHours((int)delay);

            int yday = actualSdt.DayOfYear;

            lock (_sunLatitude)
            {
                if (_sunLatitude.ContainsKey(yday) == false)
                {
                    float p = (float)Math.Asin(0.39795f * (float)Math.Cos(0.2163108f + 2 * (float)Math.Atan(0.9671396f * (float)Math.Tan(0.00860f * (yday - 182.625f)))));
                    _sunLatitude.Add(yday, p);
                }
            }

            return _sunLatitude[yday];
        }

        #endregion

        #region TempYearlyDelta


        public static DenseMatrix GetRefTempYearlyDelta_MidLevel(EarthModel earth)
        {
            return MatrixFactory.New((r, c) => GetRefTempYearlyDelta_MidLevel(earth, r, c));
        }


        public static float GetRefTempYearlyDelta_MidLevel(EarthModel earth, int r, int c)
        {
            string key = string.Format("{0}_{1}", r, c);

            lock (_refTempYearlyDelta)
            {
                if (_refTempYearlyDelta.ContainsKey(key) == false)
                {
                    float tRefMin = 100;
                    float tRefMax = -100;

                    SimDateTime sdtStart = earth.UTC;

                    for (int i = 0; i < 365; i += 5)
                    {
                        SimDateTime sdt = sdtStart.AddHours((int)AbsoluteConstants.HoursPerDay * i);
                        float tRef = GetRefTemp(earth, sdt, r, c);
                        tRefMin = Math.Min(tRefMin, tRef);
                        tRefMax = Math.Max(tRefMax, tRef);
                    }

                    float delta = (tRefMax - tRefMin);
                    _refTempYearlyDelta.Add(key, delta);
                }
            }

            return _refTempYearlyDelta[key];
        }

        #endregion
    }
}
