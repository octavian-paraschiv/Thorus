using Grib.Api;
using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;

namespace ThorusCommon.Data
{
    public class GribImporter : FileImporter
    {
        string _inputFile = null;

        int[] _levels = { 1000, 850, 500 };

        bool time = false;

        GribMessage[] _messages = null;

        public GribImporter(string inputFile)
        {
            _inputFile = inputFile;
            CorrectFilePath(ref _inputFile);            

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

            using (var gf = new Grib.Api.GribFile(_inputFile))
                _messages = gf.ToArray();
        }

        protected override void ImportSurface()
        {
            ImportLevelData(SoilTempFile,
                "Soil temperature", -1,
                (d) => (d > 1000) ? 0 : (d - AbsoluteConstants.WaterFreezePoint));

            ImportLevelData(SnowCoverFile,
                "Water equivalent of accumulated snow depth", -1,
                (d) => (d));
        }

        protected override void ImportLevel(int idx)
        {
            // T, P, H must be read and built in this order
            // Calculation of P depends on T;
            DenseMatrix t = ImportLevelData(TempFiles[idx], 
                "Temperature", idx,
                (d) => (d > 1000) ? 0 : (d - AbsoluteConstants.WaterFreezePoint));

            if (t != null)
            {
                DenseMatrix z = ImportLevelData(null,
                    "Geopotential height", idx,
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
                           "Relative humidity", idx,
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

            int level = (levelIdx < 0) ? 0 : _levels[levelIdx];

            var messages = _messages.Where(m => m.Name.Contains(paramName) && m.Level == level).First();

            var nodes = messages.GeoSpatialValues.Where(gs => gs.IsMissing == false &&
                (gs.Latitude >= EarthModel.MinLat && gs.Latitude <= EarthModel.MaxLat) &&
                (gs.Longitude >= 180 + EarthModel.MinLon && gs.Longitude <= 180 + EarthModel.MaxLon) &&
                (gs.Latitude == Math.Truncate(gs.Latitude)) &&
                gs.Longitude == Math.Truncate(gs.Longitude))
                .ToArray();

            if (!time)
            {
                DateTime dt = messages.ReferenceTime;
                SimDateTime sdt = new SimDateTime(dt);

                string timeSeedFile = "timeSeed.thd";
                CorrectFilePath(ref timeSeedFile);
                File.WriteAllText(timeSeedFile, sdt.Title);

                time = true;
            }

            DenseMatrix mat = MatrixFactory.Init();

            foreach (var node in nodes)
            {
                int r = EarthModel.MaxLat - (int)node.Latitude;
                int c = ((int)node.Longitude - EarthModel.MinLon) % 360;
                mat[r, c] = conversionFunc((float)node.Value);
            }

            if (string.IsNullOrEmpty(dataFile) == false)
            {
                FileSupport.SaveMatrixToFile(mat, dataFile, false);
            }

            return mat;
        }

    }
}
