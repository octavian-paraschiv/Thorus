using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThorusCommon.Data;

namespace ThorusCommon.Engine
{
    public class SimDateTime : IComparable
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }
        public int Hour { get; private set; }

        public int DayOfYear
        {
            get
            {
                DateTime dt = new DateTime(Year, Month, Day);
                return dt.DayOfYear;
            }
        }

        public string Title
        {
            get
            {
                return string.Format("{0:d4}-{1:d2}-{2:d2}_{3:d2}",
                    Year, Month, Day, Hour);
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public int CompareTo(object obj)
        {
            SimDateTime sdt = obj as SimDateTime;
            if (sdt == null)
                return 1;

            string s1 = this.ToString();
            string s2 = sdt.ToString();
            return string.Compare(s1, s2, true);
        }

        public override string ToString()
        {
            return string.Format("{0:d4}-{1:d2}-{2:d2} {3:d2}:00",
                Year, Month, Day, Hour);
        }


        public SimDateTime AddHours(int hours)
        {
            DateTime dt = new DateTime(Year, Month, Day, Hour, 0, 0);
            DateTime dt2 = dt.AddHours(hours);

            SimDateTime sdt = new SimDateTime();
            sdt.Year = dt2.Year;
            sdt.Month = dt2.Month;
            sdt.Day = dt2.Day;
            sdt.Hour = dt2.Hour;

            return sdt;
        }

        public int GetHoursOffset(SimDateTime sdt)
        {
            DateTime dt1 = new DateTime(Year, Month, Day, Hour, 0, 0);
            DateTime dt2 = new DateTime(sdt.Year, sdt.Month, sdt.Day, sdt.Hour, 0, 0);
            float h = (float)dt1.Subtract(dt2).TotalHours;

            if (h - Math.Floor(h) >= 0.5f)
                return (int)(h) + 1;

            return (int)h;
        }

        public static SimDateTime FromFileName(string fileName)
        {
            SimDateTime sdt = null;

            try
            {
                int idx = fileName.IndexOf("MAP_");
                if (idx > 0)
                {
                    string dateTimePart = fileName.Substring(idx + 4);
                    sdt = new SimDateTime(dateTimePart);
                }
            }
            catch { }

            return sdt;
        }

        public SimDateTime(DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            Hour = dateTime.Hour;
        }


        private SimDateTime()
        {
            Year = Month = Day = Hour = -1;
        }

        public SimDateTime(string s)
            : this()
        {
            if (string.IsNullOrEmpty(s))
                throw new FormatException("SimDateTime: Input string must have a non-null and non-empty value.");
            string[] fields = s.Split(new char[] { '-', '_' });
            if (fields.Length != 4)
                throw new FormatException(
                    string.Format("SimDateTime: Input string is not properly formatted: {0}", s));

            int val = 0;
            int i = 0;

            if (int.TryParse(fields[i++], out val) == false)
                throw new FormatException(
                    string.Format("SimDateTime: Year field does not have a correct value: {0}", s));

            this.Year = val;

            if (int.TryParse(fields[i++], out val) == false)
                throw new FormatException(
                    string.Format("SimDateTime: Month field does not have a correct value: {0}", s));

            this.Month = val;

            if (int.TryParse(fields[i++], out val) == false)
                throw new FormatException(
                    string.Format("SimDateTime: Day field does not have a correct value: {0}", s));

            this.Day = val;

            if (int.TryParse(fields[i++], out val) == false)
                throw new FormatException(
                    string.Format("SimDateTime: Hour field does not have a correct value: {0}", s));

            this.Hour = val;
        }
    }
}
