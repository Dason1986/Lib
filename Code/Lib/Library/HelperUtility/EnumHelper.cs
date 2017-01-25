using System;
using System.Collections.Generic;
using System.Linq;

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
        public static bool AnyIsRight(Enum x, Enum[] y)
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
        public static bool AllIsRight(Enum x, Enum[] y)
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
    }
}