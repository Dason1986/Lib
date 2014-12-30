using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 色階
    /// </summary>
    public class ColorGradationImage : ImageBuilder
    {
        public int Red
        {
            get
            {
                InitOption();
                return _opetion.Red;
            }
            set
            {
                InitOption();
                _opetion.Red = value;
            }
        }
        public int Green
        {
            get
            {
                InitOption();
                return _opetion.Green;
            }
            set
            {
                InitOption();
                _opetion.Green = value;
            }
        }
        public int Blue
        {
            get
            {
                InitOption();
                return _opetion.Blue;
            }
            set
            {
                InitOption();
                _opetion.Blue = value;
            }
        }
        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ColorOption();
        }
        private ColorOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ColorOption == false) throw new ImageException("Opetion is not ColorOption");
                _opetion = value as ColorOption;
            }
        }

        public override ImageOption CreateOption()
        {
            return new ColorOption();
        }
        #endregion
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int widht = bmp.Width;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < widht; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    int rr = pixelValue.R + Red;
                    int gg = pixelValue.G + Green;
                    int bb = pixelValue.B + Blue;

                    if (rr > 255) rr = 255;
                    if (rr < 0) rr = 0;
                    if (gg > 255) gg = 255;
                    if (gg < 0) gg = 0;
                    if (bb > 255) bb = 255;
                    if (bb < 0) bb = 0;
                    bmp.SetPixel(column, row, Color.FromArgb(pixelValue.A, rr, gg, bb));
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

                    int rr = ptr[2] + Red;
                    int gg = ptr[1] + Green;
                    int bb = ptr[0] + Blue;
                    if (rr > 255) rr = 255;
                    if (rr < 0) rr = 0;
                    if (gg > 255) gg = 255;
                    if (gg < 0) gg = 0;
                    if (bb > 255) bb = 255;
                    if (bb < 0) bb = 0;
                    ptr[2] = (byte)rr;//B
                    ptr[1] = (byte)gg;//G
                    ptr[0] = (byte)bb;//R
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}