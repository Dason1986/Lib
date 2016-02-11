using System.Drawing;

namespace Library.Draw.Water
{
    /// <summary>
    ///
    /// </summary>
    public class WaterImageBuilderByTile : WaterImageBuilder
    {
        private WaterImageTileOption _opetion;

        /// <summary>
        ///
        /// </summary>
        protected override ImageOption Opetion
        {
            get { return _opetion; }
            set
            {
                if (value is WaterImageTileOption == false) throw new ImageException("Opetion is not WaterImageTileOption");
                _opetion = value as WaterImageTileOption;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="space"></param>
        public void SetSpace(Size space)
        {
            InitOption();
            _opetion.Space = space;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        public void SetOffset(Point offset)
        {
            InitOption();
            _opetion.Offset = offset;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void InitOption()
        {
            if (_opetion == null) _opetion = new WaterImageTileOption();
        }

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

            int spacey = _opetion.Space != null ? _opetion.Space.Value.Height : waterImg.Height;
            int spaceX = _opetion.Space != null ? _opetion.Space.Value.Width : waterImg.Width;
            int offsetY = _opetion.Offset.GetValueOrDefault().Y;
            int offsetX = _opetion.Offset.GetValueOrDefault().X;

            var clo = rectangle.Width / (waterImg.Width + spaceX);
            var row = rectangle.Height / (waterImg.Height + spacey);
            for (int r = -2; r <= row; r++)
            {
                for (int c = -2; c <= clo; c++)
                {
                    int y = (waterImg.Height + spacey) * r + (offsetY * c);
                    int x = (waterImg.Width + spaceX) * c + (offsetX * r);
                    gType.DrawImage(waterImg, new Rectangle(x, y, waterImg.Width, waterImg.Width), 0, 0, waterImg.Width, waterImg.Height, GraphicsUnit.Pixel);
                }
            }

            return img;
        }
    }
}