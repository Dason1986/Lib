using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{


    /// <summary>
    /// 对比度/亮度
    /// </summary> 
    [LanguageDescription("对比度/亮度"), LanguageDisplayName("对比度/亮度")]
    public class BrightContrastImage : ImageBuilder
    {
        /// <summary>
        /// The brightness factor.
        /// Should be in the range [-1, 1].
        /// </summary>
        [LanguageDescription("亮度 [-1, 1]"), LanguageDisplayName("亮度"), Category("濾鏡選項")]
        public float BrightnessFactor
        {
            get
            {
                InitOption();
                return _opetion.BrightnessFactor;
            }
            set
            {
                InitOption();
                _opetion.BrightnessFactor = value;
            }
        }

        /// <summary>
        /// The contrast factor.
        /// Should be in the range [-1, 1].
        /// </summary>
        [LanguageDescription("对比因子 [-1, 1]"), LanguageDisplayName("对比度"), Category("濾鏡選項")]
        public float ContrastFactor
        {
            get
            {
                InitOption();
                return _opetion.ContrastFactor;
            }
            set
            {
                InitOption();
                _opetion.ContrastFactor = value;
            }
        }

        #region Option
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new BrightContrastlOption();
        }
        private BrightContrastlOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is BrightContrastlOption == false) throw new ImageException("Opetion is not BrightContrastlOption");
                _opetion = (BrightContrastlOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class BrightContrastlOption : ImageOption
        {
            /// <summary>
            /// The brightness factor.
            /// Should be in the range [-1, 1].
            /// </summary>
            [LanguageDescription("亮度 [-1, 1]"), LanguageDisplayName("亮度"), Category("濾鏡選項")]
            public float BrightnessFactor { get; set; }

            /// <summary>
            /// The contrast factor.
            /// Should be in the range [-1, 1].
            /// </summary>
            [LanguageDescription("对比因子 [-1, 1]"), LanguageDisplayName("对比度"), Category("濾鏡選項")]
            public float ContrastFactor { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new BrightContrastlOption() { BrightnessFactor = 0.25f, ContrastFactor = 0.25f };
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            int bfi = (int)(BrightnessFactor * 255);
            float cf = 1f + ContrastFactor;
            cf *= cf;
            int cfi = (int)(cf * 32768) + 1;
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    int r = pixelValue.R;
                    int g = pixelValue.G;
                    int b = pixelValue.B;
                    // Modify brightness (addition)
                    if (bfi != 0)
                    {
                        // Add brightness
                        int ri = r + bfi;
                        int gi = g + bfi;
                        int bi = b + bfi;
                        // Clamp to byte boundaries
                        r = (byte)Truncate(ri);
                        g = (byte)Truncate(gi);
                        b = (byte)Truncate(bi);
                    }
                    // Modifiy contrast (multiplication)
                    if (cfi != 32769)
                    {
                        // Transform to range [-128, 127]
                        int ri = r - 128;
                        int gi = g - 128;
                        int bi = b - 128;

                        // Multiply contrast factor
                        ri = (ri * cfi) >> 15;
                        gi = (gi * cfi) >> 15;
                        bi = (bi * cfi) >> 15;

                        // Transform back to range [0, 255]
                        ri = ri + 128;
                        gi = gi + 128;
                        bi = bi + 128;

                        // Clamp to byte boundaries
                        r = (byte)Truncate(ri);
                        g = (byte)Truncate(gi);
                        b = (byte)Truncate(bi);
                    }

                    bmp.SetPixel(column, row, Color.FromArgb(r, g, b));
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
            int height = bmp.Height;
            int width = bmp.Width;
            int bfi = (int)(BrightnessFactor * 255);
            float cf = 1f + ContrastFactor;
            cf *= cf;
            int cfi = (int)(cf * 32768) + 1;

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    byte r = ptr[2];
                    byte g = ptr[1];
                    byte b = ptr[0];
                    // Modify brightness (addition)
                    if (bfi != 0)
                    {
                        // Add brightness
                        int ri = r + bfi;
                        int gi = g + bfi;
                        int bi = b + bfi;
                        // Clamp to byte boundaries
                        r = (byte)Truncate(ri);
                        g = (byte)Truncate(gi);
                        b = (byte)Truncate(bi);
                    }
                    // Modifiy contrast (multiplication)
                    if (cfi != 32769)
                    {
                        // Transform to range [-128, 127]
                        int ri = r - 128;
                        int gi = g - 128;
                        int bi = b - 128;

                        // Multiply contrast factor
                        ri = (ri * cfi) >> 15;
                        gi = (gi * cfi) >> 15;
                        bi = (bi * cfi) >> 15;

                        // Transform back to range [0, 255]
                        ri = ri + 128;
                        gi = gi + 128;
                        bi = bi + 128;

                        // Clamp to byte boundaries
                        r = (byte)Truncate(ri);
                        g = (byte)Truncate(gi);
                        b = (byte)Truncate(bi);
                    }

                    ptr[2] = r;
                    ptr[1] = g;
                    ptr[0] = b;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }

           
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}