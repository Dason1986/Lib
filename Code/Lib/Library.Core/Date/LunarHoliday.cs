using System;
using System.Linq;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public struct LunarHoliday : IHoliday, IFormattable, IComparable, IComparable<LunarHoliday>, IEquatable<LunarHoliday>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Month { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string HolidayName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="name"></param>
        public LunarHoliday(int month, int day, string name)
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
            var day = Day == 30 ? ChineseDateTime.GetChineseMonthDays(year, Month) :Day;
            ChineseDateTime date = new ChineseDateTime(year, Month, day, false);
            return date.Date;
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
        #region operator
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <(LunarHoliday t1, LunarHoliday t2)
        {

            return t1.Month * 100 + t1.Day < t2.Month * 100 + t2.Day;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(LunarHoliday t1, LunarHoliday t2)
        {
            return t1.Month * 100 + t1.Day <= t2.Month * 100 + t2.Day;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator ==(LunarHoliday t1, LunarHoliday t2)
        {
            return t1.Day == t2.Day && t1.Month == t2.Month;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator !=(LunarHoliday t1, LunarHoliday t2)
        {
            return t1.Day != t2.Day || t1.Month != t2.Month;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(LunarHoliday t1, LunarHoliday t2)
        {
            return t1.Month * 100 + t1.Day > t2.Month * 100 + t2.Day;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(LunarHoliday t1, LunarHoliday t2)
        {
            return t1.Month * 100 + t1.Day >= t2.Month * 100 + t2.Day;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is LunarHoliday == false) throw new ChineseDateTimeException();
            return CompareTo((LunarHoliday)obj);
        }

        public int CompareTo(LunarHoliday other)
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
            if (other is LunarHoliday) return CompareTo((LunarHoliday)other);
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
        public static bool Equals(LunarHoliday t1, LunarHoliday t2)
        {
            return t1.Equals(t2);
        }
        public bool Equals(LunarHoliday other)
        {
            return CompareTo(other) == 0;
        }
        #endregion
    }
}