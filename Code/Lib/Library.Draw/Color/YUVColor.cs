using System;
using System.ComponentModel;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public class YUVColorConverter : TypeConverter
    {

    }
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(CMYKColorConverter))]
    public struct YUVColor
    {
        /// <summary>
        /// Gets an empty YUV structure.
        /// </summary>
        public static readonly YUVColor Empty = new YUVColor();

        private double y;
        private double u;
        private double v;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(YUVColor item1, YUVColor item2)
        {
            return (
                item1.Y == item2.Y
                && item1.U == item2.U
                && item1.V == item2.V
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(YUVColor item1, YUVColor item2)
        {
            return (
                item1.Y != item2.Y
                || item1.U != item2.U
                || item1.V != item2.V
                );
        }
        /// <summary>
        /// 
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            private set
            {
                y = value;
                y = (y > 1) ? 1 : ((y < 0) ? 0 : y);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double U
        {
            get
            {
                return u;
            }
            private set
            {
                u = value;
                u = (u > 0.436) ? 0.436 : ((u < -0.436) ? -0.436 : u);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double V
        {
            get
            {
                return v;
            }
            private set
            {
                v = value;
                v = (v > 0.615) ? 0.615 : ((v < -0.615) ? -0.615 : v);
            }
        }

        /// <summary>
        /// Creates an instance of a YUV structure.
        /// </summary>
        public YUVColor(double y, double u, double v)
        {
            this.y = (y > 1) ? 1 : ((y < 0) ? 0 : y);
            this.u = (u > 0.436) ? 0.436 : ((u < -0.436) ? -0.436 : u);
            this.v = (v > 0.615) ? 0.615 : ((v < -0.615) ? -0.615 : v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (YUVColor)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Y.GetHashCode() ^ U.GetHashCode() ^ V.GetHashCode();
        }

        /// <summary>
        /// Converts YUV to RGB.
        /// </summary> 
        public Color ToRGB()
        {


            return Color.FromArgb(Convert.ToInt32((y + 1.139837398373983740 * v) * 255),
                                  Convert.ToInt32((y - 0.3946517043589703515 * u - 0.5805986066674976801 * v) * 255),
                                  Convert.ToInt32((y + 2.032110091743119266 * u) * 255)
                );
        }
        /// <summary>
        /// Converts RGB to YUV.
        /// </summary>
        /// <param name="red">Red must be in [0, 255].</param>
        /// <param name="green">Green must be in [0, 255].</param>
        /// <param name="blue">Blue must be in [0, 255].</param>
        public static YUVColor FromRGB(int red, int green, int blue)
        {
            YUVColor yuv = new YUVColor();

            // normalizes red, green, blue values
            double r = (double)red / 255.0;
            double g = (double)green / 255.0;
            double b = (double)blue / 255.0;

            yuv.Y = 0.299 * r + 0.587 * g + 0.114 * b;
            yuv.U = -0.14713 * r - 0.28886 * g + 0.436 * b;
            yuv.V = 0.615 * r - 0.51499 * g - 0.10001 * b;

            return yuv;
        }
    }
}