using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 光照效果
    /// </summary>
    [LanguageDescription("照明图像"), LanguageDisplayName("照明图像")]
    public class IlluminationImage : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        [DefaultValue(50)]
        [LanguageDescription("光點大小"), LanguageDisplayName("光點大小"), Category("濾鏡選項")]
        public int Radii
        {
            get
            {
                InitOption();
                return _opetion.Radii;
            }
            set
            {
                InitOption();
                _opetion.Radii = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(typeof(Point), "10,50")]
        [LanguageDescription("光點位置"), LanguageDisplayName("光點位置"), Category("濾鏡選項")]
        public Point Center
        {
            get
            {
                InitOption();
                return _opetion.Center;
            }
            set
            {
                InitOption();
                _opetion.Center = value;
            }
        }

        #region Option

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new IlluminationsOption();
        }

        private IlluminationsOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is IlluminationsOption == false) throw new ImageException("Opetion is not IlluminationsOption");
                _opetion = (IlluminationsOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class IlluminationsOption : ImageOption
        {
            /// <summary>
            /// 光點大小
            /// </summary>
            [LanguageDescription("光點大小"), LanguageDisplayName("光點大小"), Category("濾鏡選項")]
            public int Radii { get; set; }

            /// <summary>
            /// 光點位置
            /// </summary>
            [LanguageDescription("光點位置"), LanguageDisplayName("光點位置"), Category("濾鏡選項")]
            public Point Center { get; set; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new IlluminationsOption() { Radii = 10, Center = new Point(50, 20) };
        }

        #endregion Option

        /*
         按照一定的规则对图像中某范围内像素的亮度进行处理后, 能够产生类似光照的效果...
         */

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override System.Drawing.Image ProcessBitmap()
        {
            Bitmap myBmp = this.Source.Clone() as Bitmap;
            if (myBmp == null) throw new Exception();
            int myWidth = myBmp.Width;
            int myHeight = myBmp.Height;

            //MyCenter图片中心点，发亮此值会让强光中心发生偏移
            Point myCenter = this.Center;
            //R强光照射面的半径，即”光晕”
            int R = Radii;
            for (int i = myWidth - 1; i >= 1; i--)
            {
                for (int j = myHeight - 1; j >= 1; j--)
                {
                    float myLength = (float)Math.Sqrt(Math.Pow((i - myCenter.X), 2) + Math.Pow((j - myCenter.Y), 2));
                    //如果像素位于”光晕”之内
                    if (!(myLength < R)) continue;
                    Color myColor = myBmp.GetPixel(i, j);
                    //220亮度增加常量，该值越大，光亮度越强
                    float myPixel = 220.0f * (1.0f - myLength / R);
                    int r = myColor.R + (int)myPixel;
                    r = Math.Max(0, Math.Min(r, 255));
                    int g = myColor.G + (int)myPixel;
                    g = Math.Max(0, Math.Min(g, 255));
                    int b = myColor.B + (int)myPixel;
                    b = Math.Max(0, Math.Min(b, 255));
                    //将增亮后的像素值回写到位图
                    Color myNewColor = Color.FromArgb(255, r, g, b);
                    myBmp.SetPixel(i, j, myNewColor);
                }
            }
            return myBmp;
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
            int R = Radii;
            int r = 0, g = 0, b = 0;
            Point myCenter = this.Center;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float myLength = (float)Math.Sqrt(Math.Pow((x - myCenter.X), 2) + Math.Pow((y - myCenter.Y), 2));
                    //如果像素位于”光晕”之内
                    if (!(myLength < R))
                    {
                        ptr += 4;
                        continue;
                    }

                    //220亮度增加常量，该值越大，光亮度越强
                    float myPixel = 220.0f * (1.0f - myLength / R);
                    r = ptr[2] + (int)myPixel;

                    g = ptr[1] + (int)myPixel;

                    b = ptr[0] + (int)myPixel;
                    ptr[2] = this.Truncate(r);
                    ptr[1] = this.Truncate(g);
                    ptr[0] = this.Truncate(b);

                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}