using Library.Att;
using System.ComponentModel;

namespace Library.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    [LanguageDescription("值選擇"), LanguageDisplayName("值選擇")]
    public class ValueOption : ImageOption
    {
        /// <summary>
        ///
        /// </summary>
        [LanguageDescription("值"), LanguageDisplayName("值"), Category("濾鏡選項")]
        public float Value { get; set; }
    }
}