using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Data;
using ThorusCommon.Engine;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;
using System.Threading;
using System.Diagnostics;
using ThorusCommon;

namespace ThorusSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dtInit = DateTime.Now;

            // -------------------------
            // Parse command line args


            bool runStats = false;
            int statRangeLen = 1;

            if (args.Length < 2)
                return;

            int argCnt = 0;

            SimDateTime sdtStart = null;
            SimDateTime sdtEnd = null;
            
            int totalDays = 0;
            int totalDaysArg = 0;
            int nofSnapshots = 3;


            int totalExpectedArgs = 4;
            bool runSim = true;
            bool regenerateInitialConditions = Environment.CommandLine.EndsWith(" regen");

            if (string.Compare(args[argCnt], "nosim", true) != 0)
            {
                if (string.Compare(args[argCnt], "0", true) == 0)
                {
                    sdtStart = null;
                    argCnt++;
                }
                else
                {
                    try
                    {
                        sdtStart = new SimDateTime(args[argCnt++]);
                    }
                    catch
                    {
                        sdtStart = null;
                    }
                }

                string arg = args[argCnt++];

                try
                {
                    sdtEnd = new SimDateTime(arg);
                    totalDaysArg = 0;
                }
                catch
                {
                    totalDaysArg = int.Parse(arg);
                }

                // TODO nofSnapshots as part of cmd line
                nofSnapshots = 1;
            }
            else
            {
                argCnt++;
                totalExpectedArgs = 3;
                runSim = false;
            }
             
            if (args.Length >= totalExpectedArgs)
            {
                runStats = string.Compare(args[argCnt++], "stat", true) == 0;
                statRangeLen = int.Parse(args[argCnt++]);
            }
            // -------------------------

            // This is just for loading the customizable simulation params
            var x = SimulationParameters.Instance.JetStreamPeriod;

            // -------------------------
            // Run simulation engine
            if (runSim)
            {
                if (regenerateInitialConditions)
                {
                    // First Grib, then NetCdf. Otherwise it crashes and I don't know why...
                    new GribImporter("input.grib").ImportFiles();
                    new NetCdfImporter(true).ImportFiles();
                }

                string dataDir = SimulationData.DataFolder;
                if (Directory.Exists(dataDir))
                    Directory.Delete(dataDir, true);

                Directory.CreateDirectory(dataDir);

                try
                {
                    string timeSeedFile = "timeSeed.thd";
                    NetCdfImporter.CorrectFilePath(ref timeSeedFile);
                    
                    if (sdtStart == null && File.Exists(timeSeedFile))
                    {
                        try
                        {
                            string timeSeed = File.ReadAllText(timeSeedFile);
                            sdtStart = new SimDateTime(timeSeed);
                        }
                        catch
                        {
                            sdtStart = null;
                        }
                    }

                    if (sdtStart == null)
                        sdtStart = new SimDateTime(DateTime.Now);

                    if (totalDaysArg == 0)
                    {
                        if (sdtEnd == null)
                            sdtEnd = new SimDateTime(DateTime.Now);

                        totalDays = (int)Math.Round(sdtEnd.GetHoursOffset(sdtStart) / AbsoluteConstants.HoursPerDay);
                    }
                    else
                        totalDays = totalDaysArg;

                    SimulationEngine sim = new SimulationEngine(sdtStart, totalDays, nofSnapshots);
                    sim.Run(dtInit);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            // -------------------------

            GC.Collect();

            // -------------------------
            // Run statistics engine
            if (runStats)
            {
                string statDir = Path.Combine(SimulationData.DataFolder, "stats");
                if (Directory.Exists(statDir))
                    Directory.Delete(statDir, true);

                Directory.CreateDirectory(statDir);

                try
                {
                    StatisticsEngine stat = new StatisticsEngine(statRangeLen);
                    stat.Run(dtInit);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            // -------------------------

            Process.GetCurrentProcess().Kill();
        }
    }
}
