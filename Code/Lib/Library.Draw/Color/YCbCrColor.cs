using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    ///
    /// </summary>
    [TypeConverter(typeof(YCbCrColorConverter))]
    public struct YCbCrColor : IToRGBColor
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Color ToRGB()
        {
            float r = Math.Max(0.0f, Math.Min(1.0f, (float)(this.Y + 0.0000 * this.Cb + 1.4022 * this.Cr)));
            float g = Math.Max(0.0f, Math.Min(1.0f, (float)(this.Y - 0.3456 * this.Cb - 0.7145 * this.Cr)));
            float b = Math.Max(0.0f, Math.Min(1.0f, (float)(this.Y + 1.7710 * this.Cb + 0.0000 * this.Cr)));
            return Color.FromArgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="gg"></param>
        /// <param name="bb"></param>
        public static YCbCrColor FromRGB(int rr, int gg, int bb)
        {
            float r = (float)rr / 255;
            float g = (float)gg / 255;
            float b = (float)bb / 255;
            YCbCrColor yCbCr = new YCbCrColor();
            yCbCr.Y = (float)(0.2989 * r + 0.5866 * g + 0.1145 * b);
            yCbCr.Cb = (float)(-0.1687 * r - 0.3313 * g + 0.5000 * b);
            yCbCr.Cr = (float)(0.5000 * r - 0.4184 * g - 0.0816 * b);
            return yCbCr;
        }

        /// <summary>
        ///
        /// </summary>
        public float Cr { get; set; }

        /// <summary>
        ///
        /// </summary>
        public float Cb { get; set; }

        /// <summary>
        ///
        /// </summary>
        public float Y { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class YCbCrColorConverter : TypeConverter
    {
    }
}