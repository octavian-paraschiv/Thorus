using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.IO;
using System.Linq;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.SQLite;

namespace ThorusCommon.Export
{
    public static class ExportEngine
    {
        public delegate void ExportProgressHandler(int current, int total, string desc);

        private static readonly string[] ExportRegions = new string[] { "EU", "RO" };

        public static void GenerateSubregionData(ExportProgressHandler handler)
        {
            try
            {
                Viewport[] exportRegions = Viewport.AllViewports
                    .Where(v => ExportRegions.Contains(v.Code))
                    .ToArray();

                handler?.Invoke(0, -1, "Preparing to generate subregion data...");

                string exportDbPath = Path.Combine(Directory.GetParent(SimulationData.DataFolder).FullName, "Snapshot.db3");

                MeteoDB exportDb = null;
                try
                {
                    if (File.Exists(exportDbPath))
                        File.Delete(exportDbPath);

                    File.Copy("Data/Template.db3", "./Template.db3", true);
                    exportDb = MeteoDB.OpenOrCreate(exportDbPath, true);
                }
                finally
                {
                    File.Delete("./Template.db3");
                }

                // Clean up DB in case already used eg. by local web site
                exportDb.PurgeAll<SQLite.Data>();

                var allFiles = Directory.GetFiles(SimulationData.DataFolder);
                if (allFiles != null)
                {
                    int count = 0;
                    int step = 0;

                    foreach (string file in allFiles)
                    {
                        string title = Path.GetFileNameWithoutExtension(file).ToUpperInvariant();
                        string type = title.Substring(0, 4);

                        switch (type)
                        {
                            case "C_00":
                            case "L_00":
                            case "N_00":
                            case "T_01":
                            case "T_SH":
                            case "T_SL":
                            case "T_TE":
                            case "T_TS":
                            case "F_SI":
                            case "T_NL":
                            case "T_NH":
                            case "R_00":
                            case "R_DD":
                            case "N_DD":
                            case "P_00":
                            case "P_01":
                                count += exportRegions.Length;
                                break;

                            default:
                                continue;
                        }
                    }

                    foreach (var region in exportRegions)
                    {
                        foreach (string file in allFiles)
                        {
                            string title = Path.GetFileNameWithoutExtension(file).ToUpperInvariant();
                            string type = title.Substring(0, 4);
                            string timestamp = title.Substring(9, 10);

                            DenseMatrix rawMatrix = FileSupport.LoadSubMatrixFromFile(file, region.MinLon, region.MaxLon, region.MinLat, region.MaxLat);
                            bool interpolate = (rawMatrix.RowCount < 10);
                            DenseMatrix output = interpolate ? rawMatrix.Interpolate() : rawMatrix;

                            bool isWindMap;

                            switch (type)
                            {
                                case "C_00":
                                case "L_00":
                                case "N_00":
                                case "T_01":
                                case "T_SH":
                                case "T_SL":
                                case "T_TE":
                                case "T_TS":
                                case "F_SI":
                                case "T_NL":
                                case "T_NH":
                                case "R_00":
                                case "R_DD":
                                case "N_DD":
                                    isWindMap = false;
                                    break;

                                case "P_00":
                                case "P_01":
                                    isWindMap = true;
                                    break;

                                default:
                                    continue;
                            }

                            exportDb.AddMatrix(region.Code, timestamp, type, output);

                            if (isWindMap)
                            {
                                var wind = output.ToWindComponents();
                                var module = DenseMatrix.Create(output.RowCount, output.ColumnCount, 0);
                                var angle = DenseMatrix.Create(output.RowCount, output.ColumnCount, 0);

                                for (int r = 0; r < output.RowCount; r++)
                                {
                                    for (int c = 0; c < output.ColumnCount; c++)
                                    {
                                        var wx = wind[Direction.X][r, c];
                                        var wy = wind[Direction.Y][r, c];

                                        var ang = Math.Atan2(-wy, wx);
                                        if (ang < 0)
                                            ang += 2 * Math.PI;

                                        module[r, c] = (float)Math.Sqrt(wx * wx + wy * wy);
                                        angle[r, c] = (float)ang;
                                    }
                                }

                                string mType = type.Replace("P_0", "W_0");
                                string aType = type.Replace("P_0", "W_1");

                                exportDb.AddMatrix(region.Code, timestamp, mType, module);
                                exportDb.AddMatrix(region.Code, timestamp, aType, angle);
                            }

                            handler?.Invoke(step++, count, "Generating subregion data...");
                        }
                    }
                }

                handler?.Invoke(0, -1, "Saving subregion data...");

                exportDb.SaveAndClose();

                handler?.Invoke(0, 0, "");
            }
            catch
            {
                handler?.Invoke(0, 0, "");
                throw;
            }
        }
    }
}
