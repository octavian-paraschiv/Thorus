using System;
using System.Diagnostics;

namespace ThorusCommon
{
    public class TicToc : IDisposable
    {
        private DateTime _tic = DateTime.MinValue;

        public TicToc()
        {
            Tic();
        }

        public void Tic()
        {
            _tic = DateTime.Now;
        }

        public void Toc()
        {
            TocInternal();
            Tic();
        }

        public void TocInternal()
        {
            DateTime tm = DateTime.Now;
            TimeSpan diff = tm.Subtract(_tic);

            Trace.WriteLine(string.Format("    => Elapsed time is {0} seconds", diff.TotalSeconds));
        }

        public void Dispose()
        {
            TocInternal();
        }
    }
}
