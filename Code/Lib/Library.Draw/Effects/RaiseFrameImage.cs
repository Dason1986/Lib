using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// ߅��
    /// </summary>
    [LanguageDescription(""), LanguageDisplayName("߅��")]
    public class RaiseFrameImage : ImageBuilder
    {
        /// <summary>
        /// ��߅��ɫ
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
        public Color LeftColor
        {
            get
            {
                InitOption();
                return _opetion.LeftColor;
            }
            set
            {
                InitOption();
                _opetion.LeftColor = value;
            }
        }
        /// <summary>
        /// ��߅��ɫ
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
        public Color TopColor
        {
            get
            {
                InitOption();
                return _opetion.TopColor;
            }
            set
            {
                InitOption();
                _opetion.TopColor = value;
            }
        }
        /// <summary>
        /// ��߅��ɫ
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
        public Color RightColor
        {
            get
            {
                InitOption();
                return _opetion.RightColor;
            }
            set
            {
                InitOption();
                _opetion.RightColor = value;
            }
        }
        /// <summary>
        /// ��߅��ɫ
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
        public Color BottomColor
        {
            get
            {
                InitOption();
                return _opetion.BottomColor;
            }
            set
            {
                InitOption();
                _opetion.BottomColor = value;
            }
        }
        /// <summary>
        /// ߅��ɫ͸����
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("߅��ɫ͸����"), Category("�V�R�x�")]
        public byte Alpha
        {
            get
            {
                InitOption();
                return _opetion.Alpha;
            }
            set
            {
                InitOption();
                _opetion.Alpha = value;
            }
        }
        /// <summary>
        /// ߅���С
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("߅���С"), Category("�V�R�x�")]
        public byte Border
        {
            get
            {
                InitOption();
                return _opetion.Border;
            }
            set
            {
                InitOption();
                _opetion.Border = value;
            }
        }
        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new RaiseFrameOption();
        }
        private RaiseFrameOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is RaiseFrameOption == false) throw new ImageException("Opetion is not RaiseFrameOption");
                _opetion = (RaiseFrameOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class RaiseFrameOption : ImageOption
        {
            /// <summary>
            /// ��߅��ɫ
            /// </summary>
            [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
            public Color LeftColor { get; set; }
            /// <summary>
            /// ��߅��ɫ
            /// </summary>
            [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
            public Color TopColor { get; set; }
            /// <summary>
            /// ��߅��ɫ
            /// </summary>
            [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
            public Color RightColor { get; set; }
            /// <summary>
            /// ��߅��ɫ
            /// </summary>
            [LanguageDescription(""), LanguageDisplayName("��߅��ɫ"), Category("�V�R�x�")]
            public Color BottomColor { get; set; }
            /// <summary>
            /// ߅��ɫ͸����
            /// </summary>
            [LanguageDescription(""), LanguageDisplayName("߅��ɫ͸����"), Category("�V�R�x�")]
            public byte Alpha { get; set; }
            /// <summary>
            /// ߅���С
            /// </summary>
            [LanguageDescription(""), LanguageDisplayName("߅���С"), Category("�V�R�x�")]
            public byte Border { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new RaiseFrameOption()
            {
                LeftColor = Color.FromArgb(255, 255, 65), // left
                TopColor = Color.FromArgb(255, 255, 120),// top
                RightColor = Color.FromArgb(0, 0, 65), // right
                BottomColor = Color.FromArgb(0, 0, 120), // bottom
                Alpha = 100,
                Border = 5
            };
        }
        #endregion
        public override Image ProcessBitmap()
        {

            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            int a = this.Alpha;
            var _size = this.Border;



            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int r = 255 - c.R;
                    int g = 255 - c.G;
                    int b = 255 - c.B;

                    Color cr;
                    if ((x < _size) && (y < height - x) && (y >= x))
                        cr = this.LeftColor; // left
                    else if ((y < _size) && (x < width - y) && (x >= y))
                        cr = TopColor; // top
                    else if ((x > width - _size) && (y >= width - x) && (y < height + x - width))
                        cr = RightColor; // right
                    else if (y > height - _size)
                        cr = BottomColor; // bottom
                    else
                        continue;

                   var pp= (double)x/width*255;
                   int t = 0xFF - a + (int)pp;
                    bmp.SetPixel(x, y, Color.FromArgb((cr.R * a + r * t) / 0xFF, (cr.G * a + g * t) / 0xFF, (cr.B * a + b * t) / 0xFF));
                }
            }

            return bmp;
        }

        public override unsafe Image UnsafeProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            int a = this.Alpha;
            var _size = this.Border;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    int r = 255 - ptr[0];
                    int g = 255 - ptr[1];
                    int b = 255 - ptr[2];

                    Color cr;
                    if ((x < _size) && (y < height - x) && (y >= x))
                        cr = this.LeftColor; // left
                    else if ((y < _size) && (x < width - y) && (x >= y))
                        cr = TopColor; // top
                    else if ((x > width - _size) && (y >= width - x) && (y < height + x - width))
                        cr = RightColor; // right
                    else if (y > height - _size)
                        cr = BottomColor; // bottom
                    else
                    {
                        ptr += 4;
                        continue;
                    }


                
                    var pp = (double)x / width * 255;
                    int t = 0xFF - a + (int)pp;
                    ptr[0] = this.Truncate((cr.B * a + b * t) / 0xFF);//B
                    ptr[1] = this.Truncate((cr.G * a + g * t) / 0xFF);//G
                    ptr[2] = this.Truncate((cr.R * a + r * t) / 0xFF);//R
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}