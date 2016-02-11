using System;
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
    }
}