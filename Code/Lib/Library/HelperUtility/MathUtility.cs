using System;

namespace Library.HelperUtility
{
    /// <summary>
    ///數學工具
    /// </summary>
    public static class MathUtility
    {
        /// <summary>
        /// 是否2的乘方
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPowerOf2(int number)
        {
            return (number & number - 1) == 0;
        }
        /// <summary>
        /// 最大公約數
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GCD(int a, int b)
        {
            if (0 != b) while (0 != (a %= b) && 0 != (b %= a)) ;
            return a + b;
        }

        /// <summary>
        /// 最大公約數
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GCD(float a, float b)
        {
            if (0 != b) while (0 != (a %= b) && 0 != (b %= a)) ;
            return a + b;
        }
        /// <summary>
        /// 最小公倍數
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int LCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }
        /// <summary>
        /// 對角線
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int GetDiagonal(int width, int height)
        {

            var f = Math.Round(Math.Sqrt(Math.Pow(height, 2) + Math.Pow(width, 2)), 0);
            return (int)f;
        }
    }
}