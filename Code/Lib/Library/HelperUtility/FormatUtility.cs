using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Library.Att;
using Library.Date;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public class FormatUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static TryResult<DateTime> GetDateTimeddMMyyyy(string datestr)
        {

            try
            {
                return DateTime.ParseExact(datestr, "dd/MM/yyyy", null);
            }
            catch (Exception e)
            {

                return e;
            }

        }
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

                return e;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatddMMyyyy(DateTime datetime)
        {
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DateFormatddMMyyyyhhmmssttFull(DateTime datetime)
        {
            return string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", datetime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatyyyyMMdd(DateTime datetime)
        {
            return string.Format("{0:yyyy-MM-dd}", datetime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatHHmm(DateTime datetime)
        {
            return string.Format("{0:HH:mm}", datetime);
        }
    
        /// <summary>
        /// 時間段
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTimePeriod DateFormatPeriod(DateTime datetime)
        {
            var now = DateTime.Now;

            TimeSpan ts = now - datetime;

            int d = ts.Days;
            if (d < 0) return DateTimePeriod.Future;

            switch (d)
            {
                case 0: return DateTimePeriod.Today;
                case 1: return DateTimePeriod.Yesterday;
                default:
                    {
                        var weekstart = 7- datetime.DayOfWeek.GetHashCode();
                       
                        if (d < weekstart) return DateTimePeriod.ThisWeek;//in this week

                        if (d < now.Day)//in this month
                        {
                            d = d - weekstart;
                            if (d < 7) return DateTimePeriod.LastWeek;
                            return DateTimePeriod.ThisMonth;
                        }
                        var lastmonth = now.AddMonths(-1);
                        lastmonth = new DateTime(lastmonth.Year, lastmonth.Month, 1);

                        if (lastmonth < datetime) return DateTimePeriod.LastMonth;
                        if (now.Year == datetime.Year) return DateTimePeriod.ThisYear;
                        return DateTimePeriod.Older;
                    }
            }

        }

        /// <summary>
        /// 時間段
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatChinese(DateTime datetime)
        {
            var now = DateTime.Now;

            TimeSpan ts = now - datetime;


            if (ts.TotalMinutes < 3) return "幾分鐘前";//"A few minutes ago";

            int d = ts.Days;
            if (d == 0) return string.Format("{0:HH:mm}", datetime);
            if (now.Year == datetime.Year) return string.Format("{0:MM-dd}({1}){0:HH:mm}", datetime, Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(datetime.DayOfWeek));
            return string.Format("{0:yyyy-MM-dd}({1}){0:HH:mm}", datetime, Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(datetime.DayOfWeek));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatddMMyyyy(DateTime? datetime)
        {
            return datetime == null ? string.Empty : DateFormatddMMyyyy(datetime.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DateFormatddMMyyyyhhmmssttFull(DateTime? datetime)
        {
            return datetime == null ? string.Empty : DateFormatddMMyyyyhhmmssttFull(datetime.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatyyyyMMdd(DateTime? datetime)
        {
            return datetime == null ? string.Empty : DateFormatyyyyMMdd(datetime.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatHHmm(DateTime? datetime)
        {
            return datetime == null ? string.Empty : DateFormatHHmm(datetime.Value);
        }


    }
}
