using System;
using Library.Att;

namespace Library
{
    /// <summary>
    /// ����
    /// </summary>
    [Flags]
    public enum AlignmentType
    {
        /// <summary>
        /// ��ֱ
        /// </summary>
        [LanguageDescription("��ֱ"), LanguageDisplayName("��ֱ")]
        Horizontally = 1,
        /// <summary>
        /// �M��
        /// </summary> 
        [LanguageDescription("�M��"), LanguageDisplayName("�M��")]
        Vertically = 2,

    }
}