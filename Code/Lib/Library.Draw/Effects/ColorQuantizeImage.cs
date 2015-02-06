using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 
    /// </summary>
    [LanguageDescription("ɫ��"), LanguageDisplayName("ɫ��")]
    public class ColorQuantizeImage : ImageBuilder
    {
        /// <summary>
        /// �ȼ�
        /// </summary>
        [LanguageDescription("�ȼ�"), LanguageDisplayName("�ȼ�"), Category("�V�R�x�")]

        public float Levels
        {
            get
            {
                InitOption();
                return _opetion.Levels;
            }
            set
            {
                InitOption();
                _opetion.Levels = value;
            }
        }
        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ColorQuantizeOption();
        }
        private ColorQuantizeOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ColorQuantizeOption == false) throw new ImageException("Opetion is not ColorQuantizeOption");
                _opetion = (ColorQuantizeOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class ColorQuantizeOption : ImageOption
        { /// <summary>
            /// �ȼ�
            /// </summary>
            [LanguageDescription("�ȼ�"), LanguageDisplayName("�ȼ�"), Category("�V�R�x�")]
            public float Levels { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new ColorQuantizeOption() { Levels = 5 };
        }
        #endregion
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            float levels = Levels;


            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    int r = pixelValue.R;
                    int g = pixelValue.G;
                    int b = pixelValue.B;
                    float quanR = (((float)((int)(r * 0.003921569f * levels))) / levels) * 255f;
                    float quanG = (((float)((int)(g * 0.003921569f * levels))) / levels) * 255f;
                    float quanB = (((float)((int)(b * 0.003921569f * levels))) / levels) * 255f;
                    r = Truncate((int)quanR);
                    g = Truncate((int)quanG);
                    b = Truncate((int)quanB);
                    bmp.SetPixel(column, row, Color.FromArgb(r, g, b));
                }
            }
            return bmp;
        }

        public override unsafe Image UnsafeProcessBitmap()
        {

            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            float levels = Levels;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {

                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    float quanR = (((float)((int)(r * 0.003921569f * levels))) / levels) * 255f;
                    float quanG = (((float)((int)(g * 0.003921569f * levels))) / levels) * 255f;
                    float quanB = (((float)((int)(b * 0.003921569f * levels))) / levels) * 255f;
                    r = Truncate((int)quanR);
                    g = Truncate((int)quanG);
                    b = Truncate((int)quanB);
                    ptr[2] = (byte)r;
                    ptr[1] = (byte)g;
                    ptr[0] = (byte)b;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}