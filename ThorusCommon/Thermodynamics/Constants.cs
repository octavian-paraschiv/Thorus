#define HAVE_TESTSIMBREAKPOINT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ThorusCommon.Data;
using ThorusCommon.Engine;

namespace ThorusCommon.Engine
{
   
}

namespace ThorusCommon
{
    public static class LevelType
    {
        public const int SeaLevel = 0;
        public const int MidLevel = 1;
        public const int TopLevel = 2;
        public const int JetLevel = 3;
    }

    public static class LevelPressure
    {
        public const float SeaLevelPressure = 1015;
        public const float MidLevelPressure = 850;
        public const float TopLevelPressure = 500;
        public const float JetLevelPressure = 300;
    }

    public static class LevelPressureExtremes
    {
        public static readonly float[] SeaLevelExtremes = new float[] { 940, 1090 };
        public static readonly float[] MidLevelExtremes = new float[] { 770, 920 };
        public static readonly float[] TopLevelExtremes = new float[] { 460, 610 };
        public static readonly float[] JetLevelExtremes = new float[] { 240, 360 };
    }

    public static class Direction
    {
        public const int X = 0;
        public const int Y = 1;

        public const int C = X;
        public const int R = Y;
    }

}

namespace ThorusCommon.Thermodynamics
{
    public class AbsoluteConstants
    {
        /// <summary>
        /// Earth's gravitational acceleration
        /// </summary>
        public const float g = 9.8076f;

        /// <summary>
        /// Specific gas constant of dry air
        /// </summary>
        public const float Rsd = 287f;

        /// <summary>
        /// Specific gas constant of water vapors
        /// </summary>
        public const float Rsw = 461.5f;

        /// <summary>
        /// Heat of vaporization of water
        /// </summary>
        public const float Hv = 2.501e6f;

        /// <summary>
        /// Dimensionless ratio of the specific gas constant of dry air to the specific gas constant for water vapour
        /// </summary>
        public const float eps = Rsd / Rsw;

        /// <summary>
        /// The specific heat of dry air at constant pressure
        /// </summary>
        public const float Cpd = 1003.5f;

        /// <summary>
        /// Freezing temperature of water, in K
        /// </summary>
        public const float WaterFreezePoint = 273.15f;

        /// <summary>
        /// Duration of one earth day, in hours
        /// </summary>
        public const float HoursPerDay = 24f;

        /// <summary>
        /// Angular rotation speed of the Earth
        /// </summary>
        public const float EarthRotationSpeed = 7.2921e-5f;
    }

    public static class SimConstants
    {
        public static float FAdvanceOld = 0.2f;
        public static float FAdvanceNew = 1 - FAdvanceOld;

        public static float FSeaMixOld = 0.5f;
        public static float FSeaMixNew = 1 - FSeaMixOld;

        public static float FTopMixOld = 0.8f;
        public static float FTopMixNew = 1 - FTopMixOld;

        public static readonly float[] LevelHeights = 
        { 
            50,
            1500,
            5500,
            10000,
        };

        public static readonly float[] LevelJetWeights =
        {
           50f / (50 + 1500 + 5500),
           1500f / (50 + 1500 + 5500),
           5500f / (50 + 1500 + 5500),
        };
      

        public static readonly float[] Thicknesses = 
        { 
            0,
            LevelHeights[LevelType.MidLevel] - LevelHeights[LevelType.SeaLevel],
            LevelHeights[LevelType.MidLevel] - LevelHeights[LevelType.TopLevel],
            LevelHeights[LevelType.TopLevel] - LevelHeights[LevelType.JetLevel],
        };


        public const float AngleBetweenWindAndIsobars = (float)(-(float)Math.PI / 2);

        public static readonly float TEST_Lon = 25f;
        public static readonly float TEST_Lat = 44f;

        public static bool SimBreakPoint(int r, int c, EarthModel model = null)
        {
            SimDateTime compareTo = new SimDateTime("2019-06-20_00");

#if HAVE_TESTSIMBREAKPOINT

            if (model == null || model.UTC.GetHoursOffset(compareTo) >= 0)
            {
                return (r == EarthModel.MaxLat - (int)SimConstants.TEST_Lat &&
                    c == EarthModel.MaxLon + (int)SimConstants.TEST_Lon);
            }
#endif
            return false;
        }


    }

}
