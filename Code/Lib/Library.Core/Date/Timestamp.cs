using System;

namespace Library.Date
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public struct Timestamp
    {
#if !SILVERLIGHT
        static readonly DateTime UnixTpStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
#else
          static readonly DateTime UnixTpStart = new DateTime(1970, 1, 1) ;
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToUtp(DateTime dt)
        {
            TimeSpan toNow = dt - UnixTpStart;
            return (long)Math.Round(toNow.TotalSeconds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static long ToUtp(int year, int month, int day)
        {
            return ToUtp(new DateTime(year, month, day));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static long ToUtp(int year, int month, int day, int hour, int minute, int second)
        {
            return ToUtp(new DateTime(year, month, day, hour, minute, second));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static DateTime FromUtp(long tp)
        {
            return UnixTpStart + (new TimeSpan(tp * 10000000));
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        public static long Now { get { return ToUtp(DateTime.Now); } }


    }
}
