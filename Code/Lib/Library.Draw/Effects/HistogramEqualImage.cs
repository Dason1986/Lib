using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 
    /// </summary>
    [LanguageDescription("该方法从图象的灰度概率域入手，将概率谱分为两个等面积的集群，通过判定群间距离检测图象的边缘。"), LanguageDisplayName("灰度概率域")]
    public class HistogramEqualImage : ImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1f)]
        [LanguageDescription("强度对比"), LanguageDisplayName("强度对比"), Category("濾鏡選項")]
        public float ContrastIntensity
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
            return new ValueOption() { Value = 1f };
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
            int r, g, b;
            int[] array = new int[256];
            int[] numArray = new int[height * width];
            int contrast = (int)(this.ContrastIntensity * 255f);
            int pos = 0;
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    r = pixelValue.R;
                    g = pixelValue.G;
                    b = pixelValue.B;
                    int index = (r * 0x1b36 + g * 0x5b8c + b * 0x93e) >> 15;
                    array[index]++;
                    numArray[pos] = index;
                    pos++;
                }
            }
            for (int i = 1; i < 0x100; i++)
            {
                array[i] += array[i - 1];
            }
            for (int i = 0; i < 0x100; i++)
            {
                array[i] = (array[i] << 8) / height * width;
                array[i] = ((contrast * array[i]) >> 8) + (((0xff - contrast) * i) >> 8);
            }
            pos = 0;
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = bmp.GetPixel(column, row);
                    r = pixelValue.R;
                    g = pixelValue.G;
                    b = pixelValue.B;
                    if (numArray[pos] != 0)
                    {
                        int num = array[numArray[pos]];
                        r = (r * num) / numArray[pos];
                        g = (g * num) / numArray[pos];
                        b = (b * num) / numArray[pos];
                        r = Truncate(r);
                        g = Truncate(g);
                        b = Truncate(b);
                    }
                    bmp.SetPixel(column, row, Color.FromArgb(r, g, b));
                    pos++;
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
            int r, g, b;
            int[] array = new int[256];
            int[] numArray = new int[height * width];
            int contrast = (int)(this.ContrastIntensity * 255f);
            int pos = 0;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {


                    r = ptr[2];
                    g = ptr[1];
                    b = ptr[0];
                    ptr += 4;
                    int index = (r * 0x1b36 + g * 0x5b8c + b * 0x93e) >> 15;
                    array[index]++;
                    numArray[pos] = index;
                    pos++;
                }
                ptr += bmpData.Stride - width * 4;
            }
            for (int i = 1; i < 0x100; i++)
            {
                array[i] += array[i - 1];
            }
            for (int i = 0; i < 0x100; i++)
            {
                array[i] = (array[i] << 8) / height * width;
                array[i] = ((contrast * array[i]) >> 8) + (((0xff - contrast) * i) >> 8);
            }
            pos = 0;

            ptr = (byte*)(bmpData.Scan0);
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    r = ptr[2];
                    g = ptr[1];
                    b = ptr[0];
                    if (numArray[pos] != 0)
                    {
                        int num = array[numArray[pos]];
                        r = (r * num) / numArray[pos];
                        g = (g * num) / numArray[pos];
                        b = (b * num) / numArray[pos];
                        r = Truncate(r);
                        g = Truncate(g);
                        b = Truncate(b);
                    }

                    ptr[2] = (byte)r;
                    ptr[1] = (byte)g;
                    ptr[0] = (byte)b;
                    ptr += 4;
                    pos++;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}