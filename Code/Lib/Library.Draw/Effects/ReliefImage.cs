using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 浮雕处理
    /// </summary>
    [LanguageDescription("浮雕处理"), LanguageDisplayName("浮雕处理")]
    public class ReliefImage : ImageBuilder
    {

        /********************************************************************
         * 
         * 浮雕处理原理：通过对图像像素点的像素值与相邻像素点的像素值相减后加上128, 然后作为新的像素点的值...

         *  g(i,j)=f(i,i)-f(i+1,j)+128
         * 
         * ******************************************************************/
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width - 1; i++)//注意控制边界  相邻元素 i+1=width
                {
                    Color c1 = bmp.GetPixel(i, j);
                    Color c2 = bmp.GetPixel(i + 1, j);//相邻的像素
                    var rr = Truncate(c1.R - c2.R + 128);
                    var gg = Truncate(c1.G - c2.G + 128);
                    var bb = Truncate(c1.B - c2.B + 128);



                    bmp.SetPixel(i, j, Color.FromArgb(rr, gg, bb));
                }
            }
            return bmp;
        }


        #region IImageProcessable 成员


        public unsafe override Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {


                    var rr = Truncate(ptr[2] - ptr[6] + 128);
                    var gg = Truncate(ptr[1] - ptr[5] + 128);
                    var bb = Truncate(ptr[0] - ptr[4] + 128);

                    ptr[0] = bb;
                    ptr[1] = gg;
                    ptr[2] = rr;

                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #endregion
    }
}