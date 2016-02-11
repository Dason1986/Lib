using Library.Att;
using System;

namespace Library
{
    /// <summary>
    /// 方向
    /// </summary>
    [Flags]
    public enum AlignmentType
    {
        /// <summary>
        /// 垂直
        /// </summary>
        [LanguageDescription("垂直"), LanguageDisplayName("垂直")]
        Horizontally = 1,

        /// <summary>
        /// 橫向
        /// </summary>
        [LanguageDescription("橫向"), LanguageDisplayName("橫向")]
        Vertically = 2,
    }
}