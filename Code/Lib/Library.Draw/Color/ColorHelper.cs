using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    ///
    /// </summary>
    public class ColorHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Color ToRGB(int color)
        {
            return Color.FromArgb(0xff & (color >> 0x10), 0xff & (color >> 8), 0xff & color);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int ToRGBValue(Color color)
        {
            int r = color.R, g = color.G, b = color.B;
            return (255 << 24) + (r << 16) + (g << 8) + b;
        }
    }
}