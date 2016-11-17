using Library.Att;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 灰度效果
    /// </summary>
    [LanguageDescription("灰度效果"), LanguageDisplayName("灰度效果")]
    public class GrayImage : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("灰度算法"), LanguageDisplayName("灰度算法"), Category("濾鏡選項")]
        public GrayType Pixel
        {
            get
            {
                InitOption();
                return _opetion.Pixel;
            }
            set
            {
                InitOption();
                _opetion.Pixel = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("灰度算法"), LanguageDisplayName("灰度算法")]
        public enum GrayType
        {
            /// <summary>
            /// 加權
            /// </summary>
            [LanguageDescription("加權"), LanguageDisplayName("加權")]
            Weighted,

            /// <summary>
            /// 平均
            /// </summary>
            [LanguageDescription("平均"), LanguageDisplayName("平均")]
            Average,

            /// <summary>
            /// 最大值
            /// </summary>
            [LanguageDescription("最大值"), LanguageDisplayName("最大值")]
            Max,

            /// <summary>
            /// 位移
            /// </summary>
            [LanguageDescription("位移"), LanguageDisplayName("位移")]
            Shift,

            /// <summary>
            /// 整數
            /// </summary>
            [LanguageDescription("整數"), LanguageDisplayName("整數")]
            Integer,
        }

        #region Option

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new GrayOption();
        }

        private GrayOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is GrayOption == false) throw new ImageException("Opetion is not GrayOption");
                _opetion = (GrayOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class GrayOption : ImageOption
        {
            /// <summary>
            /// 灰度算法
            /// </summary>
            [LanguageDescription("灰度算法"), LanguageDisplayName("灰度算法"), Category("濾鏡選項")]
            public GrayType Pixel { get; set; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new GrayOption();
        }

        #endregion Option

        #region Process

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            for (int i = 0; i < width; i++) //这里如果用i<curBitmap.Width做循环对性能有影响
            {
                for (int j = 0; j < height; j++)
                {
                    Color curColor = bmp.GetPixel(i, j);
                    int ret = 0;
                    switch (Pixel)
                    {
                        case GrayType.Average://（R+G+B）/3; 　
                            ret = (curColor.R + curColor.G + curColor.B) / 3;
                            break;

                        case GrayType.Weighted:
                            ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                            break;

                        case GrayType.Shift://(R*28+G*151+B*77)>>8
                            ret = ((int)(curColor.R * 28 + curColor.G * 151 + curColor.B * 77)) >> 8;
                            break;

                        case GrayType.Integer://(R*30+G*59+B*11)/100 　
                            ret = ((int)(curColor.R * 30 + curColor.G * 59 + curColor.B * 11) / 100);
                            break;

                        case GrayType.Max:
                            ret = curColor.R > curColor.G ? curColor.R : curColor.G;
                            ret = ret > curColor.B ? ret : curColor.B;
                            break;
                    }
                    bmp.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
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
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//curBitmap.PixelFormat
            int w = bmpData.Width;
            int h = bmpData.Height;
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    byte temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);

                    switch (Pixel)
                    {
                        case GrayType.Average:
                            temp = (byte)((ptr[2] + ptr[1] + ptr[0]) / 3);
                            break;

                        case GrayType.Weighted:
                            temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                            break;

                        case GrayType.Shift:
                            temp = (byte)((int)(28 * ptr[2] + 151 * ptr[1] + 77 * ptr[0]) >> 8);
                            break;

                        case GrayType.Integer:
                            temp = (byte)((30 * ptr[2] + 59 * ptr[1] + 11 * ptr[0]) / 100);
                            break;

                        case GrayType.Max:
                            temp = ptr[2] > ptr[1] ? ptr[2] : ptr[1];
                            temp = temp > ptr[0] ? temp : ptr[0];
                            break;
                    }
                    ptr[0] = ptr[1] = ptr[2] = temp;
                    ptr += 3; //Format24bppRgb格式每个像素占3字节
                }
                ptr += bmpData.Stride - bmpData.Width * 3;//每行读取到最后“有用”数据时，跳过未使用空间XX
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #endregion Process
    }
}