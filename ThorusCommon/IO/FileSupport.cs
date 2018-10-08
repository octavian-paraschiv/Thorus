using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;

namespace ThorusCommon.IO
{
    public static class FileSupport
    {
        public static DenseMatrix Load(string title, string type)
        {
            string fileTitle = string.Format("{0}_{1}.thd", type, title);
            string filePath = Path.Combine(SimulationData.DataFolder, fileTitle);

            if (File.Exists(filePath) == false)
                throw new FileNotFoundException();

            return LoadMatrixFromFile(filePath);
        }

        public static DenseMatrix LoadMatrixFromFile(string filePath)
        {
            try
            {
                return new MatrixFile(filePath, true).Matrix;
            }
            catch
            {
                return MatrixFactory.Init();
            }
        }

        public static DenseMatrix LoadSubMatrixFromFile(string filePath, float minLon, float maxLon, float minLat, float maxLat)
        {
            DenseMatrix dm = LoadMatrixFromFile(filePath);
            return dm.RegionSubMatrix((int)minLon, (int)maxLon, (int)minLat, (int)maxLat);
        }

        static long _opsStarted = 0;
        static long _opsEnded = 0;

        public static void SaveMatrixToFile(DenseMatrix m, string filePath, bool asyncSave)
        {
            if (asyncSave)
            {
                Task.Factory.StartNew(() =>
                {
                    Interlocked.Increment(ref _opsStarted);
                    DoSaveMatrixToFile(m, filePath);
                    Interlocked.Increment(ref _opsEnded);
                });
            }
            else
            {
                DoSaveMatrixToFile(m, filePath);
            }
        }

        private static void DoSaveMatrixToFile(DenseMatrix m, string filePath)
        {
            MatrixFile of = new MatrixFile(filePath, false);
            of.Matrix = m;
            of.Save();
        }
       

        public static void WaitForPendingWriteOperations()
        {
            while (true)
            {
                var startedCount = Interlocked.Read(ref _opsStarted);
                var endedCount = Interlocked.Read(ref _opsEnded);

                if (startedCount <= endedCount)
                    break;

                Console.WriteLine($"  -> Waiting to finish {startedCount - endedCount} pending save operations ....");
                Thread.Sleep(100);
            }

            Thread.Sleep(100);
            
            _opsStarted = _opsEnded = 0;
        }

        public static void Save(DenseMatrix m, string title, string type)
        {
            if (type.EndsWith("_MAP") == false)
                type += "_MAP";

            string fileTitle = string.Format("{0}_{1}.thd", type, title);
            string filePath = Path.Combine(SimulationData.DataFolder, fileTitle);

            SaveMatrixToFile(m, filePath, true);
        }

        public static void SaveAsStats(DenseMatrix m, string title, string type, string category)
        {
            string fileTitle = string.Format("{0}_{1}.thd", type, title);
            string fileFolder = Path.Combine(SimulationData.DataFolder, @"stats\" + category);
            if (Directory.Exists(fileFolder) == false)
                Directory.CreateDirectory(fileFolder);

            string filePath = Path.Combine(fileFolder, fileTitle);

            SaveMatrixToFile(m, filePath, true);
        }
    }

}
