using Library.Comparable;
using Library.Date;
using System;
using System.Data.SqlTypes;
using System.Threading;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class DateTimeUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static TryResult<DateTime> GetDate(string datestr)
        {
            try
            {
                return DateTime.ParseExact(datestr, formats, null, System.Globalization.DateTimeStyles.AssumeLocal);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.GetAllExceptionInfo());
                return e;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static TryResult<DateTime> GetDateddMMyyyy(string datestr)
        {
            try
            {
                return DateTime.ParseExact(datestr, "dd/MM/yyyy", null);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.GetAllExceptionInfo());
                return e;
            }
        }

        private static string[] formats = { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" };

        /// <summary>
        ///
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static TryResult<DateTime> GetDateyyyyMMdd(string datestr)
        {
            try
            {
                return DateTime.ParseExact(datestr, "yyyyMMdd", null);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.GetAllExceptionInfo());
                return e;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatddMMyyyy(this DateTime datetime)
        {
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }

        /// <summary>
        ///
        /// </summary>
        public static string FormatddMMyyyyhhmmssttFull(this DateTime datetime)
        {
            return string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", datetime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatyyyyMMdd(this DateTime datetime)
        {
            return string.Format("{0:yyyy-MM-dd}", datetime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatHHmm(this DateTime datetime)
        {
            return string.Format("{0:HH:mm}", datetime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static RangeItem<DateTime> GetDateTimeRange(this DateTimePeriod period)
        {
            switch (period)
            {
                case DateTimePeriod.NextMonth:
                    {
                        var nextmonth = DateTime.Today.AddMonths(1);

                        return new RangeItem<DateTime>(new DateTime(nextmonth.Year, nextmonth.Month, 1), new DateTime(nextmonth.Year, nextmonth.Month, DateTime.DaysInMonth(nextmonth.Year, nextmonth.Month), 23, 59, 59));
                    }
                case DateTimePeriod.NextWeek:
                    {
                        var week = DateTime.Now.DayOfWeek;
                        var weekday = DateTime.Now.AddDays((int)week);
                        var endday = weekday.AddDays(7);
                        return new RangeItem<DateTime>(weekday.Date, new DateTime(endday.Year, endday.Month, endday.Day, 23, 59, 59));
                    }
                case DateTimePeriod.TheDayAfterTomorrow:
                    {
                        var tomorrow = DateTime.Today.AddDays(2).Date;
                        return new RangeItem<DateTime>(tomorrow, new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 59, 59));
                    }
                case DateTimePeriod.Tomorrow:
                    {
                        var tomorrow = DateTime.Today.AddDays(1).Date;
                        return new RangeItem<DateTime>(tomorrow, new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 23, 59, 59));
                    }
                case DateTimePeriod.Today:
                    {
                        var today = DateTime.Today;
                        return new RangeItem<DateTime>(today, new DateTime(today.Year, today.Month, today.Day, 23, 59, 59));
                    }
                case DateTimePeriod.Yesterday:
                    {
                        var yesterday = DateTime.Today.AddSeconds(-1);
                        var begin = yesterday.Date;
                        return new RangeItem<DateTime>(begin, yesterday);
                    }
                case DateTimePeriod.ThisWeek:
                    {
                        var today = DateTime.Today;
                        var startday = today.AddDays(-(int)today.DayOfWeek);
                        var endday = startday.AddDays(7);
                        return new RangeItem<DateTime>(startday, new DateTime(endday.Year, endday.Month, endday.Day, 23, 59, 59));
                    }
                case DateTimePeriod.LastWeek:
                    {
                        var today = DateTime.Today;
                        var endday = today.AddDays(-(int)today.DayOfWeek).AddSeconds(-1);
                        var startday = endday.AddDays(-6);
                        return new RangeItem<DateTime>(startday.Date, endday);
                    }
                case DateTimePeriod.ThisMonth:
                    {
                        var today = DateTime.Today;
                        return new RangeItem<DateTime>(new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month), 23, 59, 59));
                    }
                case DateTimePeriod.LastMonth:
                    {
                        var lastMonth = DateTime.Today.Date.AddMonths(-1);

                        return new RangeItem<DateTime>(new DateTime(lastMonth.Year, lastMonth.Month, 1), new DateTime(lastMonth.Year, lastMonth.Month, DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month), 23, 59, 59));
                    }
                case DateTimePeriod.ThisYear:
                    {
                        var today = DateTime.Today;
                        return new RangeItem<DateTime>(new DateTime(today.Year, 1, 1), new DateTime(today.Year, 12, 31, 23, 59, 59));
                    }
                case DateTimePeriod.Earlier:
                    {
                        var today = DateTime.Today;
                        return new RangeItem<DateTime>(SqlDateTime.MinValue.Value, new DateTime(today.Year - 1, 12, 31, 23, 59, 59));
                    }
            }

            return RangeItem<DateTime>.Empty;
        }

        /// <summary>
        /// 輸入時間與當前時間的關係
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTimePeriod GetPeriod(this DateTime datetime)
        {
            var now = DateTime.Now;

            TimeSpan ts = now - datetime;

            int d = ts.Days;
            bool isabs = false;
            if (ts.TotalDays < 0)
            {
                d = Math.Abs((int)Math.Ceiling(ts.TotalDays));
                d++;
                isabs = true;
            }
            switch (d)
            {
                case 0: return DateTimePeriod.Today;
                case 1: return isabs ? DateTimePeriod.Tomorrow : DateTimePeriod.Yesterday;
                case 2: return isabs ? DateTimePeriod.TheDayAfterTomorrow : DateTimePeriod.TheDayBeforeYesterday;
                default:
                    {
                        var weekstart = now.Day - now.DayOfWeek.GetHashCode();
                        var weekend = weekstart + 7;
                        if (datetime.Day.IsBetween(weekstart, weekend)) return DateTimePeriod.ThisWeek;//in this week
                        if (datetime.Day.IsBetween(weekstart + 7, weekend + 7)) return isabs ? DateTimePeriod.NextWeek : DateTimePeriod.LastWeek;
                        if (now.Month == datetime.Month)
                            return DateTimePeriod.ThisMonth;
                        var lastmonth = now.AddMonths(-1);
                        lastmonth = new DateTime(lastmonth.Year, lastmonth.Month, 1);

                        if (lastmonth < datetime) return isabs ? DateTimePeriod.NextMonth : DateTimePeriod.LastMonth;
                        if (now.Year == datetime.Year) return DateTimePeriod.ThisYear;
                        return isabs ? DateTimePeriod.After : DateTimePeriod.Earlier;
                    }
            }
        }

        /// <summary>
        /// 获取中文间隔时间差
        /// </summary>
        /// <param name="time"></param>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        public static string TimeSpanChinese(this DateTime time, DateTime? nowTime)
        {
            var now = nowTime.HasValue ? nowTime.Value : DateTime.Now;
            var span = now.Subtract(time);
            var day = 60 * 24;//天
            var hour = 60;
            if (span.Minutes >= day * 4)
            {
                return string.Format("{0}年{1}月{2}日", time.Year, time.Month, time.Day);
            }
            else if (span.Minutes >= day * 3 && span.Minutes < day * 4)
            {
                return string.Format("{0}天前", span.Days);
            }
            else if (span.Minutes >= day * 2 && span.Minutes < day * 3)
            {
                return string.Format("{0}天前", span.Days);
            }
            else if (span.Minutes > day && span.Minutes < day * 2)
            {
                return string.Format("{0}天前", span.Days);
            }
            else if (span.Minutes < day && span.Minutes >= hour)
            {
                return string.Format("{0}小时前", span.Minutes % 60);
            }
            else if (span.Minutes < hour && span.Minutes >= 1)
            {
                return string.Format("{0}分钟前", span.Minutes);
            }
            else
            {
                return "刚刚";
            }
        }
        /// <summary>
        /// 日期關係輸出
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatPeriodText(this DateTime datetime)
        {
            var now = DateTime.Now;

            TimeSpan ts = now - datetime;

            if (ts.TotalMinutes.IsBetween(0, 5)) return "A few minutes ago";//"A few minutes ago";

            int d = ts.Days;
            if ((int)Math.Ceiling(ts.TotalDays) == 1) return string.Format("{0:HH:mm}", datetime);
            if (now.Year == datetime.Year) return string.Format("{0:MM-dd}({1}){0:HH:mm}", datetime, Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(datetime.DayOfWeek));
            return string.Format("{0:yyyy-MM-dd}({1}){0:HH:mm}", datetime, Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(datetime.DayOfWeek));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatddMMyyyy(this DateTime? datetime)
        {
            return datetime == null ? string.Empty : FormatddMMyyyy(datetime.Value);
        }

        /// <summary>
        ///
        /// </summary>
        public static string FormatddMMyyyyhhmmssttFull(this DateTime? datetime)
        {
            return datetime == null ? string.Empty : FormatddMMyyyyhhmmssttFull(datetime.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatyyyyMMdd(this DateTime? datetime)
        {
            return datetime == null ? string.Empty : FormatyyyyMMdd(datetime.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatHHmm(this DateTime? datetime)
        {
            return datetime == null ? string.Empty : FormatHHmm(datetime.Value);
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long TimeStamp(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 通過时间戳獲取日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime FromTimeStamp(long timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
          
            return startTime.AddSeconds(timeStamp);

        }
    }
}