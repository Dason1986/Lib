using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static bool IsRigth(this Enum x, Enum y)
        {
            return (x.HasFlag(y));
        }
    }
}
