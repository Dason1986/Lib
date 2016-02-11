using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// ��ɫ�ռ�Ӧ����ӡˢ��ҵ
    /// </summary>
    /// [Editor("Library.Draw.Design.CMYKColorEditor, Library.Draw.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [TypeConverter(typeof(CMYKColorConverter))]
    public struct CMYKColor : IToRGBColor
    {
        /// <summary>
        /// Gets an empty RGB structure;
        /// </summary>
        public static readonly CMYKColor Empty = new CMYKColor();

        /// <summary>
        /// ��
        /// </summary>
        [LanguageDescription("C - Cyan �� ������ɫ�� R - Red �� "), LanguageDisplayName("��")]
        public int Cyan { get; private set; }

        /// <summary>
        /// Ʒ��
        /// </summary>
        [LanguageDescription("M - Magenta Ʒ�� ������ɫ�� G - Green �� "), LanguageDisplayName("Ʒ��")]
        public int Magenta { get; private set; }

        /// <summary>
        /// ��
        /// </summary>
        [LanguageDescription("Y - Yellow �� ������ɫ�� B - Blue ��"), LanguageDisplayName("��")]
        public int Yellow { get; private set; }

        /// <summary>
        /// ��
        /// </summary>
        [LanguageDescription(""), LanguageDisplayName("��")]
        public int Black { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Color ToRGB()
        {
            float myC = this.Cyan / 100;
            float myM = this.Magenta / 100;
            float myY = this.Yellow / 100;
            float myK = this.Black / 100;

            int r = (int)((1 - (myC * (1 - myK) + myK)) * 255);
            int g = (int)((1 - (myM * (1 - myK) + myK)) * 255);
            int b = (int)((1 - (myY * (1 - myK) + myK)) * 255);

            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="gg"></param>
        /// <param name="bb"></param>
        public static CMYKColor FromRGB(int rr, int gg, int bb)
        {
            CMYKColor cmyk = new CMYKColor();

            cmyk.Black = (int)(Math.Min(Math.Min(255 - rr, 255 - gg), 255 - bb) / 2.55);//cmykK
            int myR = (int)(rr / 2.55);
            int div = 100 - cmyk.Black;
            if (div == 0) div = 1;
            cmyk.Cyan = ((100 - myR - cmyk.Black) / div) * 100;//cmykC
            int myG = (int)(gg / 2.55);
            cmyk.Magenta = ((100 - myG - cmyk.Black) / div) * 100;
            int myB = (int)(bb / 2.55);
            cmyk.Yellow = ((100 - myB - cmyk.Black) / div) * 100;
            return cmyk;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(CMYKColor item1, CMYKColor item2)
        {
            return (
                item1.Cyan == item2.Cyan
                && item1.Magenta == item2.Magenta
                && item1.Yellow == item2.Yellow
                && item1.Black == item2.Black
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(CMYKColor item1, CMYKColor item2)
        {
            return (
                item1.Cyan != item2.Cyan
                || item1.Magenta != item2.Magenta
                || item1.Yellow != item2.Yellow
                || item1.Black != item2.Black
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (CMYKColor)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Cyan.GetHashCode() ^
              Magenta.GetHashCode() ^ Yellow.GetHashCode() ^ Black.GetHashCode();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CMYKColorConverter : TypeConverter
    {
    }
}