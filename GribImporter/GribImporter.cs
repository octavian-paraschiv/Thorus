using MathNet.Numerics.LinearAlgebra.Single;
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

        NGrib.Grib2Reader _reader;
        NGrib.Grib2.Message[] _messages;

        public GribImporter(string inputFile)
        {
            _inputFile = inputFile;
            CorrectFilePath(ref _inputFile);

            _reader = new NGrib.Grib2Reader(_inputFile);
            _messages = _reader.ReadMessages().ToArray();
        }

        public override void Dispose()
        {
            _reader.Dispose();
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

            var message = _messages.Where(m =>
            {
                if (m.DataSets?.FirstOrDefault()?.ProductDefinitionSection?.ProductDefinition is ProductDefinition0001 def &&
                    def.Parameter.HasValue && def.Parameter.Value.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                {
                    return (level == 0 ||
                        (def.FirstFixedSurfaceType == NGrib.Grib2.CodeTables.FixedSurfaceType.IsobaricSurface &&
                        def.FirstFixedSurfaceValue == level * 100));
                }

                return false;

            }).FirstOrDefault();

            var unfiltered = _reader.ReadDataSetValues(message.DataSets.First()).ToArray();
            var nodes = unfiltered.Where(gs =>
                (gs.Key.Latitude >= EarthModel.MinLat && gs.Key.Latitude <= EarthModel.MaxLat) &&
                (gs.Key.Longitude >= EarthModel.MinLon && gs.Key.Longitude <= EarthModel.MaxLon) &&
                (gs.Key.Latitude == Math.Truncate(gs.Key.Latitude)) &&
                gs.Key.Longitude == Math.Truncate(gs.Key.Longitude))
                .ToArray();
            /*
            var messages = _messages.Where(m => m.ParameterName.Contains(paramName) && m.Level == level).First();

            var nodes = messages.GridCoordinateValues.Where(gs => gs.IsMissing == false &&
                (gs.Latitude >= EarthModel.MinLat && gs.Latitude <= EarthModel.MaxLat) &&
                (gs.Longitude >= 180 + EarthModel.MinLon && gs.Longitude <= 180 + EarthModel.MaxLon) &&
                (gs.Latitude == Math.Truncate(gs.Latitude)) &&
                gs.Longitude == Math.Truncate(gs.Longitude))
                .ToArray();
            */

            if (!time)
            {
                DateTime dt = message.IdentificationSection.ReferenceTime;
                SimDateTime sdt = new SimDateTime(dt);

                string timeSeedFile = "timeSeed.thd";
                CorrectFilePath(ref timeSeedFile);
                File.WriteAllText(timeSeedFile, sdt.Title);

                time = true;
            }

            DenseMatrix mat = MatrixFactory.Init();

            foreach (var node in nodes)
            {
                int r = EarthModel.MaxLat - (int)node.Key.Latitude;
                int c = ((int)node.Key.Longitude - EarthModel.MinLon) % 360;

                try
                {
                    mat[r, c] = conversionFunc((float)node.Value.GetValueOrDefault());
                }
                catch (Exception ex)
                {
                    _ = ex.Message;
                }

            }

            if (string.IsNullOrEmpty(dataFile) == false)
            {
                FileSupport.SaveMatrixToFile(mat, dataFile, false);
            }

            return mat;
        }

    }
}
