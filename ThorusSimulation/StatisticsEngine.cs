using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using System.Threading;
using ThorusCommon.Thermodynamics;
using System.Diagnostics;
using ThorusCommon.IO;

namespace ThorusSimulation
{
    public class StatisticsEngine
    {
        int _statsRangeLength = 5;

        List<SimDateTimeRange> _statRanges = new List<SimDateTimeRange>();
        List<ManualResetEvent> _doneEvents = new List<ManualResetEvent>();

        public StatisticsEngine(int statsRangeLength)
        {
            _statsRangeLength = statsRangeLength;
            SimulationData.LookupDataFiles(null);
        }

        public void Run(DateTime dtInit)
        {
            if (SimulationData.AvailableSnapshots.Count > 0)
            {
                int rangeSize = _statsRangeLength;

                int totalRanges = (int)Math.Round((float)SimulationData.AvailableSnapshots.Count / (float)rangeSize);

                SimDateTime firstSnapshotStart = SimulationData.AvailableSnapshots[0];

                for (int id = 0; id < totalRanges; id++)
                {
                    _doneEvents.Add(new ManualResetEvent(false));

                    ThreadPool.QueueUserWorkItem((c) => ProcessSimRange(dtInit, firstSnapshotStart, rangeSize, (int)c), id);
                    //ProcessSimRange(dtInit, firstSnapshotStart, rangeSize, id);
                }

                ManualResetEvent.WaitAll(_doneEvents.ToArray());

                TimeSpan tsDiff = DateTime.Now - dtInit;
                Console.WriteLine($"  -> Statistics completed after {(int)tsDiff.TotalMilliseconds} msec.");
            }

            FileSupport.WaitForPendingWriteOperations();
        }

        private void ProcessSimRange(DateTime dtInit, SimDateTime firstSnapshotStart, int rangeSize, int idx)
        {
            //--------------------------------
            TimeSpan tsDiff = DateTime.Now - dtInit;

            SimDateTime dtStart = firstSnapshotStart.AddHours((int)(AbsoluteConstants.HoursPerDay * rangeSize * (idx)));

            Console.WriteLine(string.Format("STAT: Initializing range {0} + {2}days [elapsed {1} msec]",
                dtStart, (int)tsDiff.TotalMilliseconds, rangeSize));

            SimDateTimeRange sdtr = SimDateTimeRange.BuildRange(dtStart, rangeSize);

            if (sdtr == null)
                return;

            tsDiff = DateTime.Now - dtInit;

            Console.WriteLine(string.Format("STAT: Starting processing on range {0} [elapsed {1} msec]",
                sdtr, (int)tsDiff.TotalMilliseconds));

            var c1 = sdtr.AtmList.Count;
            var c2 = sdtr.SfcList.Count;
            if (c1 <= 0 || c2 <= 0 || c1 != c2)
                return;

            SimDateTimeRangeStats stats = sdtr.BuildStatistics();
            stats.Save(sdtr.Start.Title);

            tsDiff = DateTime.Now - dtInit;

            Console.WriteLine(string.Format("STAT: Done range {0} [elapsed {1} msec]",
                sdtr, (int)tsDiff.TotalMilliseconds));

            _doneEvents[idx].Set();
            //--------------------------------
        }
    }
}
