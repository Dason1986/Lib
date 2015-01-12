using System.Drawing;

namespace Library.Draw
{
    public class ImageOption
    {
        private float _opacity = 1;
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (value > 1 || _opacity < 0) throw new ImageException("超出範圍，值必須在0-1之間");
                _opacity = value;
            }
        }
        public Size? TrageSize { get; set; }
    }

    public enum WaterImageType
    {
        Pixel,
        Full,
        Tile,
        Text,
    }

    public enum Localization
    {
        Top,
        TopLeft,
        TopRight,
        Centre,
        CentreLeft,
        CentreRight,
        Bottom,
        BottomLeft,
        BottomRight,
    }


}