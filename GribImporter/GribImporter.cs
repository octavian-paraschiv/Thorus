using Grib.Api;
using MathNet.Numerics.LinearAlgebra.Single;
using NGrib;
using NGrib.Grib2.CodeTables;
using NGrib.Grib2.Templates.ProductDefinitions;
using System;
using System.IO;
using System.Linq;
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

        Grib2Reader _reader = null;
        GribMessage[] _messages = null;

        public GribImporter(string inputFile)
        {
            _inputFile = inputFile;
            CorrectFilePath(ref _inputFile);
        }
        protected override void Init()
        {
            _reader = new Grib2Reader(_inputFile);
            /*
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
                _messages = gf.ToArray();*/
        }

        protected override void Cleanup()
        {
            _reader?.Dispose();
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
        private DenseMatrix ImportLevelData_v1(string dataFile, string paramName, int levelIdx, Func<float, float> conversionFunc)
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

        private DenseMatrix ImportLevelData(string dataFile, string paramName, int levelIdx, Func<float, float> conversionFunc)
            => ImportLevelData_v2(dataFile, paramName, levelIdx, conversionFunc);

        private DenseMatrix ImportLevelData_v2(string dataFile, string paramName, int levelIdx, Func<float, float> conversionFunc)
        {
            DenseMatrix mat = MatrixFactory.Init();

            try
            {
                if (string.IsNullOrEmpty(dataFile) == false)
                {
                    if (File.Exists(dataFile))
                        File.Delete(dataFile);
                }

                if (!time)
                {
                    DateTime dt = _reader.ReadMessages()
                        .Where(msg => (msg?.IdentificationSection?.ReferenceTime).HasValue)
                        .Select(msg => msg.IdentificationSection.ReferenceTime)
                        .FirstOrDefault();

                    SimDateTime sdt = new SimDateTime(dt);

                    string timeSeedFile = "timeSeed.thd";
                    CorrectFilePath(ref timeSeedFile);
                    File.WriteAllText(timeSeedFile, sdt.Title);

                    time = true;
                }

                int level = (levelIdx < 0) ? 0 : _levels[levelIdx];

                var datasets = _reader.ReadAllDataSets().Where(d =>
                    d.Parameter?.Name == paramName &&
                    (levelIdx < 0 || (d.ProductDefinitionSection?.ProductDefinition as ProductDefinition0000)?.FirstFixedSurfaceType == FixedSurfaceType.IsobaricSurface &&
                    (d.ProductDefinitionSection?.ProductDefinition as ProductDefinition0000)?.FirstFixedSurfaceValue == level));

                if (datasets?.Count() > 0)
                {
                    var dsValues = _reader.ReadDataSetValues(datasets.First());

                    var nodes = dsValues.Where(dsv =>
                        dsv.Key.Latitude >= EarthModel.MinLat && dsv.Key.Latitude <= EarthModel.MaxLat &&
                        dsv.Key.Longitude >= EarthModel.MinLon && dsv.Key.Longitude <= EarthModel.MaxLon &&
                        dsv.Key.Latitude == Math.Truncate(dsv.Key.Latitude) &&
                        dsv.Key.Longitude == Math.Truncate(dsv.Key.Longitude));

                    foreach (var node in nodes)
                    {
                        try
                        {
                            int r = EarthModel.MaxLat - (int)node.Key.Latitude;
                            int c = ((int)node.Key.Longitude - EarthModel.MinLon) % 360;
                            mat[r, c] = conversionFunc(node.Value ?? 0);
                        }
                        catch (Exception ex)
                        {
                            _ = ex;
                            //Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if (string.IsNullOrEmpty(dataFile) == false)
                FileSupport.SaveMatrixToFile(mat, dataFile, false);

            return mat;
        }
    }
}
