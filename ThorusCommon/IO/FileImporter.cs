using System;
using System.IO;
using ThorusCommon.Data;
using ThorusCommon.Engine;

namespace ThorusCommon.IO
{
    public abstract class FileImporter
    {
        protected string TemperatureFileLow = "T00.thd";
        protected string TemperatureFileMid = "T01.thd";
        protected string TemperatureFileTop = "T02.thd";

        protected string GeopotentialFileLow = "Z00.thd";
        protected string GeopotentialFileMid = "Z01.thd";
        protected string GeopotentialFileTop = "Z02.thd";

        protected string PressureFileLow = "P00.thd";
        protected string PressureFileMid = "P01.thd";
        protected string PressureFileTop = "P02.thd";

        protected string HumidityFileLow = "H00.thd";
        protected string HumidityFileMid = "H01.thd";
        protected string HumidityFileTop = "H02.thd";

        protected string SeaTempFile = "SST.thd";
        protected string SoilTempFile = "SOIL.thd";

        protected string SnowCoverFile = "SNOW.thd";

        protected int EW = SurfaceLevel.GridColumnCount;
        protected int EH = SurfaceLevel.GridRowCount;

        const int TotalLevels = 3;
        protected string[] TempFiles = new string[TotalLevels];
        protected string[] GeopotentialFiles = new string[TotalLevels];
        protected string[] HumidityFiles = new string[TotalLevels];
        protected string[] PressureFiles = new string[TotalLevels];

        protected FileImporter()
        {
            // ----------------------------------------------
            // Surface files
            CorrectFilePath(ref SeaTempFile);
            CorrectFilePath(ref SoilTempFile);
            CorrectFilePath(ref SnowCoverFile);

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
            filePath = Path.Combine(SimulationData.WorkFolder, filePath);
        }

        public void ImportFiles()
        {
            try
            {
                Init();

                ImportSurface();
                ImportLevel(0); // low level    
                ImportLevel(1); // mid level
                ImportLevel(2); // top level
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Cleanup();
            }
        }

        protected abstract void Init();
        protected abstract void Cleanup();

        protected abstract void ImportSurface();
        protected abstract void ImportLevel(int idx);
    }
}
