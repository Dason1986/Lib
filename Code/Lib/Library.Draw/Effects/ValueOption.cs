using System.ComponentModel;
using Library.Att;

namespace Library.Draw.Effects
{
    /// <summary>
    /// 
    /// </summary>
    [LanguageDescription("ֵ"), LanguageDisplayName("�����x�")]
    public class ValueOption : ImageOption
    {
        /// <summary>
        /// 
        /// </summary>
        [LanguageDescription("ֵ"), LanguageDisplayName("ֵ"), Category("�V�R�x�")]
        public float Value { get; set; }
    }
}