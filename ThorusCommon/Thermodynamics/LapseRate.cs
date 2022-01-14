using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Data;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;

namespace ThorusCommon.Thermodynamics
{
    public class LapseRate
    {
        public static DenseMatrix T2(DenseMatrix T1, DenseMatrix LR, DenseMatrix dH)
        {
            return MatrixFactory.New((r, c) =>
                {

                    float t1 = T1[r, c];
                    float lr = LR[r, c];
                    float dh = dH[r, c];

                    return (t1 + lr * dh / 1000);
                });
        }

        public static float EnvironmentalLapseRate(AirMassType amt, float t, float mr)
        {
            var tk = t + AbsoluteConstants.WaterFreezePoint;

            float qd =  AbsoluteConstants.Rsd * tk * tk + 
                        AbsoluteConstants.Hv * mr * tk;

            float qn =  AbsoluteConstants.Cpd * AbsoluteConstants.Rsd * tk * tk + 
                        AbsoluteConstants.Hv * AbsoluteConstants.Hv * mr * AbsoluteConstants.eps;

            float elr = 1000 * AbsoluteConstants.g * qd / qn;

            switch (amt)
            {
                case AirMassType.ContinentalTropical:
                    elr += 2.0f;
                    break;

                case AirMassType.MaritimeTropical:
                    elr += 1.0f;
                    break;

                case AirMassType.WarmMaritimePolar:
                    elr += 0.5f;
                    break;

                case AirMassType.ColdMaritimePolar:
                    elr -= 0.5f;
                    break;

                case AirMassType.ContinentalPolar:
                    elr -= 1.0f;
                    break;

                case AirMassType.Arctic:
                    elr -= 2.0f;
                    break;

                default:
                    break;
            }

            return elr;
        }

        public static float MixingRatio(float p, float t, float h)
        {
            float a = 6.116441f;
            float m = 7.591386f;
            float tn = 240.7263f;

            if (t < 0)
            {
                a = 6.114742f;
                m = 9.778707f;
                tn = 273.1466f;
            }

            // water vapour saturation pressure
            float pws = a * (float)Math.Pow(10, (m * t) / (t + tn));

            // actual vapour pressure
            float pw = pws * h / 100;

            // and the mixing ratio
            float r = AbsoluteConstants.eps * pw / (p - pw);

            return r;
        }

        public static float DewPoint(float p, float t, float h)
        {
            float a = 6.116441f;
            float m = 7.591386f;
            float tn = 240.7263f;

            if (t < 0)
            {
                a = 6.114742f;
                m = 9.778707f;
                tn = 273.1466f;
            }
            

            // water vapour saturation pressure
            float pw =  (float)Math.Pow(10, (m * t) / (t + tn)) * h / 100;

            float td = tn / (m / (float)Math.Log10(pw) - 1);

            return td;
        }
    }
}
