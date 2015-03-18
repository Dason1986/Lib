using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public struct Margin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <param name="Top"></param>
        /// <param name="Bottom"></param>
        public Margin(float Left, float Right, float Top, float Bottom)
            : this()
        {
            this.Left = Left;
            this.Right = Right;
            this.Top = Top;
            this.Bottom = Bottom;
        }
        /// <summary>
        /// 0,0,0,0
        /// </summary>
        public static readonly Margin Empty = new Margin(0, 0, 0, 0);


        /// <summary>
        /// 5,5,5,5
        /// </summary>
        public static readonly Margin M5 = new Margin(75, 75, 75, 75);
        /// <summary>
        /// 
        /// </summary>
        public float Bottom { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public float Left { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public float Right { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public float Top { get; private set; }

        public static bool operator ==(Margin item1, Margin item2)
        {
            return (
                item1.Bottom == item2.Bottom
                && item1.Left == item2.Left
                && item1.Right == item2.Right
                && item1.Top == item2.Top
                );
        }

        public static bool operator !=(Margin item1, Margin item2)
        {
            return (
                item1.Bottom != item2.Bottom
                || item1.Left != item2.Left
                || item1.Right != item2.Right
                || item1.Top != item2.Top
                );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PageSize
    {
        _11X17,
        A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10,
        B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10,
    }
}
