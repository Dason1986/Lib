using System;

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
        public static string DateFormatddMMyyyy(object datetime)
        {
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DateFormatddMMyyyyhhmmssttFull(object datetime)
        {
            return string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", datetime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatyyyyMMdd(object datetime)
        {
            return string.Format("{0:yyyy-MM-dd}", datetime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatHHmm(object datetime)
        {
            return string.Format("{0:HH:mm}", datetime);
        }

    }
}
