using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.Thermodynamics;
using System.Threading;
using ThorusCommon;
using ThorusCommon.IO;

namespace ThorusSimulation
{
    public class SimulationEngine
    {
        SimDateTime _simStart = null;
        Simulation _sim = null;

        int _simLength = 0;
        int _snapshotLength = 0;

        public SimulationEngine(SimDateTime sdtStart, int totalDays, int nofSnapshots)
        {
            SimDateTime sdtSimEnd = sdtStart.AddHours((int)(AbsoluteConstants.HoursPerDay * totalDays));

            Console.WriteLine(string.Format("Simulation params: starting at: {0}, ending at: {1}, total days: {2} ...", 
                sdtStart, sdtSimEnd, totalDays));

            // The length of the baseline is equal to the total number of days.
            _simLength = totalDays;

            _snapshotLength = (int)(AbsoluteConstants.HoursPerDay / nofSnapshots);

            Console.WriteLine(string.Format("  -> snapshot: {0} hrs, total sim length: {1} days...", 
                _snapshotLength, _simLength));

            Console.WriteLine("********");
            Console.WriteLine("Using parameters:");
            Console.Write(SimulationParameters.Instance.ToString());
            Console.WriteLine("********");

            _simStart = sdtStart;
            
            SimDateTime sdtBaselineEnd = _simStart.AddHours((int)(AbsoluteConstants.HoursPerDay * _simLength));

            _sim = new Simulation(sdtStart, sdtBaselineEnd, _snapshotLength, false);
            _sim.ID = 0;
        }

        public void Run(DateTime dtInit)
        {
            Console.WriteLine("  -> Starting simulation ...");

            if (_sim != null)
                _sim.Run(dtInit);

            GC.Collect();

            FileSupport.WaitForPendingWriteOperations();

            TimeSpan tsDiff = DateTime.Now - dtInit;
            Console.WriteLine($"  -> Simulation completed after {(int)tsDiff.TotalMilliseconds} msec.");
        }
    }
}
