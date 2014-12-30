using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 黑白效果
    /// </summary>
    public class PixelFunImage : ImageBuilder
    {

        public PixelType Pixel
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
        public enum PixelType
        {
            Weighted,
            Average,
            Max
        }

        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new PixelOption();
        }
        private PixelOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is PixelOption == false) throw new ImageException("Opetion is not PixelOption");
                _opetion = value as PixelOption;
            }
        }
        public class PixelOption : ImageOption
        {
            public PixelType Pixel { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new PixelOption();
        }
        #endregion
        #region Process
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
                        case PixelType.Average:
                            ret = (curColor.R + curColor.G + curColor.B) / 3;
                            break;
                        case PixelType.Weighted:
                            ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                            break;
                        case PixelType.Max:
                            ret = curColor.R > curColor.G ? curColor.R : curColor.G;
                            ret = ret > curColor.B ? ret : curColor.B;
                            break;
                    }
                    bmp.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
            return bmp;
        }
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
                        case PixelType.Average:
                            temp = (byte)((ptr[2] + ptr[1] + ptr[0]) / 3);
                            break;
                        case PixelType.Weighted:
                            temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);//(int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                            break;
                        case PixelType.Max:
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
        #endregion
    }
}