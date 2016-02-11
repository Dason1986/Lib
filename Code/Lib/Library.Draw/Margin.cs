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

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(Margin item1, Margin item2)
        {
            return (
                item1.Bottom == item2.Bottom
                && item1.Left == item2.Left
                && item1.Right == item2.Right
                && item1.Top == item2.Top
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(Margin item1, Margin item2)
        {
            return (
                item1.Bottom != item2.Bottom
                || item1.Left != item2.Left
                || item1.Right != item2.Right
                || item1.Top != item2.Top
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals((Margin)obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum PageSize
    {
        /// <summary>
        ///
        /// </summary>
        _11X17,

        /// <summary>
        ///
        /// </summary>
        A0,

        /// <summary>
        ///
        /// </summary>
        A1,

        /// <summary>
        ///
        /// </summary>
        A2,

        /// <summary>
        ///
        /// </summary>
        A3,

        /// <summary>
        ///
        /// </summary>
        A4,

        /// <summary>
        ///
        /// </summary>
        A5,

        /// <summary>
        ///
        /// </summary>
        A6,

        /// <summary>
        ///
        /// </summary>
        A7,

        /// <summary>
        ///
        /// </summary>
        A8,

        /// <summary>
        ///
        /// </summary>
        A9,

        /// <summary>
        ///
        /// </summary>
        A10,

        /// <summary>
        ///
        /// </summary>
        B0,

        /// <summary>
        ///
        /// </summary>
        B1,

        /// <summary>
        ///
        /// </summary>
        B2,

        /// <summary>
        ///
        /// </summary>
        B3,

        /// <summary>
        ///
        /// </summary>
        B4,

        /// <summary>
        ///
        /// </summary>
        B5,

        /// <summary>
        ///
        /// </summary>
        B6,

        /// <summary>
        ///
        /// </summary>
        B7,

        /// <summary>
        ///
        /// </summary>
        B8,

        /// <summary>
        ///
        /// </summary>
        B9,

        /// <summary>
        ///
        /// </summary>
        B10,
    }
}