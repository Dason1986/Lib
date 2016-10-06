using System.Drawing;

namespace Library.Draw.SimilarImages
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimilarAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageX"></param>
        /// <param name="imageY"></param>
        void SetIamge(Image imageX, Image imageY);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SimilarityResult Compare();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisogramX"></param>
        /// <param name="hisogramY"></param>
        /// <returns></returns>
        SimilarityResult Compare(byte[] hisogramX, byte[] hisogramY);
    }
}