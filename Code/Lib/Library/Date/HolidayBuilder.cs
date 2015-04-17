using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class HolidayBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly List<IHoliday> Holidays = new List<IHoliday>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="holiday"></param>
        public void AddHolidy(IHoliday holiday)
        {
            Holidays.Add(holiday);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public void AddRangeHolidy(IEnumerable<IHoliday> collection)
        {
            Holidays.AddRange(collection);
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract void Create();
    }
}
