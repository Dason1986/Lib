﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

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
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = bmp.GetPixel(x + row, y + col);
                            var tmpLap = Laplacian[index];
                            r += pixel.R * tmpLap;
                            g += pixel.G * tmpLap;
                            b += pixel.B * tmpLap;
                            index++;
                        }
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

        public override unsafe Image UnsafeProcessBitmap()
        {
            //   throw new NotImplementedException();
            var bmp = Source.Clone() as Bitmap;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//curBitmap.PixelFormat
            int w = bmpData.Width;
            int h = bmpData.Height;
            byte* ptr = (byte*)(bmpData.Scan0);
       
            for (int x = 1; x < w; x++)
            {
                for (int y = 1; y < h; y++)
                {
                    int rr = 0, gg = 0, bb = 0;
                    int index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            var tmpLap = Laplacian[index];
                            bb += ptr[0 + index * 3] * tmpLap;
                            gg += ptr[1 + index * 3] * tmpLap;
                            rr += ptr[2 + index * 3] * tmpLap;
                            index++;
                        }
                    }
                    //处理颜色值溢出
                    if (rr > 255) rr = 255;
                    if (rr < 0) rr = 0;
                    if (gg > 255) gg = 255;
                    if (gg < 0) gg = 0;
                    if (bb > 255) bb = 255;
                    if (bb < 0) bb = 0;
                    ptr[0] = (byte)bb;
                    ptr[1] = (byte)gg;
                    ptr[2] = (byte)rr;
                    ptr += 3;

                }
              //  ptr += bmpData.Stride - w * 3;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}