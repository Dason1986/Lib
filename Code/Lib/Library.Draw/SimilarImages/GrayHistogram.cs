using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.SimilarImages
{
    /// <summary>
    /// 灰度直方图
    /// </summary>
    public sealed class GrayHistogram : SimilarAlgorithm
    {




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SimilarityResult Compare(byte[] hisogramX, byte[] hisogramY)
        {




            var res = Math.Round(GetResult(hisogramX, hisogramY) * 100, 2);
            return new SimilarityResult() { Similarity = res, IsSame = res > Similarity };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SimilarityResult Compare()
        {



            byte[] x = BuildFingerprint(ImageX);
            byte[] y = BuildFingerprint(ImageY);


            return Compare(x, y);
        }
        private float GetAbs(int firstNum, int secondNum)
        {

            float abs = Math.Abs((float)firstNum - (float)secondNum);

            float result = Math.Max(firstNum, secondNum);

            if (result == 0)

                result = 1;

            return abs / result;

        }



        //最终计算结果

        float GetResult(byte[] firstNum, byte[] scondNum)
        {

            if (firstNum.Length != scondNum.Length)

            {

                return 0;

            }

            else

            {

                float result = 0;

                int j = firstNum.Length;

                for (int i = 0; i < j; i++)
                {

                    result += 1 - GetAbs(firstNum[i], scondNum[i]);

                    //  Console.WriteLine(i + "----" + result);

                }

                return result / j;

            }

        }


        byte[] GetHisogram(Bitmap img)
        {
            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] histogram = new byte[(int)Live];
            int count = 0;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int remain = data.Stride - data.Width * 3;

                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        int mean = ptr[0] + ptr[1] + ptr[2];
                        mean /= 3;
                        histogram[count] = (byte)mean;
                        ptr += 3;
                    }
                    ptr += remain;
                }
            }
            img.UnlockBits(data);
            img.Dispose();
            return histogram;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public override byte[] BuildFingerprint(Image image)
        {
            byte[] x = GetHisogram(GetThumbnailImage(image));
            return x;
        }
    }
}