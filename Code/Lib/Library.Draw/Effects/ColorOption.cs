using Library.Att;
using System.ComponentModel;

namespace Library.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    [LanguageDescription("顏色"), LanguageDisplayName("顏色")]
    public class ColorOption : ImageOption
    {
        /// <summary>
        /// 紅
        /// </summary>
        [LanguageDescription("顏色RGB:紅"), LanguageDisplayName("紅"), Category("濾鏡選項")]
        public int Red { get; set; }

        /// <summary>
        /// 綠
        /// </summary>
        [LanguageDescription("顏色RGB:綠"), LanguageDisplayName("綠"), Category("濾鏡選項")]
        public int Green { get; set; }

        /// <summary>
        /// 藍
        /// </summary>
        [LanguageDescription("顏色RGB:藍"), LanguageDisplayName("藍"), Category("濾鏡選項")]
        public int Blue { get; set; }
    }
}