using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.MatrixExtensions;

namespace ThorusCommon.Thermodynamics
{
    public static class HypsometricEquation
    {
        public static DenseMatrix Hypso(this DenseMatrix Z1, DenseMatrix T1, float levelP1, DenseMatrix T2, float levelP2)
        {
            float fP = (float)Math.Log(levelP1 / levelP2);

            DenseMatrix Z2 = MatrixFactory.New((r, c) =>
            {
                var z1 = Z1[r, c];
                var t1 = T1[r, c];
                var t2 = T2[r, c];
                var tv = AbsoluteConstants.WaterFreezePoint + 0.5f * (t1 + t2);

                var z2 = z1 + (AbsoluteConstants.Rsd * tv) / (AbsoluteConstants.g) * fP;

                return z2;
            });

            return Z2;
        }
    }
}
