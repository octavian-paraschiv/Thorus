using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThorusCommon.Utility
{
    public static class Utils
    {
        public static float BP_to_FP(float bp)
        {
            var raw_fp = 1 - 0.5f * bp;

            // Bounds check (must be between 0  and 1)
            return Math.Min(1f, Math.Max(0f, raw_fp));
        }
    }
}
