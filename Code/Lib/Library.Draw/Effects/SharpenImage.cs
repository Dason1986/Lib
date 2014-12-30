using System.Drawing;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 锐化
    /// </summary>
    public class SharpenImage : ImageBuilder
    {
        readonly int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
        /*获取图像有关形体的边缘, 并突出显示.*/
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            Bitmap bitmap = new Bitmap(width, height);


            //拉普拉斯模板

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = bmp.GetPixel(x + row, y + col); r += pixel.R * Laplacian[index];
                            g += pixel.G * Laplacian[index];
                            b += pixel.B * Laplacian[index];
                            index++;
                        }
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }
    }
}