using MathNet.Numerics.LinearAlgebra.Single;
using System;
using ThorusCommon.Thermodynamics;

namespace ThorusCommon.Data
{
    public static class JetModelFunctions
    {
        static readonly float dx = 0.5f;

        static readonly float JetDevFactor_X = dx;
        static readonly float JetDevFactor_Y = dx;

        static readonly float RidgeDevFactor_X = 1 - JetDevFactor_X;
        static readonly float RidgeDevFactor_Y = 1 - JetDevFactor_Y;

        public static float SingleJet_WithReversal(int dayOfYear, float latRad)
        {
            float sinLat = (float)Math.Sin(latRad);
            float cosLat = (float)Math.Cos(latRad);
            return 2 * (float)Math.Abs(sinLat * cosLat) - 0.5f;
        }

        public static float DualJet_WithReversal(int dayOfYear, float latRad)
        {
            float sin2Lat = (float)Math.Sin(2 * latRad);
            float cos2Lat = (float)Math.Cos(2 * latRad);
            return -2 * (float)Math.Abs(sin2Lat * cos2Lat) + 0.5f;
        }

        public static float SingleJet_SeasonalReversal(int dayOfYear, float latRad)
        {
            float seasonDelta = 18f * (float)Math.Cos(2 * Math.PI * ((double)dayOfYear + 10) / (double)365);
            float seasonDeltaRad = (float)(seasonDelta * Math.PI / (double)180);
            return (float)(Math.Sin(2.5f * (latRad + seasonDeltaRad)) * Math.Sign(latRad));
        }

        public static float DualJet_SeasonalReversal(int dayOfYear, float latRad)
        {
            float seasonDelta = 18f * (float)Math.Cos(2 * Math.PI * ((double)dayOfYear + 10) / (double)365);
            float seasonDeltaRad = (float)(seasonDelta * Math.PI / (double)180);
            return -(float)Math.Cos(5 * (latRad + seasonDeltaRad));
        }

        public static float GetVariability(float daysElapsed)
        {
            float varSeed = SimulationParameters.Instance.JetStreamVariabilitySeed;
            float varPeriod = SimulationParameters.Instance.JetStreamVariabilityPeriod;
            return (float)Math.Sin((2 * (float)Math.PI * (varSeed / 4 + daysElapsed / varPeriod)));
        }

        public static float DevY_InvariableJet(int hoursElapsed, float snapshotDivFactor, float ridgeDevY, float lat, float lon)
        {
            float daysElapsed = (hoursElapsed / AbsoluteConstants.HoursPerDay);
            float dailyJetAdvance = SimulationParameters.Instance.JetStreamWaveSpeed;
            float deltaLonRad = daysElapsed * dailyJetAdvance * (float)Math.PI / 180;

            float lonRad = lon * (float)Math.PI / 180;
            float latRad = lat * (float)Math.PI / 180;

            float sinLat = (float)Math.Sin(latRad);
            float cosLat = (float)Math.Cos(latRad);
            float sinLon = (float)Math.Sin(SimulationParameters.Instance.JetStreamPeaks * (lonRad - deltaLonRad));

            float f = sinLat * cosLat * sinLon;

            var devY1 = f * SimulationParameters.Instance.JetStreamWaveSpeed;

            var devY = ComposeDevs(Direction.Y, devY1, ridgeDevY);

            return ((snapshotDivFactor * devY) % 180);
        }

        public static float DevY_VariableJet(int hoursElapsed, float snapshotDivFactor, float ridgeDevY, float lat, float lon)
        {
            float daysElapsed = (hoursElapsed / AbsoluteConstants.HoursPerDay);
            float dailyJetAdvance = SimulationParameters.Instance.JetStreamWaveSpeed;
            float deltaLonRad = daysElapsed * dailyJetAdvance * (float)Math.PI / 180;

            float lonRad = lon * (float)Math.PI / 180;
            float latRad = lat * (float)Math.PI / 180;

            float sinLat = (float)Math.Sin(latRad);
            float cosLat = (float)Math.Cos(latRad);
            float sinLon = (float)Math.Sin(SimulationParameters.Instance.JetStreamPeaks * (lonRad - deltaLonRad));

            float f = sinLat * cosLat * sinLon * JetModelFunctions.GetVariability(daysElapsed);

            var devY1 = f * SimulationParameters.Instance.JetStreamWaveSpeed;

            var devY = ComposeDevs(Direction.Y, devY1, ridgeDevY);

            return ((snapshotDivFactor * devY) % 180);
        }

        public static float ComposeDevs(int direction, float jetDev, float ridgeDev)
        {
            /*
            float jf = 0, rf = 0;

            switch (direction)
            {
                case Direction.X:
                    jf = JetDevFactor_X;
                    rf = RidgeDevFactor_X;
                    break;

                case Direction.Y:
                    jf = JetDevFactor_Y;
                    rf = RidgeDevFactor_Y;
                    break;
            }

            return (jf * jetDev + rf * ridgeDev);
            */

            return (jetDev + ridgeDev);
        }
    }
}
