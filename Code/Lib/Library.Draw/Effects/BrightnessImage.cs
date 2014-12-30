using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// ¡¡∂»
    /// </summary>
    public class BrightnessImage : ImageBuilder
    {


        /// <summary>
        /// 
        /// </summary>
        public int Brightness
        {
            get
            {
                InitOption();
                return _opetion.Brightness;
            }
            set
            {
                InitOption();
                _opetion.Brightness = value;
            }
        }
        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new BrightnessOption();
        }
        private BrightnessOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is BrightnessOption == false) throw new ImageException("Opetion is not BrightnessOption");
                _opetion = value as BrightnessOption;
            }
        }
        public class BrightnessOption : ImageOption
        {
            public int Brightness { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new BrightnessOption();
        }
        #endregion

        #region Process

        public override Image ProcessBitmap()
        {
            var sourceImage = Source.Clone() as Bitmap;
            if (Brightness < -255)
            {
                Brightness = -255;
            }
            if (Brightness > 255)
            {
                Brightness = 255;
            }

            int height = sourceImage.Height;
            int widht = sourceImage.Width;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < widht; column++)
                {
                    var pixelValue = sourceImage.GetPixel(column, row);



                    int b = pixelValue.B + Brightness;
                    int g = pixelValue.G + Brightness;
                    int r = pixelValue.R + Brightness;

                    if (b < 0)
                    {
                        b = 0;
                    }
                    else if (b > 255)
                    {
                        b = 255;
                    }

                    if (g < 0)
                    {
                        g = 0;
                    }
                    else if (b > 255)
                    {
                        g = 255;
                    }

                    if (r < 0)
                    {
                        r = 0;
                    }
                    else if (b > 255)
                    {
                        r = 255;
                    }



                    sourceImage.SetPixel(column, row, Color.FromArgb(pixelValue.A, r, g, b));
                }
            }
            return sourceImage;
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



                    int b = ptr[0] + Brightness;
                    int g = ptr[1] + Brightness;
                    int r = ptr[2] + Brightness;

                    if (b < 0)
                    {
                        b = 0;
                    }
                    else if (b > 255)
                    {
                        b = 255;
                    }

                    if (g < 0)
                    {
                        g = 0;
                    }
                    else if (b > 255)
                    {
                        g = 255;
                    }

                    if (r < 0)
                    {
                        r = 0;
                    }
                    else if (b > 255)
                    {
                        r = 255;
                    }

                    ptr[2] = (byte)r;
                    ptr[1] = (byte)g;
                    ptr[0] = (byte)b;
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