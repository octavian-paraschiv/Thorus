using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Data;
using System.Threading;

namespace ThorusSimulation
{
    public class Simulation
    {
        private EarthModel _earth = null;

        public SimDateTime Start { get; set; }
        public SimDateTime End { get; set; }

        public int ID { get; set; }

        public Simulation(SimDateTime sdtStart, SimDateTime sdtEnd, int snaphotLength, bool loadFromStateFiles)
        {
            Start = sdtStart;
            End = sdtEnd;

            _earth = new EarthModel(sdtStart, loadFromStateFiles, snaphotLength);
            _earth.Save(sdtStart.Title);
        }

        public void Run(DateTime dtInit)
        {
            int diff = End.GetHoursOffset(Start);
            int diffStart = diff;

            Console.WriteLine(" SIM: from {0} to {1} with {2} hours step ...", 
                Start, End, _earth.SnapshotLength);

            for (int i = 0; diff > 0; i++)
            {
                DateTime dtInitCurrent = DateTime.Now;

                GC.Collect();

                Start = Start.AddHours(_earth.SnapshotLength);
                diff = End.GetHoursOffset(Start);

                _earth.Advance(Start);
                _earth.RebuildState();
                _earth.Save(Start.Title);

                DateTime now = DateTime.Now;
                TimeSpan tsDiffCurrent = now - dtInitCurrent;
                TimeSpan tsDiff = now - dtInit;

                if (diffStart != diff)
                {
                    Console.WriteLine(string.Format(" SIM: [{2:d2}% done @ {0:d5}h/{1:d5}h] -> {3} [current: {5} msec, total: {4} msec]",
                        (diffStart - diff), 
                        diffStart, 
                        (100 * (diffStart - diff) / diffStart), 
                        Start.Title,
                        (int)tsDiff.TotalMilliseconds,
                        (int)tsDiffCurrent.TotalMilliseconds));
                }
                else
                {
                    Console.WriteLine(string.Format(" SIM: [{2:d2}% done @ {0:d5}h/{1:d5}h] -> {3} [current: {5} msec, total: {4} msec]",
                        (diffStart - diff), 
                        diffStart, 
                        "100", 
                        Start.Title,
                        (int)tsDiff.TotalMilliseconds,
                        (int) tsDiffCurrent.TotalMilliseconds));

                }
            }
        }
    }
}
