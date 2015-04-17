using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 逆反处理
    /// </summary>
    [LanguageDescription("逆反处理"), LanguageDisplayName("逆反处理")]
    public class RebelliousImage : ImageBuilder
    {
        /*
         逆反处理的原理很简单，用255减去该像素的RGB作为新的RGB值即可。g(i,j)=255-f(i,j)
         */
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    int r = 255 - c.R;
                    int g = 255 - c.G;
                    int b = 255 - c.B;

                    bmp.SetPixel(i, j, Color.FromArgb(r, g, b));

                }
            }
            return bmp;
        }

        #region IImageProcessable 成员

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ptr[0] = (byte)(255 - ptr[0]);//B
                    ptr[1] = (byte)(255 - ptr[1]);//G
                    ptr[2] = (byte)(255 - ptr[2]);//R
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