using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Thermodynamics;
using ThorusCommon.Data;

namespace ThorusCommon.MatrixExtensions
{
    public static class Calculus
    {
        // TODO: move to dedicated atmosheric calculus class
        public static DenseMatrix EnsureScale(this DenseMatrix V, float levelPressure)
        {
            float[] range = levelPressure.IdentifyPressureRange();
            return V.EnsureScale(range);
        }

        // TODO: move to dedicated atmosheric calculus class
        public static DenseMatrix EnsureScale(this DenseMatrix V, float[] range)
        {
            if (range == null)
                range = V.IdentifyPressureRange();

            float min = range[0];
            float max = range[1];

            var V2 = MatrixFactory.New((r, c) =>
            {
                return Math.Min(max, Math.Max(min, V[r, c]));
            }).EQ();

            return V2;
        }

        // TODO: move to dedicated atmosheric calculus class
        public static float[] IdentifyPressureRange(this float p)
        {
            var M = DenseMatrix.Create(1, 1, (r, c) => p);
            float[] ret = M.IdentifyPressureRange();
            return ret;
        }

        // TODO: move to dedicated atmosheric calculus class
        public static float[] IdentifyPressureRange(this DenseMatrix V)
        {
            float avg = 0;
            float sc = 1.07683943f;
            float sc2 = 1 + 2 * (sc - 1);
            
            if (V != null)
                avg = V.Mean();

            if (LevelPressure.SeaLevelPressure / sc2 <= avg)
                return LevelPressureExtremes.SeaLevelExtremes;

            if (LevelPressure.MidLevelPressure / sc2 <= avg)
                return LevelPressureExtremes.MidLevelExtremes;

            if (LevelPressure.TopLevelPressure / sc2 <= avg)
                return LevelPressureExtremes.TopLevelExtremes;

            return LevelPressureExtremes.JetLevelExtremes;
        }

        /// <summary>
        /// Calculates the divergence of a scalar 2D-field
        /// </summary>
        /// <param name="V">The scalar 2D-field</param>
        /// <returns>The divergence of the scalar field.</returns>
        public static DenseMatrix Divergence(this DenseMatrix V)
        {
            // The divergence of a scalar field 
            // is a scalar field given by the sum of 2nd-order derivatives of the scalar field
            // in all available directions.
            //
            // I.e. given the scalar 2D-field V(x, y)
            // then the divergence is div(V) = d2x/dx(V) + d2y/dy(V)
            //

            // Calculate 2nd-order derivatives by deriving 2 times in the respective direction :)
            // And finally perform the sum of 2nd-order derivatives
            return (DX(DX(V)) + DY(DY(V)));
        }

        /// <summary>
        /// Calculates the gradient of a scalar 2D-field
        /// </summary>
        /// <param name="V">The scalar 2D-field</param>
        /// <returns>The gradient of the scalar field, as a 2D-vector in (x, y) directions.
        /// First element (index 0) is the gradient in X direction, 
        /// second element (index 1) is the gradient in Y direction.</returns>
        public static DenseMatrix[] Gradient(this DenseMatrix V)
        {
            // The gradient of a scalar field
            // is a vector field comprised by first derivatives of the scalar field
            // in all available directions
            //
            // I.e. given the scalar 2D-field V(x, y)
            // then the gradient is grad(V) = [d/dx(V), d/dy(V)]
            //
            return new DenseMatrix[] { DX(V), DY(V) };
        }

        public static DenseMatrix Gradient2(this DenseMatrix V)
        {
            DenseMatrix[] grad = V.Gradient();
            return DenseMatrix.Create(V.RowCount, V.ColumnCount, (r, c) =>
                {
                    var gx2 = grad[Direction.X][r, c] * grad[Direction.X][r, c];
                    var gy2 = grad[Direction.Y][r, c] * grad[Direction.Y][r, c];

                    return (float)Math.Sqrt(gx2 + gy2);
                });
        }

        /// <summary>
        /// First derivative of a scalar 2D-field, in X direction.
        /// </summary>
        /// <param name="V">The scalar 2D-field.</param>
        /// <returns>First derivative in X direction.</returns>
        public static DenseMatrix DX(this DenseMatrix V)
        {
            DenseMatrix gx = new DenseMatrix(V.RowCount, V.ColumnCount);
            for (int r = 0; r < V.RowCount; r++)
            {
                for (int c = 0; c < V.ColumnCount; c++)
                {
                    if (c > 0 && c < V.ColumnCount - 1)
                        gx[r, c] = (V[r, c + 1] - V[r, c - 1]) / 2;
                    else if (c == 0)
                        gx[r, c] = (V[r, 1] - V[r, 0]);
                    else if (c == V.ColumnCount - 1)
                        gx[r, V.ColumnCount - 1] = (V[r, V.ColumnCount - 1] - V[r, V.ColumnCount - 2]);
                }
            }

            return gx;
        }


        /// <summary>
        /// First derivative of a scalar 2D-field, in X direction.
        /// </summary>
        /// <param name="V">The scalar 2D-field.</param>
        /// <returns>First derivative in X direction.</returns>
        public static DenseMatrix DY(this DenseMatrix V)
        {
            DenseMatrix gy = new DenseMatrix(V.RowCount, V.ColumnCount);
            for (int r = 0; r < V.RowCount; r++)
            {
                for (int c = 0; c < V.ColumnCount; c++)
                {
                    if (r > 0 && r < V.RowCount - 1)
                        gy[r, c] = (V[r + 1, c] - V[r - 1, c]) / 2;
                    else if (r == 0)
                        gy[r, c] = (V[1, c] - V[0, c]);
                    else if (r == V.RowCount - 1)
                        gy[V.RowCount - 1, c] = (V[V.RowCount - 1, c] - V[V.RowCount - 2, c]);
                }
            }

            return gy;
        }

        public static DenseMatrix[] ToWindComponents(this DenseMatrix V, float rot = SimConstants.AngleBetweenWindAndIsobars)
        {
            var G = V.Gradient();
            //return Calculus.Rotate(G, rot);

            var x = G[Direction.X];
            var y = G[Direction.Y];

            int rc = x.RowCount;
            int cc = x.ColumnCount;

            return new DenseMatrix[]
            {
                DenseMatrix.Create(rc, cc, (r, c) => 
                    {
                        float lat = EarthModel.MaxLat - r;
                        var p = rot * Math.Sign(lat);
                        return x[r, c] * (float)Math.Cos(p) - y[r, c] * (float)Math.Sin(p);
                    }),

                DenseMatrix.Create(rc, cc, (r, c) => 
                    {
                        float lat = EarthModel.MaxLat - r;
                        var p = rot * Math.Sign(lat);
                        return x[r, c] * (float)Math.Sin(p) + y[r, c] * (float)Math.Cos(p);
                    })
            };
        }

        public static DenseMatrix Vorticity(this DenseMatrix V, float angleToIsobars)
        {
            DenseMatrix[] wind = V.ToWindComponents(angleToIsobars);
            DenseMatrix VX = wind[Direction.X];
            DenseMatrix VY = wind[Direction.Y];

            var VY_DX = VY.DX();
            var VX_DY = VX.DY();
            var curl = (VY_DX - VX_DY);

            return curl;
        }

        //public static DenseMatrix[] Rotate(this DenseMatrix[] grad, float p)
        //{
        //    var x = grad[Direction.X];
        //    var y = grad[Direction.Y];

        //    int rc = x.RowCount;
        //    int cc = x.ColumnCount;

        //    return new DenseMatrix[]
        //    {
        //        DenseMatrix.Create(rc, cc, (r, c) => (x[r, c] * (float)Math.Cos(p) - y[r, c] * (float)Math.Sin(p))),
        //        DenseMatrix.Create(rc, cc, (r, c) => (x[r, c] * (float)Math.Sin(p) + y[r, c] * (float)Math.Cos(p))),
        //    };
        //}

        public static DenseMatrix Interpolate(this DenseMatrix dm)
        {
            DenseMatrix dm2 = DenseMatrix.Create(2 * dm.RowCount - 1, 2 * dm.ColumnCount - 1, (r, c) => -1001f);

            for (int r = 0; r < dm.RowCount; r++)
                for (int c = 0; c < dm.ColumnCount; c++)
                {
                    int r2 = 2 * r;
                    int c2 = 2 * c;

                    try
                    {
                        if (dm2[r2, c2] < -1000)
                            dm2[r2, c2] = dm[r, c];

                    }
                    catch
                    {
                    }
                }

            for (int r2 = 0; r2 < dm2.RowCount; r2++)
                for (int c2 = 0; c2 < dm2.ColumnCount; c2++)
                {
                    if (dm2[r2, c2] > -1000)
                        continue;

                    if (r2 % 2 != 0)
                        continue;
                    try
                    {
                        dm2[r2, c2] = 0.5f * (dm2[r2, c2 - 1] + dm2[r2, c2 + 1]);
                    }
                    catch { }
                }

            for (int r2 = 0; r2 < dm2.RowCount; r2++)
                for (int c2 = 0; c2 < dm2.ColumnCount; c2++)
                {
                    if (dm2[r2, c2] > -1000)
                        continue;

                    if (c2 % 2 != 0)
                        continue;

                    try
                    {
                        dm2[r2, c2] = 0.5f * (dm2[r2 - 1, c2] + dm2[r2 + 1, c2]);
                    }
                    catch { }
                }

            for (int r2 = 0; r2 < dm2.RowCount; r2++)
                for (int c2 = 0; c2 < dm2.ColumnCount; c2++)
                {
                    if (dm2[r2, c2] > -1000)
                        continue;

                    try
                    {
                        float sum = 0;

                        sum += dm2[r2 - 1, c2 - 1];
                        sum += dm2[r2 - 1, c2 + 0];
                        sum += dm2[r2 - 1, c2 + 1];

                        sum += dm2[r2 + 0, c2 - 1];
                        sum += dm2[r2 + 0, c2 + 1];

                        sum += dm2[r2 + 1, c2 - 1];
                        sum += dm2[r2 + 1, c2 + 0];
                        sum += dm2[r2 + 1, c2 + 1];

                        dm2[r2, c2] = sum / 8f;
                    }
                    catch { }
                }

            return dm2;
        }
    }
}
