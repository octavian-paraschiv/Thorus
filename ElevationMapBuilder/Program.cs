using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;

namespace ElevationMapBuilder
{
    public class ThreadStartArgs
    {
        public int lat = 0;
        public ManualResetEvent waitEvent = null;
    }

    class Program
    {
        static readonly string urlFmt = @"http://www.gpsvisualizer.com/elevation_data/elev2015.js?coords={0:0.0},{1:0.0}";

        static readonly int[] MinLatStart = new int[] { -89, -29, 31 };

        static int MaxLat = 0, MinLat = 0;

        const int MinLon = 21;
        const int MaxLon = 30;

        static decimal[,] _elevations = null;

        const string DefaultWorkFolder = "C:\\OpmWeather";
        static string WorkFolder = DefaultWorkFolder;

        static List<ManualResetEvent> _waitEvents = new List<ManualResetEvent>();

        private static void SetUpWorkingFolder()
        {
            string wf = DefaultWorkFolder;

            try
            {
                try
                {
                    wf = Environment.GetEnvironmentVariable("OpmWeather", EnvironmentVariableTarget.Machine);
                    WorkFolder = wf;
                }
                catch
                {
                    WorkFolder = DefaultWorkFolder;
                }

                if (Directory.Exists(WorkFolder) == false)
                    Directory.CreateDirectory(WorkFolder);

            }
            catch
            {
                WorkFolder = "c:\\";
            }
        }

        static void Main(string[] args)
        {
            SetUpWorkingFolder();

            int idx = int.Parse(args[0]);

            MinLat = 43;// MinLatStart[idx];
            MaxLat = 48;// Math.Min(89, MinLat + 59);

            _elevations = new decimal[1 + MaxLon - MinLon, 1 + MaxLat - MinLat];

            for (int lat = MaxLat; lat >= MinLat; lat--)
            {
                int evtIdx = _waitEvents.Count;

                ManualResetEvent evt = new ManualResetEvent(false);
                _waitEvents.Add(evt);

                ThreadStartArgs tsa = new ThreadStartArgs();
                tsa.waitEvent = evt;
                tsa.lat = lat;

                ThreadPool.QueueUserWorkItem(ThreadedRead, tsa);
            }

            if (_waitEvents != null)
                ManualResetEvent.WaitAll(_waitEvents.ToArray());

            StringBuilder sb = new StringBuilder();

            for (int lat = MaxLat; lat >= MinLat; lat--)
            {
                string line = "";
                for (int lon = MinLon; lon <= MaxLon; lon++)
                {
                    int i = (lon - MinLon);
                    int j = (MaxLat - lat);

                    line += string.Format("{0:0000},", _elevations[i, j]);
                }
                sb.AppendLine(line.TrimEnd(','));
            }

            string fileName = string.Format("scElevationMap_{0}", idx);

            File.WriteAllText(Path.Combine(WorkFolder, fileName), sb.ToString());

            Console.WriteLine("Done - press any key to exit.");
            Console.Read();
        }

        private static void ThreadedRead(object state)
        {
            ThreadStartArgs tsa = state as ThreadStartArgs;
            if (tsa != null)
            {
                using (WebClient wc = new WebClient())
                {
                    for (int lon = MinLon; lon <= MaxLon; lon++)
                        ReadData(wc, tsa.lat, lon);
                }
                tsa.waitEvent.Set();
            }
        }

        static int _cnt = 0;

        private static void ReadData(WebClient wc, int lat, int lon)
        {
            int i = (lon - MinLon);
            int j = (MaxLat - lat);
            
            decimal elv = ReadElevation(wc, lat, lon);
            _elevations[i, j] = Math.Max(0, elv);

            Console.WriteLine("[{3}] LAT: {0}, LON: {1} => ELV: {2}", lat, lon, _elevations[i, j], _cnt++);
        }

        private static decimal ReadElevation(WebClient wc, decimal lat, decimal lon)
        {
            decimal retVal = 0;

            try
            {
                string webQuery = string.Format(urlFmt, lat, lon);
                string reply = wc.DownloadString(webQuery);
                reply = reply.Replace("LocalElevationCallback", "").Replace("(", "").Replace(");", "");
                string[] fields = reply.Split(',');

                decimal inv_elevation = 0;
                if (decimal.TryParse(fields[0], out inv_elevation) && inv_elevation != 0)
                    retVal = Math.Round(1 / inv_elevation, 0, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            //Console.WriteLine("[{3}] LAT: {0:0.0}, LON: {1:0.0} => ELV: {2:0.0}", lat, lon, retVal, _cnt++);

            return retVal;
        }
    }
}
