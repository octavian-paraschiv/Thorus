using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using ThorusCommon.Engine;
using ThorusCommon.MatrixExtensions;
using ThorusCommon.Thermodynamics;
using ThorusCommon.IO;

namespace ThorusCommon.Data
{
    public class EarthModel
    {
        public const int MaxLat = 89;
        public const int MinLat = -89;
        public const int MaxLon = 179;
        public const int MinLon = -180;

        public Atmosphere ATM { get; private set; }
        public SurfaceLevel SFC { get; private set; }

        public SimDateTime SimStartUTC { get; private set; }
        public SimDateTime UTC { get; private set; }

        int _snapshotLength = 0;
        float _snapshotDivFactor = 0;


        public int SnapshotLength
        {
            get
            {
                return _snapshotLength;
            }

            set
            {
                _snapshotLength = value;

                bool isValid = false;

                isValid = (_snapshotLength % (int)(AbsoluteConstants.HoursPerDay) == 0) ||
                    ((int)AbsoluteConstants.HoursPerDay % _snapshotLength == 0);

                if (isValid == false)
                    throw new Exception("Invalid SnapshotLength value, Valid values: 2, 3, 4, 6, 8, 12, 24 or an integer multiple of 24");

                _snapshotDivFactor = (value / AbsoluteConstants.HoursPerDay);
            }
        }

        public float SnapshotDivFactor
        {
            get
            {
                return _snapshotDivFactor;
            }
        }

        public int HoursElapsed
        {
            get
            {
                return UTC.GetHoursOffset(SimStartUTC);
            }
        }

        public void SetUTC(SimDateTime utc)
        {
            this.UTC = utc;
        }

        public EarthModel(SimDateTime initUtc, bool loadFromStateFiles, int snaphotLength)
        {
            this.SnapshotLength = snaphotLength;

            SetUTC(initUtc);
            SimStartUTC = initUtc;

            SFC = new SurfaceLevel(this, loadFromStateFiles);
            ATM = new Atmosphere(this, loadFromStateFiles);

            RebuildState();
        }

        public void Advance(SimDateTime newUtc)
        {
            this.SetUTC(newUtc);
            ATM.Advance();
        }

        public void RebuildState()
        {
            ATM.RebuildState();
            SFC.RebuildState();
        }

        public void Save(string title)
        {
            ATM.Save(title);
            SFC.Save(title);
        }
    }
}
