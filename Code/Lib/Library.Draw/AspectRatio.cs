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

    /// <summary>
    /// 
    /// </summary>
    public class ResolutionEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AspectRatio Ratio { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntity HD1080P=new ResolutionEntity() {Name ="1080P", Size=new Size(1920, 1080),Ratio = new AspectRatio(16,9)};

        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntity HD720P = new ResolutionEntity() { Name = "720P", Size = new Size(1280, 720), Ratio = new AspectRatio(16, 9) };
        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntity HD4K = new ResolutionEntity() { Name = "4K", Size = new Size(4096, 2160), Ratio = new AspectRatio(17, 9) };
        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntity HD8K = new ResolutionEntity() { Name = "8K", Size = new Size(7680, 4320), Ratio = new AspectRatio(16, 9) }; 

        

    



    }
}

