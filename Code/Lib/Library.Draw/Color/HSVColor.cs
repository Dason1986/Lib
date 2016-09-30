using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// HSV颜色空间
    /// HSV(hue,saturation,value)颜色空间的模型对应于圆柱坐标系中的一个圆锥形子集，圆锥的顶面对应于V=1. 它包含RGB模型中的R=1，G=1，B=1 三个面，所代表的颜色较亮。色彩H由绕V轴的旋转角给定。红色对应于 角度0° ，绿色对应于角度120°，蓝色对应于角度240°。在HSV颜色模型中，每一种颜色和它的补色相差180° 。 饱和度S取值从0到1，所以圆锥顶面的半径为１。HSV颜色模型所代表的颜色域是CIE色度图的一个子集，这个 模型中饱和度为百分之百的颜色，其纯度一般小于百分之百。在圆锥的顶点(即原点)处，V=0,H和S无定义， 代表黑色。圆锥的顶面中心处S=0，V=1,H无定义，代表白色。从该点到原点代表亮度渐暗的灰色，即具有不同 灰度的灰色。对于这些点，S=0,H的值无定义。可以说，HSV模型中的V轴对应于RGB颜色空间中的主对角线。 在圆锥顶面的圆周上的颜色，V=1，S=1,这种颜色是纯色。HSV模型对应于画家配色的方法。画家用改变色浓和 色深的方法从某种纯色获得不同色调的颜色，在一种纯色中加入白色以改变色浓，加入黑色以改变色深，同时 加入不同比例的白色，黑色即可获得各种不同的色调。
    /// </summary>
    /// [Editor("Library.Draw.Design.HSVColorEditor, Library.Draw.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [TypeConverter(typeof(HSVColorConverter))]
    public struct HSVColor : IToRGBColor
    {  /// <summary>
       /// Gets an empty RGB structure;
       /// </summary>
        public static readonly HSVColor Empty = new HSVColor();

        /// <summary>
        /// 色相
        /// </summary>
        [LanguageDescription("是色彩的基本属性，就是平常所说的颜色名称，如红色、黄色等,360°"), LanguageDisplayName("色相")]
        public float Hue { get; private set; }

        /// <summary>
        /// 饱和度
        /// </summary>
        [LanguageDescription("是指色彩的纯度，越高色彩越纯，低则逐渐变灰，取0-100%的数值"), LanguageDisplayName("饱和度")]
        public float Saturation { get; private set; }

        /// <summary>
        /// 明度
        /// </summary>
        [LanguageDescription("明度，取0-100%"), LanguageDisplayName("明度")]
        public float Value { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="value"></param>
        public HSVColor(float hue, float saturation, float value)
            : this()
        {
            this.Hue = hue;
            this.Value = value;
            this.Saturation = saturation;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Color ToRGB()
        {
            double H = this.Hue;
            while (H < 0) { H += 360; }
            while (H >= 360) { H -= 360; }
            double R, G, B;
            if (this.Value <= 0)
            { R = G = B = 0; }
            else if (this.Saturation <= 0)
            {
                R = G = B = Value;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = Value * (1 - Saturation);
                double qv = Value * (1 - Saturation * f);
                double tv = Value * (1 - Saturation * (1 - f));
                switch (i)
                {
                    // Red is the dominant color
                    case 0:
                        R = Value;
                        G = tv;
                        B = pv;
                        break;
                    // Green is the dominant color
                    case 1:
                        R = qv;
                        G = Value;
                        B = pv;
                        break;

                    case 2:
                        R = pv;
                        G = Value;
                        B = tv;
                        break;
                    // Blue is the dominant color
                    case 3:
                        R = pv;
                        G = qv;
                        B = Value;
                        break;

                    case 4:
                        R = tv;
                        G = pv;
                        B = Value;
                        break;
                    // Red is the dominant color
                    case 5:
                        R = Value;
                        G = pv;
                        B = qv;
                        break;
                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.
                    case 6:
                        R = Value;
                        G = tv;
                        B = pv;
                        break;

                    case -1:
                        R = Value;
                        G = pv;
                        B = qv;
                        break;
                    // The color is not defined, we should throw an error.
                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = Value; // Just pretend its black/white
                        break;
                }
            }
            var r = Clamp((int)(R * 255.0));
            var g = Clamp((int)(G * 255.0));
            var b = Clamp((int)(B * 255.0));
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        private int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (HSVColor)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Hue.GetHashCode() ^ Saturation.GetHashCode() ^
                Value.GetHashCode();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(HSVColor item1, HSVColor item2)
        {
            return (
                item1.Hue == item2.Hue
                && item1.Saturation == item2.Saturation
                && item1.Value == item2.Value
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(HSVColor item1, HSVColor item2)
        {
            return (
                item1.Hue != item2.Hue
                || item1.Saturation != item2.Saturation
                || item1.Value != item2.Value
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="gg"></param>
        /// <param name="bb"></param>
        public static HSVColor FromRGB(int rr, int gg, int bb)
        {
            float r = (float)(rr / 255.0);
            float g = (float)(gg / 255.0);
            float b = (float)(bb / 255.0);

            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);
            HSVColor hsv = new HSVColor();
            hsv.Value = max;
            float delta = max - min;

            if (max != 0)
            {
                hsv.Saturation = delta / max;
            }
            else
            {
                return hsv;
            }
            float h;

            if (r == max)
            {
                h = (g - b) / delta; // between yellow & magenta
            }
            else if (r == max)
            {
                h = 2 + (b - r) / delta; // between cyan & yellow
            }
            else
            {
                h = 4 + (r - b) / delta; // between magenta & cyan
            }
            h = h * 60;

            if (h < 0)
                h += 360;

            hsv.Hue = h;
            return hsv;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HSVColorConverter : TypeConverter
    {
    }
}