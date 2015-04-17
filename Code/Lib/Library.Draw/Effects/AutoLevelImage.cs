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
    [LanguageDescription("直方图模式增强"), LanguageDisplayName("直方图模式增强")]
    public class AutoLevelImage : ImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1f)]
        [LanguageDescription("强度"), LanguageDisplayName("强度"), Category("濾鏡選項")]
        public float Intensity
        {
            get
            {
                InitOption();
                return _opetion.Intensity;
            }
            set
            {
                InitOption();
                _opetion.Intensity = value;
            }
        }
        #region Option
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new AutoLevelOption();
        }
        private AutoLevelOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is AutoLevelOption == false) throw new ImageException("Opetion is not AutoLevelOption");
                _opetion = (AutoLevelOption)value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class AutoLevelOption : ImageOption
        {
            /// <summary>
            /// 
            /// </summary>
            [LanguageDescription("强度"), LanguageDisplayName("强度"), Category("濾鏡選項")]
            public float Intensity { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new AutoLevelOption() { Intensity = 1f };
        }
        #endregion
        private static float[] ComputeGamma(int[] lo, int[] md, int[] hi)
        {
            float[] array = new float[3];
            for (int i = 0; i < 3; i++)
            {
                if (lo[i] < md[i] && md[i] < hi[i])
                {
                    double log = Math.Log((double)(((float)(md[i] - lo[i])) / ((float)(hi[i] - lo[i]))));
                    array[i] = (log > 10.0) ? ((float)10.0) : ((log < 0.1) ? ((float)0.1) : ((float)log));
                }
                else
                {
                    array[i] = 1f;
                }
            }
            return array;
        }

        int[] GetMeanColor(int[,] h)
        {
            float[] array = new float[3];
            for (int i = 0; i < 3; i++)
            {
                long sum1 = 0L;
                long sum2 = 0L;
                for (int j = 0; j < 256; j++)
                {
                    sum1 += j * h[i, j];
                    sum2 += h[i, j];
                }
                array[i] = (sum2 == 0L) ? 0f : (((float)sum1) / ((float)sum2));
            }
            return new int[] { (((int)(array[0] + 0.5f)) & 255), (((int)(array[1] + 0.5f)) & 255), (((int)(array[2] + 0.5f)) & 255) };
        }

        int[] GetPercentileColor(int[,] h, float fraction)
        {
            int[] array = new int[3];
            for (int i = 0; i < 3; i++)
            {
                long sum1 = 0L;
                long sum2 = 0L;
                for (int j = 0; j < 256; j++)
                {
                    sum2 += h[i, j];
                }
                for (int k = 0; k < 256; k++)
                {
                    sum1 += h[i, k];
                    if (sum1 > (sum2 * fraction))
                    {
                        array[i] = k;
                        break;
                    }
                }
            }
            return array;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            var bmp = Source.Clone() as Bitmap;
            int height = bmp.Height;
            int width = bmp.Width;
            int[,] h = new int[3, 256];
            int[] array = new int[3];
            int[] rgb = new int[] { 255, 255, 255 };
            int[] bb = new int[256];
            int[] gg = new int[256];
            int[] rr = new int[256];
            int intensity = (int)(this.Intensity * 255f);
            int intensity_invert = 255 - intensity;
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    var pixelValue = bmp.GetPixel(x, y);
                    h[0, pixelValue.R]++;
                    h[1, pixelValue.G]++;
                    h[2, pixelValue.B]++;
                }
            }
            int[] percentileColor = GetPercentileColor(h, 0.005f);
            int[] meanColor = GetMeanColor(h);
            int[] hi = GetPercentileColor(h, 0.995f);
            float[] gamma = ComputeGamma(percentileColor, meanColor, hi);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    int[] arr = new int[3];
                    for (int n = 0; n < 3; n++)
                    {
                        float percent = j - percentileColor[n];
                        if (percent < 0f)
                        {
                            arr[n] = array[n];
                        }
                        else if ((percent + percentileColor[n]) >= hi[n])
                        {
                            arr[n] = rgb[n];
                        }
                        else
                        {
                            double adjust = array[n] + ((rgb[n] - array[n]) * Math.Pow((double)(percent / ((float)(hi[n] - percentileColor[n]))), (double)gamma[n]));
                            arr[n] = Truncate((int)adjust);
                        }
                    }
                    rr[j] = arr[0];
                    gg[j] = arr[1];
                    bb[j] = arr[2];
                }
            }

            int r, g, b;
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    var pixelValue = bmp.GetPixel(x, y);
                    r = pixelValue.R;
                    g = pixelValue.G;
                    b = pixelValue.B;
                    r = (r * intensity_invert + rr[r] * intensity) >> 8;
                    g = (g * intensity_invert + gg[g] * intensity) >> 8;
                    b = (b * intensity_invert + bb[b] * intensity) >> 8;
                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return bmp;//做直方图模式增强
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
            int[,] h = new int[3, 256];
            int[] array = new int[3];
            int[] rgb = new int[] { 255, 255, 255 };
            int[] bb = new int[256];
            int[] gg = new int[256];
            int[] rr = new int[256];
            int intensity = (int)(this.Intensity * 255f);
            int intensity_invert = 255 - intensity;

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);

            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {

                    h[0, ptr[2]]++;
                    h[1, ptr[1]]++;
                    h[2, ptr[0]]++;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            int[] percentileColor = GetPercentileColor(h, 0.005f);
            int[] meanColor = GetMeanColor(h);
            int[] hi = GetPercentileColor(h, 0.995f);
            float[] gamma = ComputeGamma(percentileColor, meanColor, hi);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    int[] arr = new int[3];
                    for (int n = 0; n < 3; n++)
                    {
                        float percent = j - percentileColor[n];
                        if (percent < 0f)
                        {
                            arr[n] = array[n];
                        }
                        else if ((percent + percentileColor[n]) >= hi[n])
                        {
                            arr[n] = rgb[n];
                        }
                        else
                        {
                            double adjust = array[n] + ((rgb[n] - array[n]) * Math.Pow((double)(percent / ((float)(hi[n] - percentileColor[n]))), (double)gamma[n]));
                            arr[n] = Truncate((int)adjust);
                        }
                    }
                    rr[j] = arr[0];
                    gg[j] = arr[1];
                    bb[j] = arr[2];
                }
            }
            ptr = (byte*)(bmpData.Scan0);
            int r, g, b;
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    r = ptr[2];
                    g = ptr[1];
                    b = ptr[0];
                    r = (r * intensity_invert + rr[r] * intensity) >> 8;
                    g = (g * intensity_invert + gg[g] * intensity) >> 8;
                    b = (b * intensity_invert + bb[b] * intensity) >> 8;
                    ptr[2] = (byte)r;
                    ptr[1] = (byte)g;
                    ptr[0] = (byte)b;
                    ptr += 4;
                }
                ptr += bmpData.Stride - width * 4;
            }
            bmp.UnlockBits(bmpData);
            return bmp;//做直方图模式增强
        }
    }
}