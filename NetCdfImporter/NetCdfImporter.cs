using MathNet.Numerics.LinearAlgebra.Single;
using Microsoft.Research.ScientificDataSet.NetCDF4;
using System;
using System.IO;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;

namespace ThorusCommon.Data
{
    public class NetCdfImporter : FileImporter
    {
        protected string SeaTempNcFile = "SST.nc";

        public NetCdfImporter()
        {
            // ----------------------------------------------
            // Surface files - NC
            CorrectFilePath(ref SeaTempNcFile);
        }

        protected override void ImportLevel(int idx)
        {
        }

        protected override void ImportSurface()
        {
            DateTime dt = ImportDateTime(SeaTempNcFile);
            SimDateTime sdt = new SimDateTime(dt);

            string timeSeedFile = "timeSeed.thd";
            CorrectFilePath(ref timeSeedFile);
            File.WriteAllText(timeSeedFile, sdt.Title);

            ImportSstFile<float>(SeaTempNcFile,
                          "sst", 1, 0,
                          SeaTempFile,
                          true);

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

        private DenseMatrix ImportSstFile<T>(string netCdfFile, string netCdfVariable, int netCdfLevelCount,
            int netCdfLevelIdx, string dataFile, bool flipUpDown = true)
        {
            DenseMatrix mat = null;

            if (File.Exists(netCdfFile))
            {
                if (File.Exists(dataFile))
                    File.Delete(dataFile);

                const int rows = 720;
                const int cols = 1440;

                float[] data = GetData(netCdfFile, netCdfVariable, netCdfLevelCount, netCdfLevelIdx, rows, cols);
                DenseMatrix bigMat = DataToMatrix(rows, cols, data, flipUpDown);

                // bigMat has size 720 x 1440 so we need to transform it to 181 * 361
                float last = 0;
                mat = MatrixFactory.New((r, c) =>
                {
                    var c1 = (c + 180) % 360;

                    try
                    {
                        last = bigMat.SubMatrix(4 * r, 4, 4 * c1, 4).RowSums().Sum() / 16;
                    }
                    catch
                    {
                        last = bigMat.SubMatrix(4 * (r - 1), 4, 4 * c1, 4).RowSums().Sum() / 16;
                    }
                    return last;
                });

                if (string.IsNullOrEmpty(dataFile) == false)
                {
                    FileSupport.SaveMatrixToFile(mat, dataFile, false);
                }
            }

            return mat;
        }


        private float[] GetData(string path, string variable, int levelCount, int levelIdx, int rows, int cols)
        {
            int ncid = 0, varid = 0;

            if (File.Exists(path))
            {
                try
                {
                    Console.WriteLine($"Reading data from: {Path.GetFileName(path)}, variable: {variable}, level {levelIdx} of {levelCount} ");

                    NetCDF.nc_open(path, NetCDF.CreateMode.NC_NOWRITE, out ncid);
                    NetCDF.nc_inq_varid(ncid, variable, out varid);

                    float[] data = new float[levelCount * rows * cols];
                    float[] levelData = new float[rows * cols];

                    NetCDF.nc_get_var_float(ncid, varid, data);

                    Array.Copy(data, levelIdx * rows * cols, levelData, 0, rows * cols);
                    return levelData;
                }
                catch { }
                finally
                {
                    NetCDF.nc_close(ncid);
                }
            }

            return null;
        }

        public DenseMatrix DataToMatrix(int rows, int cols, float[] data, bool flipUpDown = true)
        {
            float last = 0;
            DenseMatrix mat = DenseMatrix.Create(rows, cols, (r, c) =>
            {
                int sc = c % cols;
                int dataIdx = r * cols + sc;
                float val = (float)Math.Round(data[dataIdx], 1);

                if (Math.Abs(val) < 0.05f || Math.Abs(val) > 1000)
                    val = last;
                else
                    last = val;

                return val;
            });

            if (flipUpDown)
                return mat.FlipUpDown();

            return mat.EQ(8);
        }
    }
}
