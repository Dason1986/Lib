using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.ComponentModel;
using Library.HelperUtility;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public class BitComputing
    {
        /// <summary>
        /// 或運算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int OrOperation(int x, int y)
        {
            return x & y;
        }
        /// <summary>
        /// 是否包含y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool IsRight(int x, int y)
        {
            return (x & y) == y;
        }

        /// <summary>
        /// x是否包含其中一個記錄
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool AnyIsRight(int x, int[] y)
        {
            if (!y.HasRecord()) return false;
            return y.Any(y1 => (x & y1) == y1);
        }
        /// <summary>
        /// x所有包含記錄
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool AllIsRight(int x, int[] y)
        {
            if (!y.HasRecord()) return false;
            return y.All(y1 => (x & y1) == y1);
        }

        /// <summary>
        /// 左位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitLeftOperation(int x)
        {
            return x << 1;
        }
        /// <summary>
        /// 右位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitRightOperation(int x)
        {
            return x >> 1;
        }
        /// <summary>
        /// 左位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitLeft2Operation(int x)
        {
            return x << 2;
        }
        /// <summary>
        /// 右位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitRight2Operation(int x)
        {
            return x >> 2;
        }
        /// <summary>
        /// 左位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitLeft3Operation(int x)
        {
            return x << 3;
        }
        /// <summary>
        /// 右位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitRight3Operation(int x)
        {
            return x >> 3;
        }
        /// <summary>
        /// 左位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitLeft4Operation(int x)
        {
            return x << 4;
        }
        /// <summary>
        /// 右位移
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int BitRight4Operation(int x)
        {
            return x >> 4;
        }
        /// <summary>
        /// 與運算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int AndOperation(int x, int y)
        {
            return x | y;
        }
        /// <summary>
        /// 非運算
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int NonOperation(int x)
        {
            return ~x;
        }
        /// <summary>
        /// 异或运算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int XOROperation(int x, int y)
        {
            return x ^ y;
        }
    }
}
