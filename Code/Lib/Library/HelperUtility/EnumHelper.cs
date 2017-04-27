using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Library.Date;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool IsRight(this Enum x, Enum y)
        {
            return (x.HasFlag(y));
        }

        /// <summary>
        /// x是否包含其中一個記錄
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool AnyIsRight(this Enum x, Enum[] y)
        {
            if (!y.HasRecord()) return false;
            return y.Any(y1 => (x.GetHashCode() & y1.GetHashCode()) == y1.GetHashCode());
        }

        /// <summary>
        /// x所有包含記錄
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool AllIsRight(this Enum x, Enum[] y)
        {
            if (!y.HasRecord()) return false;
            return y.All(y1 => (x.GetHashCode() & y1.GetHashCode()) == y1.GetHashCode());
        }

        /// <summary>
        /// 取後包含的枚舉數組，
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <example>
        /// enum= A1｜B2|C3
        /// input A1|B2
        /// output enum[]{A1,B2}
        /// </example>
        public static TEnum[] GetIncludeEnums<TEnum>(this Enum x)
        {
            var type = x.GetType();
            if (typeof(TEnum) != type) throw new LibException("TEnum is not input enum type");
            var tragetarr = Enum.GetValues(type).Cast<TEnum>();
            List<TEnum> list = new List<TEnum>();
            var xvalue = x.GetHashCode();
            foreach (var item in tragetarr)
            {
                var y = item.GetHashCode();
                if (y == 0) continue;
                if ((xvalue & y) == y)
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 获取DisplayAttribute上指定的Name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDisplay(this Enum value)
        {
            var info = value.GetType().GetField(value.ToString());
            var attribute = (DisplayAttribute)info.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
            return attribute == null ? value.ToString() : attribute.Name;
        }

        /// <summary>
        ///  获取DescriptionAttribute上指定的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            return EnumHelper.GetEnumDescription(value);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="inEnum"></param>
        /// <param name="resource"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetResourceStrnig(this Enum inEnum, System.Resources.ResourceManager resource, string prefix = "")
        {
            var type = inEnum.GetType();

            var flags = inEnum.GetHashCode();
            var hasPrefix = !string.IsNullOrEmpty(prefix);
            List<String> list = new List<string>();
            foreach (Enum n in Enum.GetValues(type))
            {
                var x = n.GetHashCode();
                if (x == 0) continue;
                if ((x & flags) != x) continue;
                var name = hasPrefix ? prefix + n : n.ToString();
                list.Add(resource.GetString(name) ?? name);
            }

            return string.Join(",", list);
        }

   
  

        /// <summary>
        /// 取得枚举类型的说明文字
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum objEnum)
        {
            Type typeDescription = typeof(DescriptionAttribute);
            Type typeField = objEnum.GetType();
            string strDesc = string.Empty;
            try
            {
                FieldInfo field = typeField.GetField(objEnum.ToString());
                var arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    strDesc = (arr[0] as DescriptionAttribute).Description;
                }
                else
                {
                    strDesc = field.Name;
                }
            }
            catch
            {
                strDesc = string.Empty;
            }

            return strDesc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEnum"></param>
        /// <param name="flag">默认值为0.获取Display属性Name值，否则获取Descriptiom</param>
        /// <returns></returns>
        public static string GetEnumDisplay(this Enum objEnum, int flag = 0)
        {
            var typeDisplayName = typeof(DisplayAttribute);
            var typeField = objEnum.GetType();
            string strDesc = string.Empty;
            try
            {
                FieldInfo field = typeField.GetField(objEnum.ToString());
                var arr = field.GetCustomAttributes(typeDisplayName, true);
                if (arr.Length > 0)
                {
                    var displayAttribute = arr[0] as DisplayAttribute;
                    if (displayAttribute != null)
                        strDesc = flag == 0 ? displayAttribute.Name : displayAttribute.Description;
                }
                else
                {

                    strDesc = field.Name;
                }
            }
            catch
            {
                strDesc = String.Empty;
            }

            return strDesc;
        }
        /// <summary>
        /// 取得枚举类型的Display属性
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static DisplayAttribute[] GetEnumDisplayAttributs(this Enum objEnum)
        {
            var typeDisplayName = typeof(DisplayAttribute);
            var typeField = objEnum.GetType();
            try
            {
                var field = typeField.GetField(objEnum.ToString());
                var arr = field.GetCustomAttributes(typeDisplayName, true);
                return arr.OfType<DisplayAttribute>().ToArray();
            }
            catch
            {
                return new DisplayAttribute[0];
            }
        }

        /// <summary>
        /// 把值转换为相应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="rawVal">值</param>
        /// <param name="defVal">默认值</param>
        /// <returns></returns>
        public static T ConvertToEnum<T>(string rawVal, T defVal = default(T)) where T : struct
        {
            T objEnum;
            Type typeEnum = typeof(T);
            if (String.IsNullOrEmpty(rawVal)) return defVal;
            //objEnum = (T)Enum.Parse(typeEnum, rawVal.ToString());
            if (!Enum.TryParse<T>(rawVal, out objEnum) || !Enum.IsDefined(typeEnum, objEnum))
            {
                objEnum = defVal;
            }
            return objEnum;
        }
        
    }
}