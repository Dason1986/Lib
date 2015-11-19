using System;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRangeItem<T> where T : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        T Begin { get; }
        /// <summary>
        /// 
        /// </summary>
        T End { get; }

        /// <summary>
        /// Check if the specified value is inside of the range.
        /// </summary>
        /// 
        /// <param name="x">Value to check.</param>
        /// 
        /// <returns><b>True</b> if the specified value is inside of the range or
        /// <b>false</b> otherwise.</returns>
        /// 
        bool IsInside(T x);
    }
}