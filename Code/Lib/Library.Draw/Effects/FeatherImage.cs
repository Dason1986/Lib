using Library.Att;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// ∑…“›
    /// </summary>
    [LanguageDescription("∑…“›"), LanguageDisplayName("∑…“›")]
    public class FeatherImage : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("∑…“›÷µ"), LanguageDisplayName("∑…“›÷µ"), Category("ûVÁRﬂxÌó")]
        public float FeatherValue
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

        #endregion Option

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            byte r, g, b;

            int width = Source.Width;
            int height = Source.Height;
            var clone = (Bitmap)this.Source.Clone();

            int ratio = width > height ? height * 32768 / width : width * 32768 / height;

            // Calculate center, min and max
            int cx = width >> 1;
            int cy = height >> 1;
            int max = cx * cx + cy * cy;
            int min = (int)(max * (1 - FeatherValue));
            int diff = max - min;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var point = clone.GetPixel(x, y);
                    r = point.R;
                    g = point.G;
                    b = point.B;

                    // Calculate distance to center and adapt aspect ratio
                    int dx = cx - x;
                    int dy = cy - y;
                    if (width > height)
                    {
                        dx = (dx * ratio) >> 15;
                    }
                    else
                    {
                        dy = (dy * ratio) >> 15;
                    }
                    float distSq = dx * dx + dy * dy;
                    int v = (int)((distSq / diff) * 255);
                    r = Truncate(r + (v));
                    g = Truncate(g + (v));
                    b = Truncate(b + (v));

                    clone.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return clone;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            byte r, g, b;

            int width = Source.Width;
            int height = Source.Height;
            var clone = (Bitmap)this.Source.Clone();

            int ratio = width > height ? height * 32768 / width : width * 32768 / height;

            // Calculate center, min and max
            int cx = width >> 1;
            int cy = height >> 1;
            int max = cx * cx + cy * cy;
            int min = (int)(max * (1 - FeatherValue));
            int diff = max - min;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = clone.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    r = ptr[2];
                    g = ptr[1];
                    b = ptr[0];

                    // Calculate distance to center and adapt aspect ratio
                    int dx = cx - x;
                    int dy = cy - y;
                    if (width > height)
                    {
                        dx = (dx * ratio) >> 15;
                    }
                    else
                    {
                        dy = (dy * ratio) >> 15;
                    }
                    float distSq = dx * dx + dy * dy;
                    int v = (int)((distSq / diff) * 255);
                    r = Truncate(r + (v));
                    g = Truncate(g + (v));
                    b = Truncate(b + (v));
                    ptr[2] = r;
                    ptr[1] = g;
                    ptr[0] = b;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            clone.UnlockBits(bmpData);
            return clone;
        }
    }
}