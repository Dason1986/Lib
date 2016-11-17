using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 二值处理
    /// </summary>
    [LanguageDescription("二值处理"),  LanguageDisplayName("黑白處理")]
    public class TwoValueImage : ImageBuilder
    {
        /// <summary>
        /// 判斷值的侵害點
        /// </summary>
        [LanguageDescription("侵害點, 0-255"), LanguageDisplayName("判斷值的侵害點"), Category("濾鏡選項")]
        public int Pointcut
        {
            get
            {
                InitOption();
                return _opetion.Pointcut;
            }
            set
            {
                InitOption();
                _opetion.Pointcut = value;
            }
        }

        /*
         二值处理，顾名思义，将图片处理后就剩下二值了，0、255就是RGB取值的极限值，
         * 图片只剩下黑白二色，从上一篇C#图片处理常见方法性能比较 可知，二值处理为图像灰度彩色变黑白灰度处理的一个子集，
         * 只不过值就剩下0和255了，因此处理方法有些类似。进行加权或取平均值后进行极端化，若平均值大于等于128则255，否则0.
         */

        #region Option

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new TwoValueOption();
        }

        private TwoValueOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is TwoValueOption == false) throw new ImageException("Opetion is not TwoValueOption");
                _opetion = (TwoValueOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class TwoValueOption : ImageOption
        {
            private int _pointcut;

            /// <summary>
            ///
            /// </summary>
            /// <exception cref="ImageException"></exception>
            [LanguageDescription("侵害點 , 0-255"), LanguageDisplayName("侵害點"), Category("濾鏡選項")]
            public int Pointcut
            {
                get { return _pointcut; }
                set
                {
                    if (value < 0 || value > 255) throw new ImageException("value is 0-255");
                    _pointcut = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new TwoValueOption() { Pointcut = 100 };
        }

        #endregion Option

        #region IImageProcessable 成员

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    int iAvg = (c.R + c.G + c.B) / 3;
                    int iPixel = iAvg >= Pointcut ? byte.MaxValue : 0;

                    bmp.SetPixel(i, j, Color.FromArgb(iPixel, iPixel, iPixel));
                }
            }
            return bmp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public unsafe override Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;

            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int byteCounts = bmpData.Stride * height;
            byte[] arr = new byte[byteCounts];
            IntPtr p = bmpData.Scan0;
            Marshal.Copy(p, arr, 0, byteCounts);
            for (int i = 0; i < byteCounts; i += 4)
            {
                int avg = (arr[i] + arr[i + 1] + arr[i + 2]) / 3;
                avg = avg >= Pointcut ? byte.MaxValue : 0;
                arr[i] = arr[i + 1] = arr[i + 2] = (byte)avg;
            }
            Marshal.Copy(arr, 0, p, byteCounts);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #endregion IImageProcessable 成员
    }
}