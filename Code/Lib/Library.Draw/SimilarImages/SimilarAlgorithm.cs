using System;
using System.Drawing;

namespace Library.Draw.SimilarImages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SimilarAlgorithm:ISimilarAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        public SimilarAlgorithm()
        {
            Live = LiveEnum.Pixels256;
            Similarity = 85;
        }
        /// <summary>
        /// 
        /// </summary>
        public LiveEnum Live { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Image ImageX { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public Image ImageY { get; protected set; }

        double _similarity;
        /// <summary>
        /// 
        /// </summary>
        public double Similarity
        {
            get { return _similarity; }
            set
            {
                if (value < 50 || value > 100) throw new Exception();
                _similarity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        protected Bitmap GetThumbnailImage(Image image)
        {
            Bitmap.GetThumbnailImageAbort myCallback = new Bitmap.GetThumbnailImageAbort(() => { return false; });
            var thumbnailImage = image.GetThumbnailImage((int)Live, (int)Live, myCallback, IntPtr.Zero);
            Bitmap myBitmap = new Bitmap(thumbnailImage);
            thumbnailImage.Dispose();
            return myBitmap;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public abstract byte[] BuildFingerprint(Image image);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageX"></param>
        /// <param name="imageY"></param>
        public void SetIamge(Image imageX, Image imageY)
        {
            ImageX = imageX;
            ImageY = imageY;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract SimilarityResult Compare();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisogramX"></param>
        /// <param name="hisogramY"></param>
        /// <returns></returns>
        public abstract SimilarityResult Compare(byte[] hisogramX, byte[] hisogramY);
    }
}