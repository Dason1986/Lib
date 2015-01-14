using System;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHoliday : IFormattable
    {
        /// <summary>
        /// 月
        /// </summary>
        int Month { get; }
        /// <summary>
        /// 日
        /// </summary>
        int Day { get; }

        /// <summary>
        /// 節日名稱
        /// </summary>
        string HolidayName { get; }

        /// <summary>
        /// 轉換成公曆日期
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime ConvertDateTime(int year);
    }
}