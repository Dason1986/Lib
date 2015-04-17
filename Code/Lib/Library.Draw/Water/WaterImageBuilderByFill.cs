using System.Drawing;

namespace Library.Draw.Water
{
    /// <summary>
    /// 
    /// </summary>
    public class WaterImageBuilderByFill : WaterImageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            Image img = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics gType = Graphics.FromImage(img);

            gType.DrawImage(waterImg, rectangle, 0, 0, waterImg.Width, waterImg.Height, GraphicsUnit.Pixel);

            return img;

        }


    }
}