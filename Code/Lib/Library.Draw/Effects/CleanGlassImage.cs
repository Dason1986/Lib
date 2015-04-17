using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;
using Library.HelperUtility;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 
    /// </summary> 
    [LanguageDescription("对比度/亮度"), LanguageDisplayName("对比度/亮度")]
    public class CleanGlassImage : ImageBuilder
    {  /// <summary>
        /// Should be in the range [0, 1].
        /// </summary>
        [LanguageDescription("尺寸 [0, 1]"), LanguageDisplayName("尺寸"), Category("濾鏡選項")]
        public float Size
        {
            get
            {
                InitOption();
                return _opetion.Value;
            }
            set
            {
                InitOption();
                _opetion.Value = value;
            }
        }

        #region Option
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ValueOption();
        }
        private ValueOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ValueOption == false) throw new ImageException("Opetion is not ValueOption");
                _opetion = (ValueOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ValueOption() { Value = 0.5f };
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;

            int ratio = width > height ? height * 32768 / width : width * 32768 / height;

            // Calculate center, min and max
            int cx = width >> 1;
            int cy = width >> 1;
            int max = cx * cx + cy * cy;
            int min = (int)(max * (1 - Size));

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    // Calculate distance to center and adapt aspect ratio
                    int dx = cx - column;
                    int dy = cy - row;
                    if (width > height)
                    {
                        dy = (dy * ratio) >> 14;
                    }
                    else
                    {
                        dx = (dx * ratio) >> 14;
                    }
                    int distSq = dx * dx + dy * dy;

                    if (distSq <= min) continue;
                    int k = ObjectUtility.GetRandomInt(1, 123456);
                    //像素块大小
                    int pixeldx = column + k % 19;
                    int pixeldy = height + k % 19;
                    if (pixeldx >= width)
                    {
                        pixeldx = width - 1;
                    }
                    if (pixeldy >= height)
                    {
                        pixeldy = height - 1;
                    }
                    var pixelValue = bmp.GetPixel(pixeldx, pixeldy);
                    int r = pixelValue.R;
                    int g = pixelValue.G;
                    int b = pixelValue.B;
                    bmp.SetPixel(column, row, Color.FromArgb(r, g, b));
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
            int height = bmp.Height;
            int width = bmp.Width;

            int ratio = width > height ? height * 32768 / width : width * 32768 / height;

            // Calculate center, min and max
            int cx = width >> 1;
            int cy = width >> 1;
            int max = cx * cx + cy * cy;
            int min = (int)(max * (1 - Size));

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    // Calculate distance to center and adapt aspect ratio
                    int dx = cx - column;
                    int dy = cy - row;
                    if (width > height)
                    {
                        dy = (dy * ratio) >> 14;
                    }
                    else
                    {
                        dx = (dx * ratio) >> 14;
                    }
                    int distSq = dx * dx + dy * dy;

                    if (distSq <= min) continue;
                    int k = ObjectUtility.GetRandomInt(1, 123456);
                    //像素块大小
                    int pixeldx = column + k % 19;
                    int pixeldy = height + k % 19;
                    if (pixeldx >= width)
                    {
                        pixeldx = width - 1;
                    }
                    if (pixeldy >= height)
                    {
                        pixeldy = height - 1;
                    }
                    int index = row * bmpData.Stride + column * 4;
                    int moveindex = pixeldy * bmpData.Stride + pixeldx * 4;

                    ptr[index + 2] = ptr[moveindex + 2];
                    ptr[index + 1] = ptr[moveindex + 1];
                    ptr[index] = ptr[moveindex];
                    //  bmp.SetPixel(column, row, Color.FromArgb(r, g, b));
                }
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}