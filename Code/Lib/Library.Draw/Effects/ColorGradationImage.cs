using Library.Att;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 色階
    /// </summary>
    [LanguageDescription("色階"), LanguageDisplayName("色階")]
    public class ColorGradationImage : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("RGB:紅"), LanguageDisplayName("紅"), Category("濾鏡選項")]
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

        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("RGB:綠"), LanguageDisplayName("綠"), Category("濾鏡選項")]
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

        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("RGB:藍"), LanguageDisplayName("藍"), Category("濾鏡選項")]
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

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ColorOption();
        }

        private ColorOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ColorOption == false) throw new ImageException("Opetion is not ColorOption");
                _opetion = (ColorOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ColorOption();
        }

        #endregion Option

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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
                    int rr = Truncate(pixelValue.R + Red);
                    int gg = Truncate(pixelValue.G + Green);
                    int bb = Truncate(pixelValue.B + Blue);

                    bmp.SetPixel(column, row, Color.FromArgb(pixelValue.A, rr, gg, bb));
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
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int rr = Truncate(ptr[2] + Red);
                    int gg = Truncate(ptr[1] + Green);
                    int bb = Truncate(ptr[0] + Blue);

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