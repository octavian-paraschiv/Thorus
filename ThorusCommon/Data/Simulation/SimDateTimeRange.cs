using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThorusCommon.Thermodynamics;
using ThorusCommon.Data;

namespace ThorusCommon.Engine
{
    public class SimDateTimeRange
    {
        public List<Atmosphere> AtmList { get; set; }
        public List<SurfaceLevel> SfcList { get; set; }

        public SimDateTime Start { get; set; }
        public SimDateTime End { get; set; }

        public EarthModel Earth { get; private set; }

        public int ID { get; set; }

        public override string ToString()
        {
            return string.Format("[ID: {0}, Start: {1}, End: {2}]", ID, Start, End);
        }

        public SimDateTimeRange(EarthModel earth, SimDateTime dtStart, SimDateTime dtEnd)
        {
            this.Start = dtStart;
            this.End = dtEnd;
            this.Earth = earth;

            AtmList = new List<Atmosphere>();
            SfcList = new List<SurfaceLevel>();

            for (SimDateTime sdt = dtStart; sdt.CompareTo(dtEnd) <= 0; sdt = sdt.AddHours(earth.SnapshotLength))
            {
                Atmosphere atm = null;
                SurfaceLevel sfc = null;

                try
                {
                    earth.SetUTC(sdt);
                    atm = new Atmosphere(earth, true);
                    sfc = new SurfaceLevel(earth, true);
                }
                catch
                {
                    atm = null;
                    sfc = null;
                }

                if (atm != null)
                    AtmList.Add(atm);
                if (sfc != null)
                    SfcList.Add(sfc);
            }
        }

        public static SimDateTimeRange BuildRange(SimDateTime dtStart, int rangeSize)
        {
            EarthModel earth = new EarthModel(dtStart, true, (int)AbsoluteConstants.HoursPerDay);

            if (SimulationData.AvailableSnapshots.Contains(dtStart))
            {
                SimDateTime sdt = dtStart.AddHours((rangeSize - 1) * earth.SnapshotLength);
                SimDateTime dtEnd = SimulationData.SelectNearestSnapshot(sdt);

                if (dtEnd != null)
                    return new SimDateTimeRange(earth, dtStart, dtEnd);
            }

            return null;
        }

        public SimDateTimeRangeStats BuildStatistics()
        {
            SimDateTimeRangeStats stats = new SimDateTimeRangeStats(this.Earth);

            int count = AtmList.Count;
            for (int i = 0; i < count; i++)
            {
                stats.ProcessAtmSnapshot(AtmList[i]);
                stats.ProcessSfcSnapshot(SfcList[i]);
            }

            stats.AdjustMeanValues(count);
            return stats;
        }
    }
}
