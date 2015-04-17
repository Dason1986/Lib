using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 变焦模糊
    /// </summary>
    [LanguageDescription("变焦模糊"), LanguageDisplayName("变焦模糊")]
    public class ZoomBlurImage : ImageBuilder
    {
        #region Option
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("偏移"), LanguageDisplayName("偏移"), Category("濾鏡選項")]

        public PointF Offset
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
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("模糊度"), LanguageDisplayName("模糊度"), Category("濾鏡選項")]

        public int Length
        {
            get
            {
                InitOption(); return _opetion.Length;
            }
            set
            {
                InitOption(); _opetion.Length = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class ZoomBlurOption : ImageOption
        {
            /// <summary>
            /// 
            /// </summary> 
            [LanguageDescription("偏移"), LanguageDisplayName("偏移"), Category("濾鏡選項")]
            public PointF Offset { get; set; }
            /// <summary>
            /// 
            /// </summary> 
            [LanguageDescription("模糊度"), LanguageDisplayName("模糊度"), Category("濾鏡選項")]
            public int Length { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = CreateOption() as ZoomBlurOption;
        }
        private ZoomBlurOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ZoomBlurOption == false) throw new ImageException("Opetion is not BlindsOption");
                _opetion = value as ZoomBlurOption;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new ZoomBlurOption { Length = 10 };
        }

        #endregion

        int m_length;
        double m_offset_x;
        double m_offset_y;
        int m_fcx, m_fcy;
        const int RADIUS_LENGTH = 64;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {

            int width = Source.Width;
            int height = Source.Height;
            var clone = (Bitmap)this.Source.Clone();
            m_length = (Length >= 1) ? Length : 1;
            m_offset_x = (Offset.X > 2.0 ? 2.0 : (Offset.X < -2.0 ? 0 : Offset.X));
            m_offset_y = (Offset.Y > 2.0 ? 2.0 : (Offset.Y < -2.0 ? 0 : Offset.Y));
            m_fcx = (int)(width * m_offset_x * 32768.0) + (width * 32768);

            m_fcy = (int)(height * m_offset_y * 32768.0) + (height * 32768);

            const int ta = 255;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int sr = 0, sg = 0, sb = 0, sa = 0;
                    var point = clone.GetPixel(x, y);
                    sr = point.R * ta;
                    sg = point.G * ta;
                    sb = point.B * ta;
                    sa += ta;
                    int fx = (x * 65536) - m_fcx;
                    int fy = (y * 65536) - m_fcy;
                    for (int i = 0; i < RADIUS_LENGTH; i++)
                    {
                        fx = fx - (fx / 16) * m_length / 1024;
                        fy = fy - (fy / 16) * m_length / 1024;

                        int u = (fx + m_fcx + 32768) / 65536;
                        int v = (fy + m_fcy + 32768) / 65536;
                        if (u < 0 || u >= width || v < 0 || v >= height) continue;
                        var tmppoint = clone.GetPixel(u, v);
                        sr += tmppoint.R * ta;
                        sg += tmppoint.G * ta;
                        sb += tmppoint.B * ta;
                        sa += ta;
                    }
                    int r = sr / sa;
                    int g = sg / sa;
                    int b = sb / sa;
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
            int width = Source.Width;
            int height = Source.Height;
            var clone = (Bitmap)this.Source.Clone();
            m_length = (Length >= 1) ? Length : 1;
            m_offset_x = (Offset.X > 2.0 ? 2.0 : (Offset.X < -2.0 ? 0 : Offset.X));
            m_offset_y = (Offset.Y > 2.0 ? 2.0 : (Offset.Y < -2.0 ? 0 : Offset.Y));
            m_fcx = (int)(width * m_offset_x * 32768.0) + (width * 32768);

            m_fcy = (int)(height * m_offset_y * 32768.0) + (height * 32768);

            const int ta = 255;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = clone.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    int sr = 0, sg = 0, sb = 0, sa = 0;
                    //var point = clone.GetPixel(x, y);
                    int index = y * bmpData.Stride + x * 4;
                    sr = ptr[index + 2] * ta;
                    sg = ptr[index + 1] * ta;
                    sb = ptr[index ] * ta;
                    sa += ta;
                    int fx = (x * 65536) - m_fcx;
                    int fy = (y * 65536) - m_fcy;
                    for (int i = 0; i < RADIUS_LENGTH; i++)
                    {
                        fx = fx - (fx / 16) * m_length / 1024;
                        fy = fy - (fy / 16) * m_length / 1024;

                        int u = (fx + m_fcx + 32768) / 65536;
                        int v = (fy + m_fcy + 32768) / 65536;
                        if (u < 0 || u >= width || v < 0 || v >= height) continue;
                        int moveindex = v * bmpData.Stride + u * 4;

                        sr += ptr[moveindex + 2] * ta;
                        sg += ptr[moveindex + 1] * ta;
                        sb += ptr[moveindex] * ta;
                        sa += ta;
                    }
                  

                    int r = sr / sa;
                    int g = sg / sa;
                    int b = sb / sa;
                    ptr[index+2] =this.Truncate(r) ;
                    ptr[index+1] = this.Truncate(g);
                    ptr[index] = this.Truncate(b);
                    // clone.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            clone.UnlockBits(bmpData);
            return clone;
        }
    }
}
