using System;
using System.Collections.Generic;
using Library.Annotations;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _func;
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateEqualityComparer([NotNull] Func<T, T, bool> func)
        {
            _func = func;
            if (func == null) throw new ArgumentNullException("func");
        }

        public bool Equals(T x, T y)
        {
            return _func(x, y);
        }

        public int GetHashCode(T obj)
        {
            if (obj == null) return -1;
            return obj.GetHashCode();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CompareUtility
    {
        static CompareUtility()
        {
            StringEqualityComparer = new DelegateEqualityComparer<string>((x, y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool IsBetween<T>(this T x, T min, T max) where T : IComparable
        {
            if (min.CompareTo(max) > 0) throw new Exception("跋丁程ぃ跋丁程");
            return (x.CompareTo(min) >= 0 && x.CompareTo(max) <= 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly IEqualityComparer<string> StringEqualityComparer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool IsBetween<T>(this T? x, T min, T max) where T : struct,IComparable
        {
            if (x == null) return false;
            if (min.CompareTo(max) > 0) throw new Exception("跋丁程ぃ跋丁程");
            var val = x.Value;
            return (val.CompareTo(min) >= 0 && val.CompareTo(max) <= 0);
        }
        #region

        private static bool Compare(decimal x, decimal y)
        {
            return x.CompareTo(y) == 0;
        }

        private static bool Compare(double x, decimal y)
        {
            return x.CompareTo(y) == 0;
        }

        private static bool Compare(decimal x, double y)
        {
            return x.CompareTo(y) == 0;
        }

        private static bool Compare(double x, double y)
        {
            return x.CompareTo(y) == 0;
        }

        #endregion

        #region decimal

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal x, decimal y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }


        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, decimal y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal x, decimal? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, decimal? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        #endregion


        #region double

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double x, double y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y, digth);
            return tmpx.CompareTo(tmpy) == 0;
        }

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double? x, double y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double x, double? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double? x, double? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        #endregion

        #region double&decimal
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double x, decimal y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y, digth);
            return tmpx.CompareTo(tmpy) == 0;
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double? x, decimal y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double x, decimal? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(double? x, decimal? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal x, double y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y, digth);
            return tmpx.CompareTo(tmpy) == 0;
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, double y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal x, double? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// ㄢゑ耕┛菠﹚计
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">﹚计</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, double? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        #endregion
    }
}