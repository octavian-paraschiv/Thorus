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
        SimDateTime _baselineStart = null;

        Simulation _baselineSim = null;
        List<Simulation> _branchedSims = new List<Simulation>();
        List<ManualResetEvent> _doneEvents = new List<ManualResetEvent>();

        int _baselineLength = 0;
        int _totalDays = 0;
        int _snapshotLength = 0;

        public SimulationEngine(SimDateTime sdtStart, int totalDays, int nofSnapshots)
        {
            SimDateTime sdtSimEnd = sdtStart.AddHours((int)(AbsoluteConstants.HoursPerDay * totalDays));

            Console.WriteLine(string.Format("Simulation params: starting at: {0}, ending at: {1}, total days: {2} ...", 
                sdtStart, sdtSimEnd, totalDays));

            // The length of the baseline is equal to the total number of days.
            _baselineLength = totalDays;

            _snapshotLength = (int)(AbsoluteConstants.HoursPerDay / nofSnapshots);

            Console.WriteLine(string.Format("  -> snapshotLength: {0} hrs, baselineLength: {1} days...", 
                _snapshotLength, _baselineLength));

            Console.WriteLine("********");
            Console.WriteLine("Using parameters:");
            Console.Write(SimulationParameters.Instance.ToString());
            Console.WriteLine("********");

            _baselineStart = sdtStart;
            _totalDays = totalDays;
            
            SimDateTime sdtBaselineEnd = _baselineStart.AddHours((int)(AbsoluteConstants.HoursPerDay * _baselineLength));

            _baselineSim = new Simulation(sdtStart, sdtBaselineEnd, _snapshotLength, false);
            _baselineSim.ID = 0;
        }

        public void Run(DateTime dtInit)
        {
            Console.WriteLine("  -> Running baseline...");
            if (_baselineSim != null)
                _baselineSim.Run(dtInit);

            GC.Collect();

            FileSupport.WaitForPendingWriteOperations();

            TimeSpan tsDiff = DateTime.Now - dtInit;
            Console.WriteLine($"  -> Simulation completed after {(int)tsDiff.TotalMilliseconds} msec.");
        }
    }
}
