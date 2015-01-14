using System;
using System.Linq;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public struct LunarHoliday : IHoliday, IFormattable
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
            ChineseDateTime date = new ChineseDateTime(year, Month, Day, false);
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
    }
}