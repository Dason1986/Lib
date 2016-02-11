namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class NullableHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetValue<T>(this T? t) where T : struct
        {
            return GetValue(t, default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetValue<T>(this T? t, T defaultValue) where T : struct
        {
            return t == null ? defaultValue : t.Value;
        }
    }
}