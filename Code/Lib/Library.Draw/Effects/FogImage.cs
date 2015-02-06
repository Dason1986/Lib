using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 雾化效果
    /// 图像的雾化处理不是基于图像中像素点之间的计算,而是给图像像素的颜色值引入一定的随机值, 
    /// 使图像具有毛玻璃带水雾般的效果..
    /// </summary>
    [LanguageDescription("对比度"), LanguageDisplayName("雾化效果")]
    public class FogImage : ImageBuilder
    {
        /*
         * 对每个像素A(i,j)进行处理，用其周围一定范围内随机点A(i+d,j+d),(-k<d<k)的像素替代。显然，以该点为圆心的圆半径越大，则雾化效果越明显。
         */
        /// <summary>
        /// 圆半徑
        /// </summary> 
        [LanguageDescription("圆半徑"), LanguageDisplayName("圆半徑"), Category("濾鏡選項")]
        
        public int Fog
        {
            get
            {
                InitOption();
                return _opetion.Fog;
            }
            set
            {
                InitOption();
                _opetion.Fog = value;
            }
        }
        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new FogOption();
        }
        private FogOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is FogOption == false) throw new ImageException("Opetion is not FogOption");
                _opetion = (FogOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class FogOption : ImageOption
        {
            /// <summary>
            /// 圆半徑
            /// </summary>
            [LanguageDescription("圆半徑"), LanguageDisplayName("圆半徑"), Category("濾鏡選項")]
            public int Fog { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new FogOption();
        }
        #endregion
        #region Process
        #region IImageProcessable 成员

        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Random rnd = new Random();
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    int k = rnd.Next(-12345, 12345);
                    //像素块大小
                    int dx = x + k % 7;
                    int dy = y + k % 7;
                    //处理溢出
                    if (dx >= width)
                        dx = width - 1;
                    if (dy >= height)
                        dy = height - 1;
                    if (dx < 0)
                        dx = 0;
                    if (dy < 0)
                        dy = 0;

                    Color c1 = bmp.GetPixel(dx, dy);
                    bmp.SetPixel(x, y, c1);
                }
            }
            return bmp;
        }

        #endregion

        #region IImageProcessable 成员

        public override unsafe Image UnsafeProcessBitmap()
        {
            //    throw new NotImplementedException();
            var n = Fog == 0 ? 7 : Fog;
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            Random rnd = new Random();
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    int k = rnd.Next(-12345, 12345);
                    //像素块大小 常量N的大小决定雾化模糊度
                    int dj = j + k % n; //水平向右方向像素偏移后
                    int di = i + k % n; //垂直向下方向像素偏移后
                    if (dj >= width) dj = width - 1;
                    if (di >= height) di = height - 1;
                    if (di < 0)
                        di = 0;
                    if (dj < 0)
                        dj = 0;
                    //针对Format32bppArgb格式像素，指针偏移量为4的倍数 4*dj  4*di
                    //g(i,j)=f(di,dj)
                    ptr[bmpData.Stride * i + j * 4 + 0] = ptr[bmpData.Stride * di + dj * 4 + 0]; //B
                    ptr[bmpData.Stride * i + j * 4 + 1] = ptr[bmpData.Stride * di + dj * 4 + 1]; //G
                    ptr[bmpData.Stride * i + j * 4 + 2] = ptr[bmpData.Stride * di + dj * 4 + 2]; //R
                    // ptr += 4;  注意此处指针没移动，始终以bmpData.Scan0开始
                }
                //  ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }




        #endregion

        #endregion
    }
}