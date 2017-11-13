using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

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
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static T Cast<T>(this IParameter parameter) where T : IConvertible
        {
            return Cast<T>(parameter, default(T));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static NameValueCollection CastNameValueCollection(this IEnumerable<IParameter> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            NameValueCollection valueCollection = new NameValueCollection();
            foreach (var parameter in parameters)
            {
                valueCollection.Add(parameter.Key, parameter.GetValue());
            }
            return valueCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] CastArray<T>(this IParameter parameter, char split) where T : IConvertible
        {
            if (parameter == null || string.IsNullOrEmpty(parameter.Value)) return new T[0];
            var arr = parameter.Value.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            T[] tarry = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                tarry[i] = ObjectUtility.Cast<T>(arr[i]);
            }
            return tarry;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Cast<T>(this IParameter parameter, T defaultValue) where T : IConvertible
        {

            if (parameter == null || string.IsNullOrEmpty(parameter.Value)) return defaultValue;

            return ObjectUtility.TryCast<T>(parameter.Value, defaultValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Cast<T>(this IEnumerable<IParameter> parameters, string key, T defaultValue) where T : IConvertible
        {

            if (parameters == null || parameters.Count() == 0 || string.IsNullOrEmpty(key)) return defaultValue;
            var par = parameters.FirstOrDefault(n => n.Key == key);

            return par.Cast(defaultValue);
        }
    }
}