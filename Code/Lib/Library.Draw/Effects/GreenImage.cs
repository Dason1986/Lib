using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 绿色
    /// </summary>
    [LanguageDescription("绿色"), LanguageDisplayName("绿色")]
    public class GreenImage : ImageBuilder
    {
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int widht = bmp.Width;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < widht; column++)
                {

                    var pixelValue = bmp.GetPixel(column,row);
                    bmp.SetPixel(column,row,  Color.FromArgb(pixelValue.A, 0, pixelValue.G, 0));
                }
            }
            return bmp;
        }


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
                    ptr[0] = 0;//B
                    ptr[1] = ptr[1];//G
                    ptr[2] = 0;//R
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}