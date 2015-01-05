using System.Drawing;

namespace Library.Draw.Water
{
    public class WaterImageBuilderByPixel : WaterImageBuilder
    {
        private Localization _localization = Localization.BottomRight;
        public Localization Localization
        {
            get { return _localization; }
            set { _localization = value; }
        }


        protected override Rectangle GetWaterRectangle(Image sourceImg, Image waterImg)
        {

            Rectangle waterRectangle;
            var subtractx = sourceImg.Width - waterImg.Width;
            var subtracty = sourceImg.Height - waterImg.Height;
            switch (_localization)
            {
                case Localization.Top:
                    waterRectangle = new Rectangle(subtractx / 2, 0, waterImg.Width, waterImg.Height);
                    break;
                case Localization.TopLeft:
                    waterRectangle = new Rectangle(0, 0, waterImg.Width, waterImg.Height);
                    break;
                case Localization.TopRight:
                    waterRectangle = new Rectangle(subtractx, 0, waterImg.Width, waterImg.Height);
                    break;
                case Localization.Centre:
                    waterRectangle = new Rectangle(subtractx / 2, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;
                case Localization.CentreLeft:
                    waterRectangle = new Rectangle(0, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;
                case Localization.CentreRight:
                    waterRectangle = new Rectangle(subtractx, subtracty / 2, waterImg.Width, waterImg.Height);
                    break;
                case Localization.Bottom:
                    waterRectangle = new Rectangle(subtractx / 2, subtracty, waterImg.Width, waterImg.Height);
                    break;
                case Localization.BottomLeft:
                    waterRectangle = new Rectangle(0, subtracty, waterImg.Width, waterImg.Height);
                    break;
                    //case Localization.BottomRight:
                default:
                    waterRectangle = new Rectangle(subtractx, subtracty, waterImg.Width, waterImg.Height);
                    break;
            }
            return waterRectangle;
        }

        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            return waterImg;
        }
    }
}