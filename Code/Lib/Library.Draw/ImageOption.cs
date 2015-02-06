using System.ComponentModel;
using System.Drawing;
using Library.Att;

namespace Library.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageOption
    {
        private float _opacity = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ImageException"></exception>

        [LanguageDescription("透明度，值必須在0-1之間。"), LanguageDisplayName("透明度")]
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (value > 1 || _opacity < 0) throw new ImageException("超出範圍，值必須在0-1之間。");
                _opacity = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        
        [LanguageDescription("新尺寸，新圖的尺寸。"), LanguageDisplayName("新尺寸")]
        public Size? TragetSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WaterImageType
    {
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("保持像素")]
        Pixel,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("填充")]
        Full,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("平鋪")]
        Tile,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("文字")]
        Text,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Localization
    {
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("上中")]
        Top,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("上左")]
        TopLeft,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("上右")]
        TopRight,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("中心")]
        Centre,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("左中")]
        CentreLeft,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("右中")]
        CentreRight,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("下中")]
        Bottom,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("左下")]
        BottomLeft,
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("右下")]
        BottomRight,
    }


}