using System;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public struct SolarHoliday : IHoliday
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
    }
}