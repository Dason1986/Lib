using System;
using System.Drawing;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathUtility
    {
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
        public static int GetDiagonal(int width,int height)
        {

            var f = Math.Round(Math.Sqrt(Math.Pow(height, 2) + Math.Pow(width, 2)), 0);
            return (int)f;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class SizeUtility
    {
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int GetMillionPixels(Size size)
        {

            var f = size.Height * size.Width / 1000000;
            return f;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static Size ZoomSizeByPercentage(Size size, float percentage)
        {
            return ZoomSizeFByPercentage(size, percentage).ToSize();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Size ZoomSizeByPixels(Size size, int pixels)
        {

            return ZoomSizeFByPixels(size, pixels).ToSize();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static SizeF ZoomSizeFByPercentage(SizeF size, float percentage)
        {
            if (size.Width > size.Height)
            {
                return new SizeF(size.Width * percentage, size.Height * percentage);
            }
            else
            {
                return new SizeF(size.Width * percentage, size.Height * percentage);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static SizeF ZoomSizeFByPixels(SizeF size, float pixels)
        {
            float percentage = 100;
            if (size.Width > size.Height)
            {
                percentage = pixels / size.Width;
            }
            else
            {
                percentage = pixels / size.Height;

            }
            return ZoomSizeFByPercentage(size, percentage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle GetSquareRectangle(Size size)
        {
           
            var min = Math.Min(size.Width, size.Height);
            if (min == size.Width)
            {
                var marginBottom = size.Height / 2;
                var marginTop = size.Height / 4;
                return new Rectangle(0, marginTop, size.Width, marginBottom);
            }
            else
            {
                var marginRight = size.Width / 2;
                var marginleft = size.Width / 4;
                return new Rectangle(marginleft, 0, marginRight, size.Height);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static RectangleF GetSquareRectangleF(SizeF image)
        {
            var min = Math.Min(image.Width, image.Height);
            if (min == image.Width)
            {
                var marginBottom = image.Height / 2;
                var marginTop = image.Height / 4;
                return new RectangleF(0, marginTop, image.Width, marginBottom);
            }
            else
            {
                var marginRight = image.Width / 2;
                var marginleft = image.Width / 4;
                return new RectangleF(marginleft, 0, marginRight, image.Height);
            }
        }
    }
}