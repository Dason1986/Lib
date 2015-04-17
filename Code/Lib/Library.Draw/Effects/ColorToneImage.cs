using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 調色/飽和
    /// </summary> 
    [LanguageDescription("調色/飽和"), LanguageDisplayName("調色/飽和")]
    public class ColorToneImage : ImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("調色"), LanguageDisplayName("調色"), Category("濾鏡選項")]

        public Color Tone
        {
            get
            {
                InitOption();
                return _opetion.Tone;
            }
            set
            {
                InitOption();
                _opetion.Tone = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("飽和"), LanguageDisplayName("飽和"), Category("濾鏡選項")]

        public int Saturation
        {
            get
            {
                InitOption();
                return _opetion.Saturation;
            }
            set
            {
                InitOption();
                _opetion.Saturation = value;
            }
        }

        #region Option
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ColorToneOption();
        }
        private ColorToneOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ColorToneOption == false) throw new ImageException("Opetion is not ColorToneOption");
                _opetion = (ColorToneOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ColorToneOption();
        }

        /// <summary>
        /// 
        /// </summary>
        public class ColorToneOption : ImageOption
        { /// <summary>
            /// 
            /// </summary>
            [LanguageDescription("調色"), LanguageDisplayName("調色"), Category("濾鏡選項")]
            public Color Tone { get; set; }
            /// <summary>
            /// 
            /// </summary> 
            [LanguageDescription("飽和"), LanguageDisplayName("飽和"), Category("濾鏡選項")]
            public int Saturation { get; set; }
        }


        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            SetColorToneFilter(Tone, Saturation);
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    int r = pixelValue.R;
                    int g = pixelValue.G;
                    int b = pixelValue.B;

                    double l = _lum_tab[GetGrayscale(r, g, b)];
                    var cr = HLStoRGB(_hue, l, _saturation);

                    bmp.SetPixel(column, row, cr);
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
            SetColorToneFilter(Tone, Saturation);
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];

                    double l = _lum_tab[GetGrayscale(r, g, b)];
                    var cr = HLStoRGB(_hue, l, _saturation);

                    ptr[2] = cr.R;
                    ptr[1] = cr.G;
                    ptr[0] = cr.B;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #region MyRegion

        double _hue;
        double _saturation;
        double[] _lum_tab = new double[256];

        // @name RGB <--> HLS (Hue, Lightness, Saturation).
        //@{
        /**
            RGB --> HLS \n
            prgb - address of 24bpp or 32bpp pixel.
        */
        static double[] RGBtoHLS(Color rgb, double H, double L, double S)
        {
            int colorR = rgb.R;
            int colorG = rgb.G;
            int colorB = rgb.B;
            //Color color = Color.rgb(colorR, colorG, bluecolorB);
            int n_cmax = Math.Max(colorR, Math.Max(colorG, colorB));
            int n_cmin = Math.Min(colorR, Math.Min(colorG, colorB));

            L = (n_cmax + n_cmin) / 2.0 / 255.0;
            if (n_cmax == n_cmin)
            {
                S = 0.0;
                H = 0.0;
                return new double[] { H, L, S };
            }

            double r = colorR / 255.0,
                     g = colorG / 255.0,
                     b = colorB / 255.0,
                     cmax = n_cmax / 255.0,
                     cmin = n_cmin / 255.0,
                     delta = cmax - cmin;

            if (L < 0.5)
                S = delta / (cmax + cmin);
            else
                S = delta / (2.0 - cmax - cmin);

            if (colorR == n_cmax)
                H = (g - b) / delta;
            else if (colorG == n_cmax)
                H = 2.0 + (b - r) / delta;
            else
                H = 4.0 + (r - g) / delta;

            H /= 6.0;

            if (H < 0.0)
                H += 1.0;

            return new double[] { H, L, S };
        }

        static Color DoubleRGB_to_RGB(double r, double g, double b)
        {
            return Color.FromArgb(((int)(r * 255)), ((int)(g * 255)), ((int)(b * 255)));
        }

        static double HLS_Value(double n1, double n2, double h)
        {
            if (h > 6.0)
                h -= 6.0;
            else if (h < 0.0)
                h += 6.0;

            if (h < 1.0)
                return n1 + (n2 - n1) * h;
            else if (h < 3.0)
                return n2;
            else if (h < 4.0)
                return n1 + (n2 - n1) * (4.0 - h);
            return n1;
        }

        /// HLS --> RGB.
        static Color HLStoRGB(double H, double L, double S)
        {
            if ((!(S > 0)) && (!(S < 0))) // == 0
                return DoubleRGB_to_RGB(L, L, L);

            double m1, m2;
            if (L > 0.5)
                m2 = L + S - L * S;
            else
                m2 = L * (1.0 + S);
            m1 = 2.0 * L - m2;

            double r = HLS_Value(m1, m2, H * 6.0 + 2.0);
            double g = HLS_Value(m1, m2, H * 6.0);
            double b = HLS_Value(m1, m2, H * 6.0 - 2.0);
            return DoubleRGB_to_RGB(r, g, b);
        }


        /**
           Calculate grayscale value of pixel \n
           prgb - address of 24bpp or 32bpp pixel.
       */
        static int GetGrayscale(int r, int g, int b)
        {
            return (int)((30 * r + 59 * g + 11 * g) / 100);
        }

        void SetColorToneFilter(Color tone, int saturation)
        {
            double l = 0.0f;
            double[] result = RGBtoHLS(tone, _hue, l, _saturation);
            _hue = result[0];
            l = result[1];
            _saturation = result[2];
            _saturation = _saturation * (saturation / 255.0) * (saturation / 255.0);
            _saturation = ((_saturation < 1) ? _saturation : 1);

            for (int i = 0; i < 256; i++)
            {
                var cr = Color.FromArgb(i, i, i);
                double h = 0.0f, ll = 0.0f, s = 0.0f;
                result = RGBtoHLS(cr, h, ll, s);
                h = result[0];
                ll = result[1];
                s = result[2];
                ll = ll * (1 + (128 - Math.Abs(saturation - 128)) / 128.0 / 9.0);
                _lum_tab[i] = ((ll < 1) ? ll : 1);
            }
        }


        #endregion
    }
}