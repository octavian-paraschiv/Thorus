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
    public class NetCdfImporter
    {
        static string TemperatureNcFile = "TMP_PRES.nc";
        static string GeopotentialNcFile = "HGT_PRES.nc";
        static string HumidityNcFile = "SPFH_PRES.nc";
        static string SoilTempNcFile = "TMP_BGRND.nc";
        static string SnowCoverNcFile = "WEASD_SFC.nc";

        static string SeaTempNcFile = "SST.nc";

        static string TemperatureFileLow = "T00.thd";
        static string TemperatureFileMid = "T01.thd";
        static string TemperatureFileTop = "T02.thd";

        static string GeopotentialFileLow = "Z00.thd";
        static string GeopotentialFileMid = "Z01.thd";
        static string GeopotentialFileTop = "Z02.thd";

        static string PressureFileLow = "P00.thd";
        static string PressureFileMid = "P01.thd";
        static string PressureFileTop = "P02.thd";

        static string HumidityFileLow = "H00.thd";
        static string HumidityFileMid = "H01.thd";
        static string HumidityFileTop = "H02.thd";

        static string SeaTempFile = "SST.thd";
        static string SoilTempFile = "SOIL.thd";

        static string SnowCoverFile = "SNOW.thd";
        
        static readonly int EW = SurfaceLevel.GridColumnCount;
        static readonly int EH = SurfaceLevel.GridRowCount;

        static string WorkFolder = SimulationData.WorkFolder;

        const int TotalLevels = 3;
        static string[] TempFiles = new string[TotalLevels];
        static string[] GeopotentialFiles = new string[TotalLevels];
        static string[] HumidityFiles = new string[TotalLevels];
        static string[] PressureFiles = new string[TotalLevels];

        static NetCdfImporter()
        {
            // ----------------------------------------------
            // Surface files - NC and THD
            CorrectFilePath(ref SeaTempNcFile);
            CorrectFilePath(ref SoilTempNcFile);
            CorrectFilePath(ref SnowCoverNcFile);

            CorrectFilePath(ref SeaTempFile);
            CorrectFilePath(ref SoilTempFile);
            CorrectFilePath(ref SnowCoverFile);

            // ----------------------------------------------
            // Atmosphere NC files
            CorrectFilePath(ref TemperatureNcFile);
            CorrectFilePath(ref GeopotentialNcFile);
            CorrectFilePath(ref HumidityNcFile);

            // ----------------------------------------------
            // Temperature THD files
            CorrectFilePath(ref TemperatureFileLow);
            CorrectFilePath(ref TemperatureFileMid);
            CorrectFilePath(ref TemperatureFileTop);
            
            TempFiles[0] = TemperatureFileLow;
            TempFiles[1] = TemperatureFileMid;
            TempFiles[2] = TemperatureFileTop;

            // ----------------------------------------------
            // Geopotential THD files
            CorrectFilePath(ref GeopotentialFileMid);
            CorrectFilePath(ref GeopotentialFileTop);
            CorrectFilePath(ref GeopotentialFileLow);

            GeopotentialFiles[0] = GeopotentialFileLow;
            GeopotentialFiles[1] = GeopotentialFileMid;
            GeopotentialFiles[2] = GeopotentialFileTop;

            // ----------------------------------------------
            // Pressure THD files
            CorrectFilePath(ref PressureFileMid);
            CorrectFilePath(ref PressureFileTop);
            CorrectFilePath(ref PressureFileLow);

            PressureFiles[0] = PressureFileLow;
            PressureFiles[1] = PressureFileMid;
            PressureFiles[2] = PressureFileTop;

            // ----------------------------------------------
            // Humidity THD files
            CorrectFilePath(ref HumidityFileMid);
            CorrectFilePath(ref HumidityFileTop);
            CorrectFilePath(ref HumidityFileLow);

            HumidityFiles[0] = HumidityFileLow;
            HumidityFiles[1] = HumidityFileMid;
            HumidityFiles[2] = HumidityFileTop;
        }

        public static void CorrectFilePath(ref string filePath)
        {
            filePath = Path.Combine(WorkFolder, filePath);
        }

        public static void ImportNetCdfFiles()
        {
            ImportSurface();
            ImportLevel(0); // low level    
            ImportLevel(1); // mid level
            ImportLevel(2); // top level
        }

        private static void ImportSurface()
        {
            ImportNcFile<float>(SoilTempNcFile, 
                "Temperature_depth_below_surface_layer", 1, 0, 
                SoilTempFile,
                (d) => (d > 1000) ? 0 : (d - AbsoluteConstants.WaterFreezePoint));

            ImportNcFile<float>(SeaTempNcFile, 
                "sst", 1, 0, 
                SeaTempFile,
                (d) => (d / 100), 
                false);

            ImportNcFile<float>(SnowCoverNcFile,
                "Water_equivalent_of_accumulated_snow_depth", 1, 0,
                SnowCoverFile,
                (d) => (d));

            DateTime dt = ImportDateTime(SoilTempNcFile);
            SimDateTime sdt = new SimDateTime(dt);

            string timeSeedFile = "timeSeed.thd";
            CorrectFilePath(ref timeSeedFile);
            File.WriteAllText(timeSeedFile, sdt.Title);
        }

        public static DateTime ImportDateTime(string inputNcFile)
        {
            DateTime dt = DateTime.Now;

            int ncid = 0, varid = 0;
            if (File.Exists(inputNcFile))
            {
                try
                {
                    NetCDF.nc_open(inputNcFile, NetCDF.CreateMode.NC_NOWRITE, out ncid);
                    NetCDF.nc_inq_varid(ncid, "intTime", out varid);

                    long[] hoursElapsed = new long[1];
                    NetCDF.nc_get_var_long(ncid, varid, hoursElapsed);

                    // time as an integer: YYYYMMDDHH
                    int time = (int)hoursElapsed[0];

                    int year = time / 1000000;
                    time -= 1000000 * year;

                    int month = time / 10000;
                    time -= 10000 * month;

                    int day = time / 100;
                    time -= 100 * day;

                    int hour = time;

                    return new DateTime(year, month, day, hour, 0, 0);

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

        private static void ImportLevel(int idx)
        {
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

        private static DenseMatrix ImportNcFile<T>(string netCdfFile, string netCdfVariable, int netCdfLevelCount, 
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


        private static T[] GetData<T>(string path, string variable, int levelCount, int levelIdx)
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

        public static DenseMatrix DataToMatrix<T>(T[] data, Func<T, float> conversionFunc, bool flipUpDown = true)
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
