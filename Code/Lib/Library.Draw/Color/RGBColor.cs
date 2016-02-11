using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// RGB structure.
    /// </summary>
    [TypeConverter(typeof(RGBColorConverter))]
    public struct RGBColor : IToRGBColor
    {
        /// <summary>
        /// Gets an empty RGB structure;
        /// </summary>
        public static readonly RGBColor Empty = new RGBColor();

        private int red;
        private int green;
        private int blue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(RGBColor item1, RGBColor item2)
        {
            return (
                item1.Red == item2.Red
                && item1.Green == item2.Green
                && item1.Blue == item2.Blue
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(RGBColor item1, RGBColor item2)
        {
            return (
                item1.Red != item2.Red
                || item1.Green != item2.Green
                || item1.Blue != item2.Blue
                );
        }

        /// <summary>
        /// Gets or sets red value.
        /// </summary>
        public int Red
        {
            get
            {
                return red;
            }
            set
            {
                red = (value > 255) ? 255 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets red value.
        /// </summary>
        public int Green
        {
            get
            {
                return green;
            }
            set
            {
                green = (value > 255) ? 255 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets red value.
        /// </summary>
        public int Blue
        {
            get
            {
                return blue;
            }
            set
            {
                blue = (value > 255) ? 255 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public RGBColor(int R, int G, int B)
        {
            this.red = (R > 255) ? 255 : ((R < 0) ? 0 : R);
            this.green = (G > 255) ? 255 : ((G < 0) ? 0 : G);
            this.blue = (B > 255) ? 255 : ((B < 0) ? 0 : B);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (RGBColor)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Color ToRGB()
        {
            return Color.FromArgb(0, Red, Green, Blue);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class RGBColorConverter : TypeConverter
    {
    }
}