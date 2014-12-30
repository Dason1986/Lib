using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 高斯模板
    /// </summary>
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
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            return bitmap;
        }
    }
}
