using System;

namespace Library.Date
{
    /// <summary>
    ///
    /// </summary>
    public struct WeekHoliday : IHoliday, IComparable, IComparable<WeekHoliday>, IEquatable<WeekHoliday>
    {
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// 第幾個星期
        /// </summary>
        public int WeekAtMonth { get; private set; }

        /// <summary>
        /// 星期
        /// </summary>
        public DayOfWeek WeekDay { get; private set; }

        /// <summary>
        /// 節日名稱
        /// </summary>
        public string HolidayName { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="month"></param>
        /// <param name="weekAtMonth">0為最後的一個星期</param>
        /// <param name="weekDay"></param>
        /// <param name="name"></param>
        public WeekHoliday(int month, int weekAtMonth, DayOfWeek weekDay, string name)
            : this()
        {
            Month = month;
            WeekAtMonth = weekAtMonth;
            WeekDay = weekDay;
            HolidayName = name;
        }

        /// <summary>
        /// 轉換成公曆日期
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime ConvertDateTime(int year)
        {
            DateTime firstDay = new DateTime(year, Month, 1);
            if (firstDay.DayOfWeek == WeekDay) return firstDay;

            int firstweekday = (int)firstDay.DayOfWeek + 1;
            int weekday = (int)WeekDay + 1;
            var week = WeekAtMonth;
            if (WeekAtMonth == 0)
            {
                week = (DateTime.DaysInMonth(year, Month) + firstweekday) / 7;
            }
            int firWeekDays = 8 - firstweekday; //计算第一周剩余天数
            int day;
            if (firstweekday > weekday)
            {
                day = (week - 1) * 7 + weekday + firWeekDays;
            }
            else
            {
                day = weekday + firWeekDays + (week - 2) * 7;
            }
            return new DateTime(year, Month, day);
        }

        int IHoliday.Day
        {
            get { throw new NotImplementedException(); }
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
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
        public static bool operator <(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.Month * 100 + t1.WeekAtMonth * 7 + t1.WeekDay < t2.Month * 100 + t2.WeekAtMonth * 7 + t2.WeekDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.Month * 100 + t1.WeekAtMonth * 7 + t1.WeekDay <= t2.Month * 100 + t2.WeekAtMonth * 7 + t2.WeekDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator ==(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.WeekAtMonth == t2.WeekAtMonth && t1.Month == t2.Month && t1.WeekDay == t2.WeekDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator !=(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.WeekAtMonth != t2.WeekAtMonth || t1.Month != t2.Month || t1.WeekDay != t2.WeekDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.Month * 100 + t1.WeekAtMonth * 7 + t1.WeekDay > t2.Month * 100 + t2.WeekAtMonth * 7 + t2.WeekDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.Month * 100 + t1.WeekAtMonth * 7 + t1.WeekDay >= t2.Month * 100 + t2.WeekAtMonth * 7 + t2.WeekDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is WeekHoliday == false) throw new ChineseDateTimeException(11002.107);
            return CompareTo((WeekHoliday)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(WeekHoliday other)
        {
            var x = this.Month * 100 + this.WeekAtMonth * 7 + this.WeekDay;
            var y = other.Month * 100 + other.WeekAtMonth * 7 + other.WeekDay;
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
            if (other is WeekHoliday) return CompareTo((WeekHoliday)other);
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
        public static bool Equals(WeekHoliday t1, WeekHoliday t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(WeekHoliday other)
        {
            return CompareTo(other) == 0;
        }   /// <summary>

            ///
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals((WeekHoliday)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion operator
    }
}