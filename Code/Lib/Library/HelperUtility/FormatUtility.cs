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
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatddMMyyyy(object datetime)
        {
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DateFormatddMMyyyyFull(object datetime)
        {
            return string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", datetime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateFormatyyyyMMddFull(object datetime)
        {
            return string.Format("{0:yyyy-MM-dd hh:mm:ss tt}", datetime);
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
        public static string TimeFormat(object datetime)
        {
            return string.Format("{0:HH:mm}", datetime);
        }

    }
}
