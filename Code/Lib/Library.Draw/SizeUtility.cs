using System;
using System.Drawing;

namespace Library.HelperUtility
{
    
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