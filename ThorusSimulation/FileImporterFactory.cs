using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThorusCommon.Data;
using ThorusCommon.IO;

namespace ThorusSimulation
{
    public enum DataSourceType
    {
        NetCdf,
        Grib,
    }

    public class FileImporterFactory
    {
        public static FileImporter CreateImporter(DataSourceType type)
        {
            //switch (type)
            //{
            //    case DataSourceType.NetCdf:
            //        return new NetCdfImporter();

            //    case DataSourceType.Grib:
            //    default:
            //        return new GribImporter();
            //}

            return new NetCdfImporter();
        }
    }
}
