using Library.Att;
using System.ComponentModel;

namespace Library.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    [LanguageDescription("�ɫ"), LanguageDisplayName("�ɫ")]
    public class ColorOption : ImageOption
    {
        /// <summary>
        /// �t
        /// </summary>
        [LanguageDescription("�ɫRGB:�t"), LanguageDisplayName("�t"), Category("�V�R�x�")]
        public int Red { get; set; }

        /// <summary>
        /// �G
        /// </summary>
        [LanguageDescription("�ɫRGB:�G"), LanguageDisplayName("�G"), Category("�V�R�x�")]
        public int Green { get; set; }

        /// <summary>
        /// �{
        /// </summary>
        [LanguageDescription("�ɫRGB:�{"), LanguageDisplayName("�{"), Category("�V�R�x�")]
        public int Blue { get; set; }
    }
}