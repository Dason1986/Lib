using Library.HelperUtility;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// 長寬比
    /// </summary>
    public struct AspectRatioF
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AspectRatioF(float width, float height)
        {
            Width = width;
            Height = height;
        }
        /// <summary>
        /// 
        /// </summary>
        public float Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public float Height { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", Width, Height);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static AspectRatioF FormSize(float width, float height)
        {
            var gcd = MathUtility.GCD(width, height);
            return new AspectRatioF(width / gcd, height / gcd);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static AspectRatioF FormSize(SizeF size)
        {
            return FormSize(size.Width, size.Height);
        }
    }
    /// <summary>
    /// 長寬比
    /// </summary>
    public struct AspectRatio
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AspectRatio(int width, int height)
        {
            Width = width;
            Height = height;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", Width, Height);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static AspectRatio FormSize(int width, int height)
        {
            var gcd = MathUtility.GCD(width, height);
            return new AspectRatio(width / gcd, height / gcd);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static AspectRatio FormSize(Size size)
        {
            return FormSize(size.Width, size.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator AspectRatioF(AspectRatio p)
        {
            return new AspectRatioF(p.Width, p.Height);
        }
    }

    public static class HD
    {
        public const string HD1080P = "1080P";
        public const string HD720P = "720P";
        public const string HD4K = "4K";
        public const string HD2K = "2K";
        public const string HD8K = "8K";

        //        4K UHDTV（2160p）的宽高为3840×2160。总像素数是全高清1080p的4倍。
        //8K UHDTV（4320p）的宽高为7680×4320。总像素数是全高清1080p的16倍。

        //        DCI 2K(原生分辨率)  2048 × 1080	1.90:1 (256:135, ~17:9)	2,211,840
        //DCI 2K(扁平裁切)   1998 × 1080	1.85:1	2,157,840
        //DCI 2K(宽屏幕裁切)  2048 × 858	2.39:1	1,755,136
        //PC 2K(1080p)   1920 × 1080	1.(7):1 (16:9)	2,073,600


        //分辨率有2种规格：3840×2160和4096×2160像素。
        //public static readonly Size FullAperture4K = new Size(4096 , 3112);1.32:1
        //public static readonly Size Academy4K = new Size(3656, 2664);1.37:1
        //public static readonly Size DigitalCinema4K = new Size(4096, 1714);2.39:1
        //public static readonly Size DigitalCinema4K = new Size(3996, 2160);1.85:1

        //public static readonly Size FullAperture4K = new Size(4096, 3072);4:3
        //public static readonly Size Academy4K = new Size(3656, 2664);1.37:1



    }
}

