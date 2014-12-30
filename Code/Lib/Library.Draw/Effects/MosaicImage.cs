using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 马赛克
    /// 是把一张图片分割成若干个N * N像素的小区块（可能在边缘有零星的小块，但不影响整体算法）
    /// ，每个小区块的颜色都是相同的。
    /// </summary>
    public class MosaicImage : ImageBuilder
    {
        /*
         马赛克算法很简单，说白了就是把一张图片分割成若干个val * val像素的小区块（可能在边缘有零星的小块，但不影响整体算法,val越大，马赛克效果越明显），
         每个小区块的颜色都是相同的。为了方便起见，我们不妨让这个颜色就用该区域最左上角的那个点的颜色。当然还可以有其他方法，比如取区块中间点的颜色，
         或区块中随机点的颜色作代表等等。
         * 
         * 
          当y（当前高度）是val的整数倍时：
 扫描当前行中的每一点x，如果x也是val的整数倍，记录下当前x,y的颜色值；如果x不是val的整数倍，则沿用最近一次被记录的颜色值。
当y不是val的整数倍：
 很简单，直接复制上一行。

因此，区块越大，处理效果越明显；也可得出，源图片(R)对处理后的图片(S)是多对一映射，也就是说：马赛克处理后的图片是不可逆的，不要试图用可逆算法复原。
         */
        public int Granularity
        {
            get
            {
                InitOption();
                return _opetion.Granularity;
            }
            set
            {
                InitOption();
                _opetion.Granularity = value;
            }
        }

        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new MosaicOption();
        }
        private MosaicOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is MosaicOption == false) throw new ImageException("Opetion is not MosaicOption");
                _opetion = value as MosaicOption;
            }
        }
        public class MosaicOption : ImageOption
        {
            public int Granularity { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new MosaicOption();
        }
        #endregion
        #region IImageProcessable 成员

        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            int N = Granularity;//效果粒度，值越大码越严重
            if (N <= 0) throw new ImageException("粒度值不能小於0");
            int r = 0, g = 0, b = 0;
            Color c;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    if (y % N == 0)
                    {
                        if (x % N == 0)//整数倍时，取像素赋值
                        {
                            c = bmp.GetPixel(x, y);
                            r = c.R;
                            g = c.G;
                            b = c.B;
                        }
                        else
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }
                    else //复制上一行
                    {
                        Color colorPreLine = bmp.GetPixel(x, y - 1);
                        bmp.SetPixel(x, y, colorPreLine);

                    }
                }
            }
            return bmp;
        }

        public unsafe override Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            int N = Granularity;//效果粒度，值越大码越严重
            if (N <= 0) throw new ImageException("粒度值不能小於0");
            int r = 0, g = 0, b = 0;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y % N == 0)
                    {
                        if (x % N == 0)
                        {
                            r = ptr[2];
                            g = ptr[1];
                            b = ptr[0];
                        }
                        else
                        {
                            ptr[2] = (byte)r;
                            ptr[1] = (byte)g;
                            ptr[0] = (byte)b;
                        }
                    }
                    else //复制上一行
                    {
                        ptr[0] = ptr[0 - bmpData.Stride];//b;
                        ptr[1] = ptr[1 - bmpData.Stride];//g;
                        ptr[2] = ptr[2 - bmpData.Stride];//r
                    }
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