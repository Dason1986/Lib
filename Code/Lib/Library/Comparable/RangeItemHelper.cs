using Library.Comparable;
using Library.HelperUtility;
using System;
using System.Collections.Generic;

namespace Library.Comparable
{
    /// <summary>
    /// 區間元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct RangeItem<T> : IRangeItem<T> where T : IComparable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public RangeItem(T begin, T end)
            : this()
        {
            if (begin.CompareTo(end) > 0) throw new IntersectException(RangeComparable.Own, begin, end);
            Begin = begin;
            End = end;
        }

        static RangeItem()
        {
            Empty = new RangeItem<T>(default(T), default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Begin.CompareTo(Empty.Begin) == End.CompareTo(Empty.End);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static RangeItem<T> Empty { get; private set; }

        /// <summary>
        /// 開始值
        /// </summary>
        public T Begin { get; private set; }

        /// <summary>
        /// 結束值
        /// </summary>
        public T End { get; private set; }

        /// <summary>
        /// Check if the specified value is inside of the range.
        /// </summary>
        ///
        /// <param name="x">Value to check.</param>
        ///
        /// <returns><b>True</b> if the specified value is inside of the range or
        /// <b>false</b> otherwise.</returns>
        ///
        public bool IsInside(T x)
        {
            return x.IsBetween(Begin, End);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0} , {1}]", Begin, End);
        }
    }

    /// <summary>
    /// 區間元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MergeRange<T> : IRangeItem<T> where T : IComparable
    {
        internal MergeRange()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public MergeRange(T begin, T end)
        {
            Begin = begin;
            End = end;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="list"></param>
        public MergeRange(T begin, T end, IRangeItem<T>[] list)
        {
            Begin = begin;
            End = end;
            List = list;
        }

        /// <summary>
        ///
        /// </summary>
        public IRangeItem<T>[] List { get; internal set; }

        /// <summary>
        /// 開始值
        /// </summary>
        public T Begin { get; internal set; }

        /// <summary>
        /// 結束值
        /// </summary>
        public T End { get; internal set; }

        /// <summary>
        /// Check if the specified value is inside of the range.
        /// </summary>
        ///
        /// <param name="x">Value to check.</param>
        ///
        /// <returns><b>True</b> if the specified value is inside of the range or
        /// <b>false</b> otherwise.</returns>
        ///
        public bool IsInside(T x)
        {
            return x.IsBetween(Begin, End);
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class IntersectException : LibException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="sourceItem"></param>
        /// <param name="trageItem"></param>
        public IntersectException(RangeComparable exceptionType, object sourceItem, object trageItem)
            : base(exceptionType.ToString())
        {
            Reslut = exceptionType;
            SourceItem = sourceItem;
            TrageItem = trageItem;
        }

        /// <summary>
        ///
        /// </summary>
        public object SourceItem { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public object TrageItem { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public RangeComparable Reslut { get; protected set; }
    }

    /// <summary>
    /// 區間比較
    /// </summary>
    [Flags]
    public enum RangeComparable
    {
        /// <summary>
        /// 沒相交
        /// </summary>
        None = 0,

        /// <summary>
        /// 數值出錯
        /// </summary>
        Own = 1,

        /// <summary>
        /// 左邊相交
        /// </summary>
        LeftJoin = 2,

        /// <summary>
        /// 右邊相交
        /// </summary>
        RightJoin = 4,

        /// <summary>
        /// 包含對方
        /// </summary>
        Include = 8,

        /// <summary>
        /// 被對方包含
        /// </summary>
        UnInclude = 16,

        /// <summary>
        /// 相交
        /// </summary>
        Intersect = LeftJoin | RightJoin | Include | UnInclude,

        /// <summary>
        /// 完全一致
        /// </summary>
        Same = 64
    }
}