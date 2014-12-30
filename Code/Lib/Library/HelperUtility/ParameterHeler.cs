namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParameterHeler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetValue<T>(this IParameter parameter, T defaultValue)
        {
            if (parameter == null || string.IsNullOrWhiteSpace(parameter.Value)) return defaultValue;
            var result = StringUtility.TryCast(parameter.Value, defaultValue);
            return result.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetValue<T>(this IParameter parameter)
        {
            return GetValue(parameter, default(T));
        }
    }
}