using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 霓虹处理
    /// </summary>
    [LanguageDescription("霓虹处理"), LanguageDisplayName("霓虹处理")]
    public class NeonImage : ImageBuilder
    {
        /*
        霓虹处理算法：同样以3*3的点阵为例，目标像素g(i,j)应当以f(i,j)与f(i,j+1)，f(i,j)与f(i+1,j)的梯度作为R,G,B分量，我们不妨设f(i,j)的RGB分量为(r1, g1, b1), f(i,j+1)为(r2, g2, b2), f(i+1,j)为(r3, g3, b3), g(i, j)为(r, g, b),那么结果应该为
r = 2 * sqrt( (r1 - r2)^2 + (r1 - r3)^2 )
g = 2 * sqrt( (g1 - g2)^2 + (g1 - g3)^2 )
b = 2 * sqrt( (b1 - b2)^2 + (b1 - b3)^2 )

f(i,j)=2*sqrt[(f(i,j)-f(i+1,j))^2+(f(i,j)-f(,j+1))^2]
         */
        /// <summary>
        /// 
        /// </summary>
        [Category("濾鏡選項")]
        [LanguageDescription("顏色RGB:紅"), LanguageDisplayName("紅")]
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
        [Category("濾鏡選項")]
        [LanguageDescription("顏色RGB:綠"), LanguageDisplayName("綠")]
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
        [Category("濾鏡選項")]
        [LanguageDescription("顏色RGB:藍"), LanguageDisplayName("藍")]
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
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            Bitmap bmp = this.Source.Clone() as Bitmap;
            if (bmp == null) throw new ImageException("bmp");
            int width = bmp.Width;
            int height = bmp.Height;
            for (int i = 0; i < width - 1; i++)//注意边界的控制
            {
                for (int j = 0; j < height - 1; j++)
                {
                    Color cc1 = bmp.GetPixel(i, j);
                    Color cc2 = bmp.GetPixel(i, j + 1);
                    Color cc3 = bmp.GetPixel(i + 1, j);

                    int rr = 2 * (int)Math.Sqrt((cc3.R - cc1.R) * (cc3.R - cc1.R) + (cc2.R - cc1.R) * (cc2.R - cc1.R));
                    int gg = 2 * (int)Math.Sqrt((cc3.G - cc1.G) * (cc3.G - cc1.G) + (cc2.G - cc1.G) * (cc2.G - cc1.G));
                    int bb = 2 * (int)Math.Sqrt((cc3.B - cc1.B) * (cc3.B - cc1.B) + (cc2.B - cc1.B) * (cc2.B - cc1.B));
                    rr = Truncate(rr + Red);
                    gg = Truncate(gg + Green);
                    bb = Truncate(bb + Blue);

                    bmp.SetPixel(i, j, Color.FromArgb(rr, gg, bb));

                }
            }
            return bmp;
        }


        #region IImageProcessable 成员

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            Bitmap bmp = this.Source.Clone() as Bitmap;
            if (bmp == null) throw new ImageException("bmp");
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {

                    byte bb = (byte)(2 * Math.Sqrt((ptr[4] - ptr[0]) * (ptr[4] - ptr[0])) + (ptr[bmpData.Stride] - ptr[0]) * (ptr[bmpData.Stride] - ptr[0]));//b;
                    byte gg = (byte)(2 * Math.Sqrt((ptr[5] - ptr[1]) * (ptr[5] - ptr[1])) + (ptr[bmpData.Stride + 1] - ptr[1]) * (ptr[bmpData.Stride + 1] - ptr[1]));//g
                    byte rr = (byte)(2 * Math.Sqrt((ptr[6] - ptr[2]) * (ptr[6] - ptr[2])) + (ptr[bmpData.Stride + 2] - ptr[2]) * (ptr[bmpData.Stride + 2] - ptr[2]));//r
                    rr = Truncate(rr + Red);
                    gg = Truncate(gg + Green);
                    bb = Truncate(bb + Blue);
                    ptr[0] = bb;
                    ptr[1] = gg;
                    ptr[2] = rr;
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