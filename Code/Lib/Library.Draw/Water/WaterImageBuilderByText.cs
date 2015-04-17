using System.Drawing;
using System.IO;

namespace Library.Draw.Water
{
    /// <summary>
    /// 
    /// </summary>
    public class WaterImageBuilderByText : WaterImageBuilder
    {
        private Localization _localization = Localization.BottomRight;
        /// <summary>
        /// 
        /// </summary>
        public Localization Localization
        {
            get { return _localization; }
            set { _localization = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Font TextFont { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Image ProcessBitmap()
        {
            if (Opetion == null) throw new ImageException("Opetion is null");
            if (TextFont == null) TextFont = new Font("ºÚÌå", 20);
            MemoryStream sourcestream = new MemoryStream(SourceImgBuffter);

            var sourceImg = Image.FromStream(sourcestream);

            var trageSize = Opetion == null ? null : Opetion.TragetSize;
            Image tmpimg = trageSize != null ? new Bitmap(trageSize.Value.Width, trageSize.Value.Height) : new Bitmap(sourceImg.Width, sourceImg.Height);
            Graphics gType = CreateGraphics(tmpimg, sourceImg);
            var attributes = GetOpacity(Opetion.Opacity);
            var sizef = gType.MeasureString(Text, TextFont, sourceImg.Width, StringFormat.GenericDefault);
            var waterImg = new Bitmap((int)sizef.Width, (int)sizef.Height);
            var waterRectangle = GetWaterRectangle(sourceImg, waterImg);
            var tmpwatrer = CreateFillImage(waterImg);
            try
            {
                gType.DrawImage(tmpwatrer, waterRectangle, 0, 0, tmpwatrer.Width, tmpwatrer.Height, GraphicsUnit.Pixel, attributes);
                return tmpimg;
            }
            finally
            {

                sourcestream.Dispose();
                tmpwatrer.Dispose();
                sourceImg.Dispose();

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected Image CreateFillImage(Image waterImg)
        {
            Graphics gType = Graphics.FromImage(waterImg);
            gType.DrawString(Text, TextFont, new SolidBrush(Color.Black), 1, 1);
            gType.DrawString(Text, TextFont, new SolidBrush(Color.White), 0, 0);
            return waterImg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="waterImg"></param>
        /// <returns></returns>
        protected override Image CreateFillImage(Rectangle rectangle, Image waterImg)
        {
            return CreateFillImage(waterImg);
        }
    }
}