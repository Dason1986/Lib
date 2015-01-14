using System;

namespace Library.Date
{
    /// <summary>
    /// 
    /// </summary>
    public struct WeekHoliday : IHoliday, IFormattable
    {
        /// <summary>
        /// �·�
        /// </summary>
        public int Month { get; private set; }
        /// <summary>
        /// �ڎׂ�����
        /// </summary>
        public int WeekAtMonth { get; private set; }
        /// <summary>
        /// ����
        /// </summary>
        public DayOfWeek WeekDay { get; private set; }
        /// <summary>
        /// �������Q
        /// </summary>
        public string HolidayName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="weekAtMonth">0�������һ������</param>
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
        /// �D�Q�ɹ�������
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
            int firWeekDays = 8 - firstweekday; //�����һ��ʣ������
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


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return HolidayFormat.FormatProvider.Format(format, this, formatProvider);

        }
    }
}