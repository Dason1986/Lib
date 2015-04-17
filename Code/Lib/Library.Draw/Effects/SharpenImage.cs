using System;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 锐化
    /// </summary>
    [LanguageDescription("锐化"), LanguageDisplayName("锐化")]
    public class SharpenImage : ImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public float Value
        {
            get
            {
                InitOption();
                return _opetion.Value;
            }
            set
            {
                InitOption();
                _opetion.Value = value;
            }
        }

        #region Option
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ValueOption();
        }
        private ValueOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ValueOption == false) throw new ImageException("Opetion is not ValueOption");
                _opetion = (ValueOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ValueOption() { Value = 1 };
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            float[] Laplacian = new float[] { -1, -1, -1, -1, 8 + Value, -1, -1, -1, -1 };
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    float r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            var color = bmp.GetPixel(x + row, y + col);
                            int rr = color.R;
                            int gg = color.G;
                            int bb = color.B;

                            r += rr * Laplacian[Index];
                            g += gg * Laplacian[Index];
                            b += bb * Laplacian[Index];
                            Index++;
                        }
                    }
                    bmp.SetPixel(x - 1, y - 1, Color.FromArgb(Truncate(r), Truncate(g), Truncate(b)));
                }
            }
            return bmp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            float[] Laplacian = new float[] { -1, -1, -1, -1, 8 + Value, -1, -1, -1, -1 };
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    float r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {

                            int moveindex = (y + col) * bmpData.Stride + (x + row) * 4;
                            int rr = ptr[moveindex + 2];
                            int gg = ptr[moveindex + 1];
                            int bb = ptr[moveindex];

                            r += rr * Laplacian[Index];
                            g += gg * Laplacian[Index];
                            b += bb * Laplacian[Index];
                            Index++;
                        }
                    }
                    int index = (y - 1) * bmpData.Stride + (x - 1) * 4;
                    ptr[index + 2] = Truncate(r);
                    ptr[index + 1] = Truncate(g);
                    ptr[index] = Truncate(b);

                }
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}