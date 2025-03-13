using MathNet.Numerics.LinearAlgebra.Single;
using System;
using ThorusCommon.Data;

namespace ThorusCommon.MatrixExtensions
{
    public static class MatrixFactory
    {
        //-----------------------------------------------------------------

        public static DenseMatrix[] Init2D(float value = 0)
        {
            return new DenseMatrix[]
            {
                MatrixFactory.Init(value),
                MatrixFactory.Init(value),
            };
        }

        public static DenseMatrix Init(float value = 0)
        {
            return New((r, c) => value);
        }

        //-----------------------------------------------------------------

        public static DenseMatrix New(Func<int, int, float> vFunc)
        {
            if (vFunc != null)
                return DenseMatrix.Create(SurfaceLevel.GridRowCount, SurfaceLevel.GridColumnCount, vFunc);

            return DenseMatrix.Create(SurfaceLevel.GridRowCount, SurfaceLevel.GridColumnCount, (r, c) => 0);
        }

        public static DenseMatrix[] New2D(Func<int, int, float> vFuncX, Func<int, int, float> vFuncY)
        {
            return new DenseMatrix[]
            {
                MatrixFactory.New(vFuncX),
                MatrixFactory.New(vFuncY),
            };
        }

        //-----------------------------------------------------------------

        public static DenseMatrix Assign(this DenseMatrix m, Func<int, int, float> vFunc)
        {
            if (m == null)
            {
                m = MatrixFactory.New(vFunc);
            }
            else
            {
                for (int r = 0; r < m.RowCount; r++)
                    for (int c = 0; c < m.ColumnCount; c++)
                    {
                        if (vFunc != null)
                            m[r, c] = vFunc(r, c);
                        else
                            m[r, c] = 0;
                    }
            }

            return m;
        }

        public static DenseMatrix[] Assign2D(this DenseMatrix[] m2d, Func<int, int, float> vFuncX, Func<int, int, float> vFuncY)
        {
            if (m2d == null)
            {
                m2d = MatrixFactory.New2D(vFuncX, vFuncY);
            }
            else
            {
                if (m2d[Direction.X] == null)
                    m2d[Direction.X] = MatrixFactory.New(vFuncX);
                else
                    m2d[Direction.X].Assign(vFuncX);

                if (m2d[Direction.Y] == null)
                    m2d[Direction.Y] = MatrixFactory.New(vFuncY);
                else
                    m2d[Direction.Y].Assign(vFuncY);
            }

            return m2d;
        }
    }

    public static class Algebraic
    {
        const float EQ_CenterWeight = 0.7f;
        const float EQ_SideWeight = 1 - EQ_CenterWeight;

        public static DenseMatrix ADD(this DenseMatrix m1, DenseMatrix m2)
        {
            return m1.Assign((r, c) => m1[r, c] + m2[r, c]);
        }

        public static DenseMatrix MUL(this DenseMatrix m1, DenseMatrix m2)
        {
            return m1.Assign((r, c) => m1[r, c] * m2[r, c]);
        }

        public static DenseMatrix DIV(this DenseMatrix m1, DenseMatrix m2)
        {
            return m1.Assign((r, c) => m1[r, c] / m2[r, c]);
        }

        public static DenseMatrix MIN(this DenseMatrix m1, DenseMatrix m2)
        {
            return m1.Assign((r, c) => Math.Min(m1[r, c], m2[r, c]));
        }

        public static DenseMatrix MAX(this DenseMatrix m1, DenseMatrix m2)
        {
            return m1.Assign((r, c) => Math.Max(m1[r, c], m2[r, c]));
        }
        

        public static DenseMatrix ADD(this DenseMatrix m1, float val)
        {
            return m1.Assign((r, c) => m1[r, c] + val);
        }

        public static DenseMatrix MUL(this DenseMatrix m1, float val)
        {
            return m1.Assign((r, c) => m1[r, c] * val);
        }

        public static DenseMatrix DIV(this DenseMatrix m1, float val)
        {
            return m1.Assign((r, c) => m1[r, c] / val);
        }

        public static DenseMatrix MIN(this DenseMatrix m1, float val)
        {
            return m1.Assign((r, c) => Math.Min(m1[r, c], val));
        }

        public static DenseMatrix MAX(this DenseMatrix m1, float val)
        {
            return m1.Assign((r, c) => Math.Max(m1[r, c], val));
        }



        public static DenseMatrix RegionSubMatrix(this DenseMatrix globalMatrix, int minLon, int maxLon, int minLat, int maxLat)
        {
            int rowCount = maxLat - minLat + 1;
            int colCount = maxLon - minLon + 1;
            int row = EarthModel.MaxLat - maxLat;
            int col = minLon - EarthModel.MinLon;

            return globalMatrix.SubMatrix(row, rowCount, col, colCount) as DenseMatrix;
        }

        public static DenseMatrix FlipUpDown(this DenseMatrix m)
        {
            DenseMatrix m2 = DenseMatrix.Create(m.RowCount, m.ColumnCount, (r, c) => 0);

            for (int r = 0; r < m.RowCount; r++)
                for (int c = 0; c < m.ColumnCount; c++)
                {
                    m2[r, c] = m[m.RowCount - 1 - r, c];
                }

            return m2;
        }

        public static DenseMatrix FlipLeftRight(this DenseMatrix m)
        {
            DenseMatrix m2 = DenseMatrix.Create(m.RowCount, m.ColumnCount, (r, c) => 0);

            for (int r = 0; r < m.RowCount; r++)
                for (int c = 0; c < m.ColumnCount; c++)
                {
                    m2[r, c] = m[r, m.ColumnCount - 1 - c];
                }

            return m2;
        }

        public static DenseMatrix EQ(this DenseMatrix m, int order = 1)
        {
            var me = m.Clone() as DenseMatrix;

            for (int i = 0; i < order; i++)
                me = EQ_Type_2(me);

            return me;
        }

        public static DenseMatrix EQ_Type_2(DenseMatrix m)
        {
            int rc = m.RowCount;
            int cc = m.ColumnCount;
            int lim_r = rc - 1;
            int lim_c = cc - 1;

            return DenseMatrix.Create(lim_r + 1, lim_c + 1, (r, c) =>
            {
                float centerVal = m[r, c];
                float sideVal = 0;
                int sideValCnt = 0;

                var adjIdx = GetAdjustedIndexes(m, r, c, -1, -1);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                adjIdx = GetAdjustedIndexes(m, r, c, -1, 0);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                adjIdx = GetAdjustedIndexes(m, r, c, -1, 1);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                adjIdx = GetAdjustedIndexes(m, r, c, 0, -1);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                adjIdx = GetAdjustedIndexes(m, r, c, 1, -1);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;


                adjIdx = GetAdjustedIndexes(m, r, c, 0, 1);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                adjIdx = GetAdjustedIndexes(m, r, c, 1, 0);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                adjIdx = GetAdjustedIndexes(m, r, c, 1, 1);
                sideVal += m[adjIdx[Direction.R], adjIdx[Direction.C]];
                sideValCnt++;

                return 
                    EQ_CenterWeight * centerVal + 
                    EQ_SideWeight * (sideVal / sideValCnt);
            });
        }

        public static float[] MinMax(this DenseMatrix V)
        {
            return new float[]
            {
                V.Min(),
                V.Max()
            };
        }

        public static float Min(this DenseMatrix V)
        {
            float min = float.MaxValue;
            for (int r = 0; r < V.RowCount; r++)
            {
                for (int c = 0; c < V.ColumnCount; c++)
                {
                    if (V[r, c] < min)
                        min = V[r, c];
                }
            }

            return min;
        }
        public static float Max(this DenseMatrix V)
        {
            float max = float.MinValue;
            for (int r = 0; r < V.RowCount; r++)
            {
                for (int c = 0; c < V.ColumnCount; c++)
                {
                    if (V[r, c] > max)
                        max = V[r, c];
                }
            }

            return max;
        }
        public static float Mean(this DenseMatrix V)
        {
            int cnt = 0;
            float sum = 0;

            for (int r = 0; r < V.RowCount; r++)
                for (int c = 0; c < V.ColumnCount; c++)
                {
                    sum += V[r, c];
                    cnt++;
                }

            return (sum / cnt);
        }

        public static DenseMatrix Rescale(this DenseMatrix V, float[] finalScale, float[] initialScale = null)
        {
            float max = 0;
            float min = 0;

            if (initialScale == null)
            {
                min = V.Min();
                max = V.Max();
            }
            else
            {
                min = initialScale[0];
                max = initialScale[1];
            }

            return MatrixFactory.New((r, c) =>
            {
                if (min != max)
                {
                    var m = (V[r, c] - min) / (max - min);
                    return finalScale[0] + m * (finalScale[1] - finalScale[0]);
                }

                return min;
            });
        }

        public static int GetDev(float val)
        {
            var mod = Math.Abs(val);
            var sgn = Math.Sign(val);

            if (mod <= 0.5)
                return sgn;
            if (mod <= 1.5)
                return 2 * sgn;
            if (mod <= 2.5)
                return 3 * sgn;

            return (int)Math.Round(mod) * sgn;
        }

        public static DenseMatrix ApplyDeviations(this DenseMatrix V, DenseMatrix[] dev)
        {
            DenseMatrix V2 = V.Clone() as DenseMatrix;

            for (int r = 0; r < V.RowCount; r++)
            {
                for (int c = 0; c < V.ColumnCount; c++)
                {
                    int dr = 0, dc = 0;
                    dc = dev[Direction.C][r, c].Round();
                    dr = dev[Direction.R][r, c].Round();

                    var adjIdx = V.GetAdjustedIndexes(r, c, dr, dc);
                    int c2 = adjIdx[Direction.C];
                    int r2 = adjIdx[Direction.R];

                    if (r2 < V.RowCount && c2 < V.ColumnCount)
                        V2[r2, c2] = V[r, c];
                }
            }

            return V2.EQ();
        }
        // -----------------------

        public static int Round(this float d)
        {
            //float delta = d - Math.Ceiling(d);
            //if (delta > 0.5f)
            //    return (int)(d + 1);

            //return (int)(d);
            return (int)Math.Round(d);
        }

        public static int GetMinQuadrant(this DenseMatrix V, int r, int c, int dr, int dc)
        {
            float[] val = V.GetBoundingValues(r, c, dr, dc);
            float min = float.MaxValue;
            
            for (int i = 0; i < val.Length; i++)
                min = Math.Min(min, val[i]);
            
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] == min)
                    return i;
            }

            return -1;
        }

        public static float[] GetBoundingValues(this DenseMatrix V, int r, int c, int dr, int dc)
        {
            float[] val = new float[4];

            // LT: Q_0
            int[] idx = V.GetAdjustedIndexes(r, c, -dr, -dc);
            val[0] = V[idx[Direction.R], idx[Direction.C]];

            // LB: Q_1
            idx = V.GetAdjustedIndexes(r, c, -dr, dc);
            val[1] = V[idx[Direction.R], idx[Direction.C]];

            // RT: Q_2
            idx = V.GetAdjustedIndexes(r, c, dr, -dc);
            val[2] = V[idx[Direction.R], idx[Direction.C]];

            // RB: Q_3
            idx = V.GetAdjustedIndexes(r, c, dr, dc);
            val[3] = V[idx[Direction.R], idx[Direction.C]];

            return val;
        }

        public static int[] GetAdjustedIndexes(this DenseMatrix V, int r, int c, int dr, int dc)
        {
            int r2a = r + dr;
            int c2a = c + dc;
            int cc = (V.ColumnCount - 1);

            int r2 = Math.Min(V.RowCount - 1, Math.Max(0, r2a));
            int c2 = Math.Max(0, c2a % cc);

            return new int[] { c2, r2 };
        }
    }
}
