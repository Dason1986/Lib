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
    /// 高斯模板
    /// </summary>
    [LanguageDescription("高斯模板"), LanguageDisplayName("高斯模板")]
    public class GaussianBlurImage : ImageBuilder
    {
        #region Option

        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("模糊度"), LanguageDisplayName("模糊度"), Category("濾鏡選項")]
        public float Sigma
        {
            get
            {
                InitOption(); return _opetion.Sigma;
            }
            set
            {
                InitOption(); _opetion.Sigma = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class GaussianBlurOption : ImageOption
        {

            /// <summary>
            /// 
            /// </summary> 
            [LanguageDescription("模糊度"), LanguageDisplayName("模糊度"), Category("濾鏡選項")]
            public float Sigma { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = CreateOption() as GaussianBlurOption;
        }
        private GaussianBlurOption _opetion;

        /// <summary>
        /// 
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is GaussianBlurOption == false) throw new ImageException("Opetion is not GaussianBlurOption");
                _opetion = value as GaussianBlurOption;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ImageOption CreateOption()
        {
            return new GaussianBlurOption { Sigma = 0.75f };
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        protected static int Padding = 3;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPixels"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected float[] ApplyBlur(float[] srcPixels, int width, int height)
        {
            float[] destPixels = new float[srcPixels.Length];
            System.Array.Copy(srcPixels, 0, destPixels, 0, srcPixels.Length);

            int w = width + Padding * 2;
            int h = height + Padding * 2;

            // Calculate the coefficients
            float q = Sigma;
            float q2 = q * q;
            float q3 = q2 * q;

            float b0 = 1.57825f + 2.44413f * q + 1.4281f * q2 + 0.422205f * q3;
            float b1 = 2.44413f * q + 2.85619f * q2 + 1.26661f * q3;
            float b2 = -(1.4281f * q2 + 1.26661f * q3);
            float b3 = 0.422205f * q3;

            float b = 1.0f - ((b1 + b2 + b3) / b0);

            // Apply horizontal pass
            ApplyPass(destPixels, w, h, b0, b1, b2, b3, b);

            // Transpose the array
            float[] transposedPixels = new float[destPixels.Length];
            Transpose(destPixels, transposedPixels, w, h);

            // Apply vertical pass
            ApplyPass(transposedPixels, h, w, b0, b1, b2, b3, b);

            // transpose back
            Transpose(transposedPixels, destPixels, h, w);

            return destPixels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="b0"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="b3"></param>
        /// <param name="b"></param>
        protected void ApplyPass(float[] pixels, int width, int height, float b0, float b1, float b2, float b3, float b)
        {
            float num = 1f / b0;
            int triplewidth = width * 3;
            for (int i = 0; i < height; i++)
            {
                int steplength = i * triplewidth;
                for (int j = steplength + 9; j < (steplength + triplewidth); j += 3)
                {
                    pixels[j] = (b * pixels[j]) + ((((b1 * pixels[j - 3]) + (b2 * pixels[j - 6])) + (b3 * pixels[j - 9])) * num);
                    pixels[j + 1] = (b * pixels[j + 1]) + ((((b1 * pixels[(j + 1) - 3]) + (b2 * pixels[(j + 1) - 6])) + (b3 * pixels[(j + 1) - 9])) * num);
                    pixels[j + 2] = (b * pixels[j + 2]) + ((((b1 * pixels[(j + 2) - 3]) + (b2 * pixels[(j + 2) - 6])) + (b3 * pixels[(j + 2) - 9])) * num);
                }
                for (int k = ((steplength + triplewidth) - 9) - 3; k >= steplength; k -= 3)
                {
                    pixels[k] = (b * pixels[k]) + ((((b1 * pixels[k + 3]) + (b2 * pixels[k + 6])) + (b3 * pixels[k + 9])) * num);
                    pixels[k + 1] = (b * pixels[k + 1]) + ((((b1 * pixels[(k + 1) + 3]) + (b2 * pixels[(k + 1) + 6])) + (b3 * pixels[(k + 1) + 9])) * num);
                    pixels[k + 2] = (b * pixels[k + 2]) + ((((b1 * pixels[(k + 2) + 3]) + (b2 * pixels[(k + 2) + 6])) + (b3 * pixels[(k + 2) + 9])) * num);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected void Transpose(float[] input, float[] output, int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int index = (j * height) * 3 + (i * 3);
                    int pos = (i * width) * 3 + (j * 3);
                    output[index] = input[pos];
                    output[index + 1] = input[pos + 1];
                    output[index + 2] = input[pos + 2];
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageIn"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected float[] ConvertImageWithPadding(Bitmap imageIn, int width, int height)
        {
            int newheight = height + Padding * 2;
            int newwidth = width + Padding * 2;
            float[] numArray = new float[(newheight * newwidth) * 3];
            int index = 0;
            int num = 0;
            for (int i = -3; num < newheight; i++)
            {
                int y = i;
                if (i < 0)
                {
                    y = 0;
                }
                else if (i >= height)
                {
                    y = height - 1;
                }
                int count = 0;
                int negpadding = -1 * Padding;
                while (count < newwidth)
                {
                    int x = negpadding;
                    if (negpadding < 0)
                    {
                        x = 0;
                    }
                    else if (negpadding >= width)
                    {
                        x = width - 1;
                    }
                    var color = imageIn.GetPixel(x, y);
                    numArray[index] = color.R * 0.003921569f;
                    numArray[index + 1] = color.G * 0.003921569f;
                    numArray[index + 2] = color.B * 0.003921569f;

                    count++; negpadding++;
                    index += 3;
                }
                num++;
            }
            return numArray;
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

            float[] imageArray = ConvertImageWithPadding(bmp, width, height);
            imageArray = ApplyBlur(imageArray, width, height);
            int newwidth = width + Padding * 2;
            for (int i = 0; i < height; i++)
            {
                int num = ((i + 3) * newwidth) + 3;
                for (int j = 0; j < width; j++)
                {
                    int pos = (num + j) * 3;
                    bmp.SetPixel(j, i, Color.FromArgb((byte)(imageArray[pos] * 255f), (byte)(imageArray[pos + 1] * 255f), (byte)(imageArray[pos + 2] * 255f)));
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

            float[] imageArray = UnConvertImageWithPadding(bmp, width, height);
            imageArray = ApplyBlur(imageArray, width, height);
            int newwidth = width + Padding * 2;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < height; i++)
            {
                int num = ((i + 3) * newwidth) + 3;
                for (int j = 0; j < width; j++)
                {
                    int pos = (num + j) * 3;
                    int moveindex = i * bmpData.Stride + j * 4;

                    ptr[moveindex] = (byte)(imageArray[pos + 2] * 255f);
                    ptr[moveindex + 1] = (byte)(imageArray[pos + 1] * 255f);
                    ptr[moveindex + 2] = (byte)(imageArray[pos] * 255f);

                }
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageIn"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        protected unsafe float[] UnConvertImageWithPadding(Bitmap imageIn, int width, int height)
        {
            int newheight = height + Padding * 2;
            int newwidth = width + Padding * 2;
            float[] numArray = new float[(newheight * newwidth) * 3];
            int index = 0;
            int num = 0;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = imageIn.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = -3; num < newheight; i++)
            {
                int y = i;
                if (i < 0)
                {
                    y = 0;
                }
                else if (i >= height)
                {
                    y = height - 1;
                }
                int count = 0;
                int negpadding = -1 * Padding;
                while (count < newwidth)
                {
                    int x = negpadding;
                    if (negpadding < 0)
                    {
                        x = 0;
                    }
                    else if (negpadding >= width)
                    {
                        x = width - 1;
                    }
                    int moveindex = y * bmpData.Stride + x * 4;

                    numArray[index] = ptr[moveindex + 2] * 0.003921569f;
                    numArray[index + 1] = ptr[moveindex + 1] * 0.003921569f;
                    numArray[index + 2] = ptr[moveindex] * 0.003921569f;

                    count++; negpadding++;
                    index += 3;
                }
                num++;
            }
            imageIn.UnlockBits(bmpData);
            return numArray;
        }
    }
}
