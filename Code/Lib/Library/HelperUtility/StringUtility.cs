using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Library.Annotations;

namespace Library.HelperUtility
{

    /// <summary>
    /// 
    /// </summary>
    public class StringUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryResult<T> TryCast<T>(string str)
        {
            try
            {
                return (T)Cast(str, typeof(T));
            }
            catch (Exception ex)
            {

                return new TryResult<T>(default(T), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryResult<T> TryCast<T>(string str, T defaultvalue)
        {
            try
            {
                return (T)Cast(str, typeof(T));
            }
            catch (Exception ex)
            {
                return new TryResult<T>(defaultvalue, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string[] ReomveFirstAndLastChar([NotNull] string[] arr, char ch)
        {
            if (arr == null) throw new ArgumentNullException("arr");
            var newarr = new string[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                var tmp = arr[i];
                if (string.IsNullOrEmpty(tmp))
                {
                    newarr[i] = string.Empty;
                    continue;
                }
                if (tmp[0] == ch) tmp = tmp.Substring(1);
                if (tmp.Length > 0 && tmp[tmp.Length - 1] == ch) tmp = tmp.Substring(0, tmp.Length - 1);
                newarr[i] = tmp;
            }
            return newarr;
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static object Cast(string str, [NotNull] Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException("str");
            var tmpType = targetType.RemoveNullabl();

            if (typeof(Guid) == tmpType && str.Length == 36) return Guid.Parse(str);

            if (tmpType.IsEnum) return Enum.Parse(tmpType, str);
            if (targetType == typeof(bool))
            {
                if (str == "1" ||
                    string.Equals(str, "on", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "true", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (typeof(IConvertible).IsAssignableFrom(tmpType)) return Convert.ChangeType(str, tmpType);

            var targetConvter = TypeDescriptor.GetConverter(tmpType);
            if (targetConvter == null || !targetConvter.CanConvertFrom(typeof(string))) throw new NotSupportedException();

            return targetConvter.ConvertFrom(str);
        }
    }
}