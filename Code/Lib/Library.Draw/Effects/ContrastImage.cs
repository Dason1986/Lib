using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 对比度
    /// </summary>
    public class ContrastImage : ImageBuilder
    {/// <summary>
        /// 对比度
        /// </summary>
        public double Contrast
        {
            get
            {
                InitOption();
                return _opetion.Contrast;
            }
            set
            {
                InitOption();
                _opetion.Contrast = value;
            }
        }

        #region Option

        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new ContrastOption();
        }
        private ContrastOption _opetion;


        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is ContrastOption == false) throw new ImageException("Opetion is not ContrastOption");
                _opetion = value as ContrastOption;
            }
        }
        public class ContrastOption : ImageOption
        {
            public double Contrast { get; set; }
        }
        public override ImageOption CreateOption()
        {
            return new ContrastOption();
        }
        #endregion
        #region Process
        public override Image ProcessBitmap()
        {



            var sourceImage = Source.Clone() as Bitmap;
            var tmpContrast = Contrast;
            if (tmpContrast < -100)
            {
                tmpContrast = -100;
            }
            else if (tmpContrast > 100)
            {
                tmpContrast = 100;
            }
            tmpContrast = (100.0 + tmpContrast) / 100.0;
            tmpContrast *= tmpContrast;

            int height = sourceImage.Height;
            int width = sourceImage.Width;

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var pixelValue = sourceImage.GetPixel(column,row );


                    double pixelR = pixelValue.R / 255.0;
                    pixelR -= 0.5;

                    pixelR *= tmpContrast;
                    pixelR += 0.5;
                    pixelR *= 255;
                    if (pixelR < 0) pixelR = 0;
                    if (pixelR > 255) pixelR = 255;

                    double pixelG = pixelValue.G / 255.0;
                    pixelG -= 0.5;
                    pixelG *= tmpContrast;
                    pixelG += 0.5;
                    pixelG *= 255;
                    if (pixelG < 0) pixelG = 0;
                    if (pixelG > 255) pixelG = 255;

                    double pixelB = pixelValue.B / 255.0;
                    pixelB -= 0.5;
                    pixelB *= tmpContrast;
                    pixelB += 0.5;
                    pixelB *= 255;
                    if (pixelB < 0) pixelB = 0;
                    if (pixelB > 255) pixelB = 255;


                    sourceImage.SetPixel(column, row, Color.FromArgb(pixelValue.A, (int)pixelR, (int)pixelG, (int)pixelB));

                }
            }
            return sourceImage;

        }


        public override unsafe Image UnsafeProcessBitmap()
        {
            throw new NotImplementedException();
            var bmp = Source.Clone() as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    double pixelR = ptr[2] / 255.0;
                    pixelR -= 0.5;

                    pixelR *= Contrast;
                    pixelR += 0.5;
                    pixelR *= 255;
                    if (pixelR < 0) pixelR = 0;
                    if (pixelR > 255) pixelR = 255;

                    double pixelG = ptr[1] / 255.0;
                    pixelG -= 0.5;
                    pixelG *= Contrast;
                    pixelG += 0.5;
                    pixelG *= 255;
                    if (pixelG < 0) pixelG = 0;
                    if (pixelG > 255) pixelG = 255;

                    double pixelB = ptr[0] / 255.0;
                    pixelB -= 0.5;
                    pixelB *= Contrast;
                    pixelB += 0.5;
                    pixelB *= 255;
                    if (pixelB < 0) pixelB = 0;
                    if (pixelB > 255) pixelB = 255;
                    ptr[2] = (byte)pixelR;
                    ptr[1] = (byte)pixelG;
                    ptr[0] = (byte)pixelB;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        #endregion
    }
}