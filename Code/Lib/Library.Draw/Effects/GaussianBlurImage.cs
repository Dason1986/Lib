using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 高斯模板
    /// </summary>
    [LanguageDescription("高斯模板"), LanguageDisplayName("高斯模板")]
    public class GaussianBlurImage : ImageBuilder
    {
        /*当前像素点与周围像素点的颜色差距较大时取其平均值 */
        //高斯模板
        readonly int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            Bitmap bitmap = new Bitmap(width, height);


            for (int x = 1; x < width - 1; x++)
                for (int y = 1; y < height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = bmp.GetPixel(x + row, y + col);
                            r += pixel.R * Gauss[index];
                            g += pixel.G * Gauss[index];
                            b += pixel.B * Gauss[index];
                            index++;
                        }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = Truncate(r);
                    g = Truncate(g);
                    b = Truncate(b);
                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            return bitmap;
        }

        public override unsafe Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            Bitmap bitmap = new Bitmap(width, height);

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int r = 0, g = 0, b = 0;
                    int index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {

                            int moveindex = (y + col) * bmpData.Stride + (x + row) * 4;

                            r += ptr[moveindex + 2] * Gauss[index];
                            g += ptr[moveindex + 1] * Gauss[index];
                            b += ptr[moveindex] * Gauss[index];
                            index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    int currnetindex = (y - 1) * bmpData.Stride + (x - 1) * 4;

                    ptr[currnetindex + 2] = Truncate(r);
                    ptr[currnetindex + 1] = Truncate(g);
                    ptr[currnetindex] = Truncate(b);
                }
            }
            bmp.UnlockBits(bmpData);
            return bitmap;
        }
    }
}
