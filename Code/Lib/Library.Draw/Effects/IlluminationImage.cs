using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 光照效果
    /// </summary>
    public class IlluminationImage : ImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(50)]
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

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new IlluminationsOption();
        }
        private IlluminationsOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is IlluminationsOption == false) throw new ImageException("Opetion is not IlluminationsOption");
                _opetion = value as IlluminationsOption;
            }
        }
        public class IlluminationsOption : ImageOption
        {
            /// <summary>
            /// 
            /// </summary>
            public int Radii { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Point Center { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new IlluminationsOption() { Radii = 10, Center = new Point(50, 20) };
        }
        #endregion
        /*
         按照一定的规则对图像中某范围内像素的亮度进行处理后, 能够产生类似光照的效果...
         */
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
    }
}
