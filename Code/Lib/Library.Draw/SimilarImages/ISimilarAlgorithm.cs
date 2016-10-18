using System.Drawing;

namespace Library.Draw.SimilarImages
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimilarAlgorithm
    {
        double Similarity { get; set; }

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