using Library.Att;
using System.ComponentModel;

namespace Library.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    [LanguageDescription("顏色選項"), LanguageDisplayName("顏色選項")]
    public class ColorOption : ImageOption
    {
        /// <summary>
        /// 廠
        /// </summary>
        [LanguageDescription("RGB:紅"), LanguageDisplayName("紅"), Category("濾鏡選項")]
        public int Red { get; set; }

        /// <summary>
        /// 鄭
        /// </summary>
        [LanguageDescription("RGB:綠"), LanguageDisplayName("綠"), Category("濾鏡選項")]
        public int Green { get; set; }

        /// <summary>
        /// 芅
        /// </summary>
        [LanguageDescription("RGB:藍"), LanguageDisplayName("藍"), Category("濾鏡選項")]
        public int Blue { get; set; }
    }
}