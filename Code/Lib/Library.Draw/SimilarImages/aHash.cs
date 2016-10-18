using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Library.Draw.SimilarImages
{
    /// <summary>
    ///   感知哈希算法
    /// </summary>
    public class PerceptualHash : SimilarAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        public PerceptualHash()
        {
            this.Similarity = 5;
        }

        double _similarity;
        /// <summary>
        /// 
        /// </summary>
        public override double Similarity
        {
            get { return _similarity; }
            set
            {
                if (value < 0 || value > 100) throw new Exception();
                _similarity = value;
            }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisogramX"></param>
        /// <param name="hisogramY"></param>
        /// <returns></returns>
        public override SimilarityResult Compare(byte[] hisogramX, byte[] hisogramY)
        {
            if (hisogramX.Length != hisogramY.Length)
                return new SimilarityResult();
            double count = 0;
            for (int i = 0; i < hisogramX.Length; i++)
            {
                if (hisogramX[i] != hisogramY[i])
                    count++;
            }

            //   var result = Math.Round(count * 100 / hisogramX.Length, 2);

            return new SimilarityResult() { Similarity = count, IsSame = count < this.Similarity };
        }



        byte[] GetHisogram(Bitmap img)
        {
            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            #region ReduceColor
            byte[] histogram = new byte[(int)Live * (int)Live];
            int count = 0;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int remain = data.Stride - data.Width * 3;

                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        //    ptr[0] = 0;//B
                        //     ptr[1] = ptr[1];//G
                        //    ptr[2] = 0;//R
                        byte mean = (byte)(ptr[0] * 11 + ptr[1] * 59 + ptr[2] * 30 / 100);

                        histogram[count] = mean;
                        count++;
                        ptr += 3;
                    }
                    ptr += remain;
                }
            }
            img.UnlockBits(data);
            img.Dispose();
            #endregion
            #region CalcAverage
            decimal sum = 0;
            for (int i = 0; i < histogram.Length; i++)
                sum += (int)histogram[i];
            var averageValue = Convert.ToByte(sum / histogram.Length);
            #endregion

            byte[] result = new byte[histogram.Length];
            for (int i = 0; i < histogram.Length; i++)
            {
                if (histogram[i] < averageValue)
                    result[i] = 0;
                else
                    result[i] = 1;
            }

            return result;
        }

    }
    /// <summary>
    /// 颜色分布
    /// </summary>
    public class ColorHistogram
    {

    }
    /// <summary>
    /// 平均哈希算法
    /// </summary>
    public class AverageHash
    {

    }


    /// <summary>
    /// 差异哈希算法
    /// </summary>
    public class DifferenceHash
    {

    }
}
