using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using Microsoft.Research.ScientificDataSet.NetCDF4;
using System.IO;
using ThorusCommon.Thermodynamics;
using MathNet.Numerics;
using ThorusCommon;
using ThorusCommon.Data;
using ThorusCommon.Engine;

namespace ThorusCommon.Data
{
    public class NetCdfImporter : FileImporter
    {
        protected string TemperatureNcFile = "TMP_PRES.nc";
        protected string GeopotentialNcFile = "HGT_PRES.nc";
        protected string HumidityNcFile = "SPFH_PRES.nc";
        protected string SoilTempNcFile = "TMP_BGRND.nc";
        protected string SnowCoverNcFile = "WEASD_SFC.nc";

        protected string SeaTempNcFile = "SST.nc";

        private bool _sstOnly = false;
        
        public NetCdfImporter(bool sstOnly = false)
        {
            _sstOnly = sstOnly;

            // ----------------------------------------------
            // Surface files - NC
            CorrectFilePath(ref SeaTempNcFile);

            if (_sstOnly)
                return;

            CorrectFilePath(ref SoilTempNcFile);
            CorrectFilePath(ref SnowCoverNcFile);

            // ----------------------------------------------
            // Atmosphere NC files
            CorrectFilePath(ref TemperatureNcFile);
            CorrectFilePath(ref GeopotentialNcFile);
            CorrectFilePath(ref HumidityNcFile);
        }

        protected override void ImportSurface()
        {
            ImportNcFile<float>(SeaTempNcFile,
                          "sst", 1, 0,
                          SeaTempFile,
                          (d) => (d / 100),
                          false);

            DateTime dt = ImportDateTime(SeaTempNcFile);
            SimDateTime sdt = new SimDateTime(dt);

            string timeSeedFile = "timeSeed.thd";
            CorrectFilePath(ref timeSeedFile);
            File.WriteAllText(timeSeedFile, sdt.Title);

            if (_sstOnly)
                return;

            ImportNcFile<float>(SoilTempNcFile, 
                "Temperature_depth_below_surface_layer", 1, 0, 
                SoilTempFile,
                (d) => (d > 1000) ? 0 : (d - AbsoluteConstants.WaterFreezePoint));

            ImportNcFile<float>(SnowCoverNcFile,
                "Water_equivalent_of_accumulated_snow_depth", 1, 0,
                SnowCoverFile,
                (d) => (d));
        }

        public static DateTime ImportDateTime(string inputNcFile)
        {
            DateTime dt = DateTime.Now;

            int ncid = 0, varid = 0;

            CorrectFilePath(ref inputNcFile);

            if (File.Exists(inputNcFile))
            {
                try
                {
                    NetCDF.nc_open(inputNcFile, NetCDF.CreateMode.NC_NOWRITE, out ncid);
                    NetCDF.nc_inq_varid(ncid, "time", out varid);

                    // Days since 1800-01-01
                    long[] daysElapsed = new long[1];
                    NetCDF.nc_get_var_long(ncid, varid, daysElapsed);

                    dt = new DateTime(1800, 1, 1, 0, 0, 0);
                    dt = dt.AddDays(daysElapsed[0]);

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    NetCDF.nc_close(ncid);
                }
            }

            return dt;
        }

        protected override void ImportLevel(int idx)
        {
            if (_sstOnly)
                return;

            // T, P, H must be read and built in this order
            // Calculation of P depends on T;
            // Calculation of H depends on both T and P.

            DenseMatrix t = ImportNcFile<float>(TemperatureNcFile,
                "Temperature", TempFiles.Length, idx,
                TempFiles[idx],
                (d) => (d > 1000) ? 0 : (d - AbsoluteConstants.WaterFreezePoint));

            if (t != null)
            {
                DenseMatrix z = ImportNcFile<float>(GeopotentialNcFile,
                    "Geopotential_height", GeopotentialFiles.Length, idx,
                    null,
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
                                // Caution: use 1000 and not LevelPressure.SeaLevelPressure
                                // because we imported 1000 hPa geopotential which is not
                                // quite the same as sea level geopotential.
                                // works with sea leve
                                levelPressure = 1000;
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

                        DenseMatrix q = ImportNcFile<float>(HumidityNcFile,
                            "Specific_humidity", GeopotentialFiles.Length, idx,
                            null,
                            (d) => (d));

                        if (q != null)
                        {
                            DenseMatrix h = DenseMatrix.Create(q.RowCount, q.ColumnCount, (r, c) =>
                            {
                                float temp = t[r, c];
                                float qair = q[r, c];
                                float press = p[r, c];

                                float es = 6.112f * (float)Math.Exp((17.67f * temp) / (temp + 243.5f));
                                float e = qair * press / (0.378f * qair + 0.622f);
                                float rh = e / es;
                                return 100 * Math.Min(1, Math.Max(0, rh));

                            });

                            FileSupport.SaveMatrixToFile(h, HumidityFiles[idx], false);
                        }
                    }
                }
            }
        }

        private DenseMatrix ImportNcFile<T>(string netCdfFile, string netCdfVariable, int netCdfLevelCount, 
            int netCdfLevelIdx, string dataFile, Func<T, float> conversionFunc, bool flipUpDown = true)
        {
            DenseMatrix mat = null;

            if (File.Exists(netCdfFile))
            {
                if (File.Exists(dataFile))
                    File.Delete(dataFile);

                T[] data = GetData<T>(netCdfFile, netCdfVariable, netCdfLevelCount, netCdfLevelIdx);
                mat = DataToMatrix(data, (d) => conversionFunc(d), flipUpDown);

                if (string.IsNullOrEmpty(dataFile) == false)
                {
                    FileSupport.SaveMatrixToFile(mat, dataFile, false);
                }
            }

            return mat;
        }


        private T[] GetData<T>(string path, string variable, int levelCount, int levelIdx)
        {
            int ncid = 0, varid = 0;

            if (File.Exists(path))
            {
                try
                {
                    Console.WriteLine($"Reading data from: {Path.GetFileName(path)}, variable: {variable}, level {levelIdx} of {levelCount} ");

                    NetCDF.nc_open(path, NetCDF.CreateMode.NC_NOWRITE, out ncid);
                    NetCDF.nc_inq_varid(ncid, variable, out varid);

                    if (typeof(T) == typeof(float))
                    {
                        float[] data = new float[levelCount * EH * EW];
                        float[] levelData = new float[EH * EW];

                        NetCDF.nc_get_var_float(ncid, varid, data);

                        Array.Copy(data, levelIdx * EH * EW, levelData, 0, EH * EW);
                        return levelData as T[];
                    }

                    if (typeof(T) == typeof(short))
                    {
                        short[] data = new short[levelCount * EH * EW];
                        short[] levelData = new short[EH * EW];

                        NetCDF.nc_get_var_short(ncid, varid, data);

                        Array.Copy(data, levelIdx * EH * EW, levelData, 0, EH * EW);
                        return levelData as T[];
                    }
                }
                catch { }
                finally
                {
                    NetCDF.nc_close(ncid);
                }
            }

            return null;
        }

        public DenseMatrix DataToMatrix<T>(T[] data, Func<T, float> conversionFunc, bool flipUpDown = true)
        {
            DenseMatrix mat = MatrixFactory.New((r, c) =>
            {
                int sc = (180 + c) % EW;

                int dataIdx = r * EW + sc;

                float dataElement = conversionFunc(data[dataIdx]);
                return dataElement;
            });

            if (flipUpDown)
                return mat.FlipUpDown();

            return mat;
        }
    }
}
