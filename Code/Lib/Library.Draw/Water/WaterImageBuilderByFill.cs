using System.Drawing;

namespace Library.Draw.Water
{
    public class WaterImageBuilderByFill : WaterImageBuilder
    {
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            Image img = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics gType = Graphics.FromImage(img);

            gType.DrawImage(waterImg, rectangle, 0, 0, waterImg.Width, waterImg.Height, GraphicsUnit.Pixel);

            return img;

        }


    }
}