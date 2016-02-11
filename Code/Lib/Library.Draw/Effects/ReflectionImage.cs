using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 投影
    /// </summary>
    [LanguageDescription("投影"), LanguageDisplayName("投影")]
    public class ReflectionImage : ImageBuilder
    {
        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("方向"), LanguageDisplayName("方向"), Category("濾鏡選項")]
        public AlignmentType Alignment
        {
            get
            {
                InitOption(); return _opetion.Alignment;
            }
            set
            {
                InitOption(); _opetion.Alignment = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("方向"), LanguageDisplayName("方向"), Category("濾鏡選項")]
        public float Offset
        {
            get
            {
                InitOption(); return _opetion.Offset;
            }
            set
            {
                InitOption(); _opetion.Offset = value;
            }
        }

        #region Option

        /// <summary>
        ///
        /// </summary>
        public class ReflectionOption : ImageOption
        {
            /// <summary>
            ///
            /// </summary>
            [LanguageDescription("方向"), LanguageDisplayName("方向"), Category("濾鏡選項")]
            public AlignmentType Alignment { get; set; }

            /// <summary>
            ///
            /// </summary>
            [LanguageDescription("偏移"), LanguageDisplayName("偏移"), Category("濾鏡選項")]
            public float Offset { get; set; }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ReflectionOption();
        }

        private ReflectionOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ReflectionOption == false) throw new ImageException("Opetion is not ReflectionOption");
                _opetion = (ReflectionOption)value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ReflectionOption() { Alignment = AlignmentType.Horizontally, Offset = 0.5f };
        }

        #endregion Option

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            int r, g, b;

            switch (Alignment)
            {
                case AlignmentType.Horizontally:
                    {
                        var bmp = Source.Clone() as Bitmap;
                        int height = bmp.Height;
                        int width = bmp.Width;

                        int start;
                        int limit;
                        int y_offset = (int)(this.Offset * height);
                        if (this.Offset > 0.5f)
                        {
                            start = y_offset - (height - y_offset);
                            limit = y_offset;
                        }
                        else
                        {
                            start = y_offset;
                            limit = y_offset + y_offset;
                        }
                        if (start < 0)
                        {
                            start = 0;
                        }
                        for (int y = start; (y < limit) && (y < height); y++)
                        {
                            int y_pos = (-y + (2 * y_offset)) - 1;
                            y_pos = (y_pos < 0) ? 0 : (y_pos >= height ? height - 1 : y_pos);
                            for (int x = 0; x < width; x++)
                            {
                                var c = bmp.GetPixel(x, y);
                                r = c.R;
                                g = c.G;
                                b = c.B;

                                bmp.SetPixel(x, y_pos, Color.FromArgb(r, g, b));
                            }
                        }
                        return bmp;
                    }

                case AlignmentType.Vertically:
                    {
                        var bmp = Source.Clone() as Bitmap;
                        int height = bmp.Height;
                        int width = bmp.Width;

                        int start;
                        int limit;
                        int x_offset = (int)(this.Offset * width);
                        if (this.Offset > 0.5f)
                        {
                            start = x_offset - (width - x_offset);
                            limit = x_offset;
                        }
                        else
                        {
                            start = x_offset;
                            limit = x_offset + x_offset;
                        }
                        if (start < 0)
                        {
                            start = 0;
                        }
                        for (int x = start; (x < limit) && (x < width); x++)
                        {
                            int x_pos = (-x + (2 * x_offset)) - 1;
                            x_pos = x_pos < 0 ? 0 : (x_pos >= width ? width - 1 : x_pos);
                            for (int y = 0; y < height; y++)
                            {
                                var c = bmp.GetPixel(x, y);
                                r = c.R;
                                g = c.G;
                                b = c.B;
                                bmp.SetPixel(x_pos, y, Color.FromArgb(r, g, b));
                            }
                        }
                        return bmp;
                    }
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override unsafe Image UnsafeProcessBitmap()
        {
            byte r, g, b;

            switch (Alignment)
            {
                case AlignmentType.Horizontally:
                    {
                        var bmp = Source.Clone() as Bitmap;
                        int height = bmp.Height;
                        int width = bmp.Width;

                        int start;
                        int limit;
                        int y_offset = (int)(this.Offset * height);
                        if (this.Offset > 0.5f)
                        {
                            start = y_offset - (height - y_offset);
                            limit = y_offset;
                        }
                        else
                        {
                            start = y_offset;
                            limit = y_offset + y_offset;
                        }
                        if (start < 0)
                        {
                            start = 0;
                        }
                        Rectangle rect = new Rectangle(0, 0, width, height);
                        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                        byte* ptr = (byte*)(bmpData.Scan0);
                        for (int y = start; (y < limit) && (y < height); y++)
                        {
                            int y_pos = (-y + (2 * y_offset)) - 1;
                            y_pos = (y_pos < 0) ? 0 : (y_pos >= height ? height - 1 : y_pos);
                            for (int x = 0; x < width; x++)
                            {
                                //   var c = bmp.GetPixel(x, y);
                                int moveindex = y * bmpData.Stride + x * 4;
                                int newindex = y_pos * bmpData.Stride + x * 4;
                                r = ptr[moveindex + 2];
                                g = ptr[moveindex + 1];
                                b = ptr[moveindex];

                                ptr[newindex + 2] = r;
                                ptr[newindex + 1] = g;
                                ptr[newindex] = b;
                                // bmp.SetPixel(x, y_pos, Color.FromArgb(r, g, b));
                            }
                        }
                        bmp.UnlockBits(bmpData);
                        return bmp;
                    }

                case AlignmentType.Vertically:
                    {
                        var bmp = Source.Clone() as Bitmap;
                        int height = bmp.Height;
                        int width = bmp.Width;

                        int start;
                        int limit;
                        int x_offset = (int)(this.Offset * width);
                        if (this.Offset > 0.5f)
                        {
                            start = x_offset - (width - x_offset);
                            limit = x_offset;
                        }
                        else
                        {
                            start = x_offset;
                            limit = x_offset + x_offset;
                        }
                        if (start < 0)
                        {
                            start = 0;
                        }
                        Rectangle rect = new Rectangle(0, 0, width, height);
                        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                        byte* ptr = (byte*)(bmpData.Scan0);
                        for (int x = start; (x < limit) && (x < width); x++)
                        {
                            int x_pos = (-x + (2 * x_offset)) - 1;
                            x_pos = x_pos < 0 ? 0 : (x_pos >= width ? width - 1 : x_pos);
                            for (int y = 0; y < height; y++)
                            {
                                //var c = bmp.GetPixel(x, y);
                                //r = c.R;
                                //g = c.G;
                                //b = c.B;
                                //bmp.SetPixel(x_pos, y, Color.FromArgb(r, g, b));
                                int moveindex = y * bmpData.Stride + x * 4;
                                int newindex = y * bmpData.Stride + x_pos * 4;
                                r = ptr[moveindex + 2];
                                g = ptr[moveindex + 1];
                                b = ptr[moveindex];

                                ptr[newindex + 2] = r;
                                ptr[newindex + 1] = g;
                                ptr[newindex] = b;
                            }
                        }
                        bmp.UnlockBits(bmpData);
                        return bmp;
                    }
                default: throw new NotImplementedException();
            }
        }
    }
}