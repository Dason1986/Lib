using Library.Att;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// HSV��ɫ�ռ�
    /// HSV(hue,saturation,value)��ɫ�ռ��ģ�Ͷ�Ӧ��Բ������ϵ�е�һ��Բ׶���Ӽ���Բ׶�Ķ����Ӧ��V=1. ������RGBģ���е�R=1��G=1��B=1 �����棬���������ɫ������ɫ��H����V�����ת�Ǹ�������ɫ��Ӧ�� �Ƕ�0�� ����ɫ��Ӧ�ڽǶ�120�㣬��ɫ��Ӧ�ڽǶ�240�㡣��HSV��ɫģ���У�ÿһ����ɫ�����Ĳ�ɫ���180�� �� ���Ͷ�Sȡֵ��0��1������Բ׶����İ뾶Ϊ����HSV��ɫģ�����������ɫ����CIEɫ��ͼ��һ���Ӽ������ ģ���б��Ͷ�Ϊ�ٷ�֮�ٵ���ɫ���䴿��һ��С�ڰٷ�֮�١���Բ׶�Ķ���(��ԭ��)����V=0,H��S�޶��壬 �����ɫ��Բ׶�Ķ������Ĵ�S=0��V=1,H�޶��壬�����ɫ���Ӹõ㵽ԭ��������Ƚ����Ļ�ɫ�������в�ͬ �ҶȵĻ�ɫ��������Щ�㣬S=0,H��ֵ�޶��塣����˵��HSVģ���е�V���Ӧ��RGB��ɫ�ռ��е����Խ��ߡ� ��Բ׶�����Բ���ϵ���ɫ��V=1��S=1,������ɫ�Ǵ�ɫ��HSVģ�Ͷ�Ӧ�ڻ�����ɫ�ķ����������øı�ɫŨ�� ɫ��ķ�����ĳ�ִ�ɫ��ò�ͬɫ������ɫ����һ�ִ�ɫ�м����ɫ�Ըı�ɫŨ�������ɫ�Ըı�ɫ�ͬʱ ���벻ͬ�����İ�ɫ����ɫ���ɻ�ø��ֲ�ͬ��ɫ����
    /// </summary>
    /// [Editor("Library.Draw.Design.HSVColorEditor, Library.Draw.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [TypeConverter(typeof(HSVColorConverter))]
    public struct HSVColor : IToRGBColor
    {  /// <summary>
       /// Gets an empty RGB structure;
       /// </summary>
        public static readonly HSVColor Empty = new HSVColor();

        /// <summary>
        /// ɫ��
        /// </summary>
        [LanguageDescription("��ɫ�ʵĻ������ԣ�����ƽ����˵����ɫ���ƣ����ɫ����ɫ��,360��"), LanguageDisplayName("ɫ��")]
        public float Hue { get; private set; }

        /// <summary>
        /// ���Ͷ�
        /// </summary>
        [LanguageDescription("��ָɫ�ʵĴ��ȣ�Խ��ɫ��Խ���������𽥱�ң�ȡ0-100%����ֵ"), LanguageDisplayName("���Ͷ�")]
        public float Saturation { get; private set; }

        /// <summary>
        /// ����
        /// </summary>
        [LanguageDescription("���ȣ�ȡ0-100%"), LanguageDisplayName("����")]
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