using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// 用六角形锥体表示自己的颜色模型
    /// </summary>
    /// [Editor("Library.Draw.Design.HSLColorEditor, Library.Draw.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [TypeConverter(typeof(HSLColorConverter))]
    public struct HSLColor : IToRGBColor
    {
        /// <summary>
        /// Gets an empty RGB structure;
        /// </summary>
        public static readonly HSLColor Empty = new HSLColor();

        /// <summary>
        /// 色相
        /// </summary>
        [LanguageDescription("是色彩的基本属性，就是平常所说的颜色名称，如红色、黄色等,360°"), LanguageDisplayName("色相")]
        public float Hue { get; private set; }

        /// <summary>
        /// 饱和度
        /// </summary>
        [LanguageDescription("亮度，取0-100%"), LanguageDisplayName("亮度")]
        public float Luminance { get; private set; }

        /// <summary>
        /// 亮度
        /// </summary>
        [LanguageDescription("是指色彩的纯度，越高色彩越纯，低则逐渐变灰，取0-100%的数值"), LanguageDisplayName("饱和度")]
        public float Saturation { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        private HSLColor(float h, float s, float l)
            : this()
        {
            this.Hue = h;
            this.Saturation = s;
            this.Luminance = l;
        }

        // HSL to RGB helper routine
        private static double Hue_2_RGB(double v1, double v2, double vH)
        {
            if (vH < 0)
                vH += 1;
            if (vH > 1)
                vH -= 1;
            if ((6 * vH) < 1)
                return (v1 + (v2 - v1) * 6 * vH);
            if ((2 * vH) < 1)
                return v2;
            if ((3 * vH) < 2)
                return (v1 + (v2 - v1) * ((2.0 / 3) - vH) * 6);
            return v1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(HSLColor item1, HSLColor item2)
        {
            return (
                item1.Hue == item2.Hue
                && item1.Saturation == item2.Saturation
                && item1.Luminance == item2.Luminance
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(HSLColor item1, HSLColor item2)
        {
            return (
                item1.Hue != item2.Hue
                || item1.Saturation != item2.Saturation
                || item1.Luminance != item2.Luminance
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int ToRGBValue()
        {
            int r, g, b;
            if (this.Hue == 0)
            {
                // gray values
                r = g = b = (byte)(this.Luminance * 255);
            }
            else
            {
                double hue = (double)this.Hue / 360;

                double v2 = (this.Luminance < 0.5) ?
                    (this.Luminance * (1 + this.Saturation)) :
                    ((this.Luminance + this.Saturation) - (this.Luminance * this.Saturation));
                var v1 = 2 * this.Luminance - v2;

                r = (byte)(255 * Hue_2_RGB(v1, v2, hue + (1.0 / 3)));
                g = (byte)(255 * Hue_2_RGB(v1, v2, hue));
                b = (byte)(255 * Hue_2_RGB(v1, v2, hue - (1.0 / 3)));
            }
            return (255 << 24) + (r << 16) + (g << 8) + b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="c2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public HSLColor Interpolate(HSLColor c2, float amount)
        {
            return new HSLColor(this.Hue + ((c2.Hue - this.Hue) * amount), this.Saturation + ((c2.Saturation - this.Saturation) * amount), this.Luminance + ((c2.Luminance - this.Luminance) * amount));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (HSLColor)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Hue.GetHashCode() ^ Saturation.GetHashCode() ^
                Luminance.GetHashCode();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Color ToRGB()
        {
            int r, g, b;
            if (Saturation == 0)
            {
                r = (Byte)(Luminance * 255);
                g = (Byte)(Luminance * 255);
                b = (Byte)(Luminance * 255);
            }
            else
            {
                float var2;
                if (Luminance < 0.5) var2 = Luminance * (1 + Saturation);
                else var2 = (Luminance + Saturation) - (Saturation * Luminance);

                var var1 = 2 * Luminance - var2;

                r = (Byte)(255 * Hue_2_RGB(var1, var2, Hue + (1 / 3)));
                g = (Byte)(255 * Hue_2_RGB(var1, var2, Hue));
                b = (Byte)(255 * Hue_2_RGB(var1, var2, Hue - (1 / 3)));
            }

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="lightness"></param>
        /// <returns></returns>
        public static HSLColor FromHSL(int hue, int saturation, int lightness)
        {
            return new HSLColor(hue, saturation, lightness);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="gg"></param>
        /// <param name="bb"></param>
        public static HSLColor FromRGB(int rr, int gg, int bb)
        {
            double r = (rr / 255.0);
            double g = (gg / 255.0);
            double b = (bb / 255.0);

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;
            HSLColor hsl = new HSLColor();
            // get luminance value
            hsl.Luminance = (float)(max + min) / 2;

            if (delta == 0)
            {
                // gray color
                hsl.Hue = 0;
                hsl.Saturation = 0.0f;
            }
            else
            {
                // get saturation value
                hsl.Saturation = (float)((hsl.Luminance < 0.5) ? (delta / (max + min)) : (delta / (2 - max - min)));

                // get hue value
                double del_r = (((max - r) / 6) + (delta / 2)) / delta;
                double del_g = (((max - g) / 6) + (delta / 2)) / delta;
                double del_b = (((max - b) / 6) + (delta / 2)) / delta;
                double hue;

                if (r == max)
                    hue = del_b - del_g;
                else if (g == max)
                    hue = (1.0 / 3) + del_r - del_b;
                else
                    hue = (2.0 / 3) + del_g - del_r;

                // correct hue if needed
                if (hue < 0)
                    hue += 1;
                if (hue > 1)
                    hue -= 1;

                hsl.Hue = (int)(hue * 360);
            }
            return hsl;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HSLColorConverter : TypeConverter
    {
    }
}