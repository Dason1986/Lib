using System;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public struct SolarHoliday : IHoliday, IFormattable, IComparable, IComparable<SolarHoliday>, IEquatable<SolarHoliday>
    {
        public int Month { get; private set; }
        public int Day { get; private set; }

        public string HolidayName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="name"></param>
        public SolarHoliday(int month, int day, string name)
            : this()
        {
            Month = month;
            Day = day;
            HolidayName = name;
        }
        /// <summary>
        /// 轉換成公曆日期
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime ConvertDateTime(int year)
        {
            return new DateTime(year, Month, Day);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return string.Format(HolidayFormat.FormatProvider, format, this);
        }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return HolidayFormat.FormatProvider.Format(format, this, formatProvider);

        }


        #region Compare


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <(SolarHoliday t1, SolarHoliday t2)
        {

            return t1.Month * 100 + t1.Day < t2.Month * 100 + t2.Day;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(SolarHoliday t1, SolarHoliday t2)
        {
            return t1.Month * 100 + t1.Day <= t2.Month * 100 + t2.Day;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator ==(SolarHoliday t1, SolarHoliday t2)
        {
            return t1.Day == t2.Day && t1.Month == t2.Month;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator !=(SolarHoliday t1, SolarHoliday t2)
        {
            return t1.Day != t2.Day || t1.Month != t2.Month;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(SolarHoliday t1, SolarHoliday t2)
        {
            return t1.Month * 100 + t1.Day > t2.Month * 100 + t2.Day;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(SolarHoliday t1, SolarHoliday t2)
        {
            return t1.Month * 100 + t1.Day >= t2.Month * 100 + t2.Day;
        }


        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is SolarHoliday == false) throw new ChineseDateTimeException();
            return CompareTo((SolarHoliday)obj);
        }

        public int CompareTo(SolarHoliday other)
        {
            var x = this.Month * 100 + this.Day;
            var y = other.Month * 100 + other.Day;
            if (x < y) return -1;
            if (x > y) return 1;
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int CompareTo(IHoliday other, int year)
        {
            if (other is SolarHoliday) return CompareTo((SolarHoliday)other);
            var x = this.ConvertDateTime(year);
            var y = other.ConvertDateTime(year);
            return y.CompareTo(y);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool Equals(SolarHoliday t1, SolarHoliday t2)
        {
            return t1.Equals(t2);
        }
        public bool Equals(SolarHoliday other)
        {
            return CompareTo(other) == 0;
        }
        #endregion
    }
}