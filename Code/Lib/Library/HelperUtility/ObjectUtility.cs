using System;
using System.ComponentModel;
using System.Diagnostics;
using Library.Annotations;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryResult<T> TryCast<T>(object value)
        {
            try
            {
                return (T)Cast(value, typeof(T),default(T));
            }
            catch (Exception ex)
            {

                return new TryResult<T>(default(T), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Cast<T>(object value)
        {

            return (T)Cast(value, typeof(T),default(T));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultvalue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryResult<T> TryCast<T>(object value, T defaultvalue)
        {
            try
            {
                return (T)Cast(value, typeof(T), defaultvalue);
            }
            catch (Exception ex)
            {

                return new TryResult<T>(default(T), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static TryResult<object> TryCast(object value, Type targetType, object defaultvalue)
        {

            try
            {
                return Cast(value, targetType,null);

            }
            catch (Exception ex)
            {

                return new TryResult<object>(defaultvalue, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static object Cast(object value, Type targetType,object defaultValue=null)
        {
            if (value == null) return defaultValue;
            if (targetType == null) throw new ArgumentNullException("targetType");
            if (value == null) throw new ArgumentNullException("value");
            var tmpType = targetType.RemoveNullabl();

            if (tmpType.IsInstanceOfType(value)) return value;
            if (tmpType == typeof(string)) return value.ToString();

            var convertible = value as IConvertible;

            var code = Convert.GetTypeCode(value);
            switch (code)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    throw new ArgumentNullException("value");

                case TypeCode.String:
                    return StringUtility.Cast((string)value, tmpType);

                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    {
                        if (tmpType.IsEnum)
                        {
                            return Enum.ToObject(tmpType, value);
                        }
                        if (tmpType == typeof(bool))
                        {
                            return (decimal)value > 0;
                        }
                        if (convertible != null) return Convert.ChangeType(value, tmpType);
                        break;
                    }
            }
            var valueType = value.GetType();
            var targetConvter = TypeDescriptor.GetConverter(tmpType);
            if (targetConvter.CanConvertFrom(valueType)) return targetConvter.ConvertFrom(value);
            var toConvter = TypeDescriptor.GetConverter(valueType);
            if (toConvter.CanConvertTo(tmpType)) return toConvter.ConvertTo(value, tmpType);

            throw new NotSupportedException();
        }
    }
}