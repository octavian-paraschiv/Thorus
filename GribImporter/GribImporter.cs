using Grib.Api;
using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;

namespace ThorusCommon.Data
{
    public class GribImporter : FileImporter
    {
        static string InputFile = "input.grb";

        GribFile _gf = null;

        public GribImporter()
        {
            CorrectFilePath(ref InputFile);

            

            bool ok = false;
            while (!ok)
            {
                try
                {
                    GribEnvironment.Init();
                    ok = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            _gf = new Grib.Api.GribFile(InputFile);

            foreach (var msg in _gf)
                Console.WriteLine(msg);
        }

        protected override void ImportSurface()
        {
        }

        protected override void ImportLevel(int idx)
        {
            // T, P, H must be read and built in this order
            // Calculation of P depends on T;
            DenseMatrix t = ImportLevelData(TempFiles[idx], 
                idx == 0 ? "2t" : "t", idx,
                (d) => (d > 1000) ? 0 : (d - AbsoluteConstants.WaterFreezePoint));

            if (t != null)
            {
                DenseMatrix z = ImportLevelData(null,
                    "gh", idx,
                    (d) => (d));

                if (z != null)
                {
                    DenseMatrix p = DenseMatrix.Create(z.RowCount, z.ColumnCount, (r, c) =>
                    {
                        float dz = z[r, c] - SimConstants.LevelHeights[idx];
                        float rt = AbsoluteConstants.Rsd * (t[r, c] + AbsoluteConstants.WaterFreezePoint);

                        float levelPressure = 0;
                        switch (idx)
                        {
                            case LevelType.SeaLevel:
                                levelPressure = LevelPressure.SeaLevelPressure;
                                break;
                            case LevelType.MidLevel:
                                levelPressure = LevelPressure.MidLevelPressure;
                                break;
                            case LevelType.TopLevel:
                                levelPressure = LevelPressure.TopLevelPressure;
                                break;
                        }

                        return levelPressure * (float)Math.Exp(AbsoluteConstants.g * dz / rt);
                    });

                    if (p != null)
                    {
                        FileSupport.SaveMatrixToFile(p, PressureFiles[idx], false);

                        DenseMatrix q = ImportLevelData(HumidityFiles[idx],
                           "r", idx,
                           (d) => (d));
                    }
                }
            }
        }

        private DenseMatrix ImportLevelData(string dataFile, string paramName, int levelIdx, Func<float, float> conversionFunc)
        {
            if (string.IsNullOrEmpty(dataFile) == false)
            {
                if (File.Exists(dataFile))
                    File.Delete(dataFile);
            }

            DenseMatrix tmp = GetLevelData(paramName, levelIdx);
            DenseMatrix mat = MatrixFactory.Init();

            mat.Assign((r, c) => conversionFunc(tmp[r, c]));

            if (string.IsNullOrEmpty(dataFile) == false)
            {
                FileSupport.SaveMatrixToFile(mat, dataFile, false);
            }

            return mat;
        }

        private DenseMatrix GetLevelData(string paramName, int levelIdx)
        {
            DenseMatrix mat = MatrixFactory.Init();

            GribMessage msg = SelectMessage(levelIdx, paramName);
            if (msg != null && msg.GridCoordinateValues != null)
            {
                var list = msg.GridCoordinateValues.ToList();

                for (int lat = EarthModel.MinLat; lat < EarthModel.MaxLat; lat++)
                    for (int lon = EarthModel.MinLon; lon < EarthModel.MaxLon; lon++)
                    {
                        int r = EarthModel.MaxLat - lat;
                        int c = lon - EarthModel.MinLon;

                        int idx = 360 * r + c;
                        mat[r, c] = (float)list[idx].Value;
                    }
            }

            return mat;
        }

        private GribMessage SelectMessage(int levelIdx, string paramShortName)
        {
            int level;

            switch (levelIdx)
            {
                case 0:
                    level = 2;
                    break;

                case 1:
                    level = 850;
                    break;

                case 2:
                default:
                    level = 500;
                    break;
            }


            return (from msg in _gf
                    where (msg.Level == level && msg.ParameterShortName == paramShortName)
                    select msg).FirstOrDefault();
        }
    }
}
