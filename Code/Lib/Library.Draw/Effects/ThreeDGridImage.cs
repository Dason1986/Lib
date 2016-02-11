using Library.Att;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 绿色
    /// </summary>
    [LanguageDescription("格子"), LanguageDisplayName("格子")]
    public class ThreeDGridImage : ImageBuilder
    {
        #region Option

        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("格子大小，值越大码越格越大"), LanguageDisplayName("格子大小"), Category("濾鏡選項")]
        public int GridSize
        {
            get
            {
                InitOption();
                return _opetion.GridSize;
            }
            set
            {
                InitOption();
                _opetion.GridSize = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("連線深度"), LanguageDisplayName("連線深度"), Category("濾鏡選項")]
        public int Depth
        {
            get
            {
                InitOption();
                return _opetion.Depth;
            }
            set
            {
                InitOption();
                _opetion.Depth = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ThreeDGridOption();
        }

        private ThreeDGridOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ThreeDGridOption == false) throw new ImageException("Opetion is not ThreeDGridOption");
                _opetion = (ThreeDGridOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class ThreeDGridOption : ImageOption
        {
            /// <summary>
            /// 粒度
            /// </summary>
            /// <remarks>效果粒度，值越大码越严重</remarks>
            [LanguageDescription("格子大小，值越大码越格越大"), LanguageDisplayName("格子大小"), Category("濾鏡選項")]
            public int GridSize { get; set; }

            /// <summary>
            /// 粒度
            /// </summary>
            /// <remarks>效果粒度，值越大码越严重</remarks>
            [LanguageDescription("連線深度"), LanguageDisplayName("連線深度"), Category("濾鏡選項")]
            public int Depth { get; set; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ThreeDGridOption() { GridSize = 50, Depth = 10 };
        }

        #endregion Option

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var size = GridSize <= 0 ? 10 : GridSize;
            var depth = Depth <= 0 ? 2 : Depth;
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int widht = bmp.Width;
            int r, g, b;
            for (int x = 0; x < widht; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var color = bmp.GetPixel(x, y);
                    r = color.R;
                    g = color.G;
                    b = color.B;

                    int d = 0;
                    if (((y - 1) % size == 0) && (x % size > 0) && ((x + 1) % size > 0))
                        d = -depth; // top
                    else if (((y + 2) % size == 0) && (x % size > 0) && ((x + 1) % size > 0))
                        d = depth; // bottom
                    else if (((x - 1) % size == 0) && (y % size > 0) && ((y + 1) % size) > 0)
                        d = depth; // left
                    else if (((x + 2) % size == 0) && (y % size > 0) && ((y + 1) % size) > 0)
                        d = -depth; // right

                    bmp.SetPixel(x, y, Color.FromArgb(Truncate(r + d), Truncate(g + d), Truncate(b + d)));
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
            var size = GridSize <= 0 ? 10 : GridSize;
            var depth = Depth <= 0 ? 2 : Depth;
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            int r, g, b;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    r = ptr[2];
                    g = ptr[1];
                    b = ptr[0];

                    int d = 0;
                    if (((y - 1) % size == 0) && (x % size > 0) && ((x + 1) % size > 0))
                        d = -depth; // top
                    else if (((y + 2) % size == 0) && (x % size > 0) && ((x + 1) % size > 0))
                        d = depth; // bottom
                    else if (((x - 1) % size == 0) && (y % size > 0) && ((y + 1) % size) > 0)
                        d = depth; // left
                    else if (((x + 2) % size == 0) && (y % size > 0) && ((y + 1) % size) > 0)
                        d = -depth; // right
                    ptr[2] = Truncate(r + d);//B
                    ptr[1] = Truncate(g + d);//G
                    ptr[0] = Truncate(b + d);//R
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}