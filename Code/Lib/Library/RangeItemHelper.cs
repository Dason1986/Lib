using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.HelperUtility;

namespace Library
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
    public class IntersectException : Exception
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
    /// <summary>
    /// 
    /// </summary>
    public static class RangeItemHelper
    {
        /// <summary>
        /// 騅兩個區間列表，沒有相交項
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="trageList"></param>
        public static void ValidateNone<T, TV>(IList<T> sourceList, IList<T> trageList = null)
            where TV : IComparable
            where T : IRangeItem<TV>
        {
            if (sourceList == null) throw new ArgumentNullException("sourceList");
            if (trageList == null) trageList = sourceList;
            bool sameList = Equals(sourceList, trageList);
            int index = 0;
            for (int i = 0; i < sourceList.Count; i++)
            {
                var sourceItem = sourceList[i];
                if (sourceItem.Begin.CompareTo(sourceItem.End) > 0) throw new IntersectException(RangeComparable.Own, sourceItem, null);
                if (sameList) index = i + 1;
                for (int j = index; j < trageList.Count; j++)
                {
                    var trageItem = trageList[j];
                    if (Equals(sourceItem, trageItem)) continue;
                    var flag = sourceItem.IsIntersect(trageItem);
                    if (!flag) continue;
                    throw new IntersectException(RangeComparable.Intersect, sourceItem, trageItem);
                }
            }
        }
        /// <summary>
        /// 判斷兩個區間是否相交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool IsIntersect<T>(this IRangeItem<T> x, IRangeItem<T> y) where T : IComparable
        {
            if (x == null || y == null) return false;
            bool tf = x.Begin.IsBetween(y.Begin, y.End) || x.End.IsBetween(y.Begin, y.End);
            if (!tf) tf = y.Begin.IsBetween(x.Begin, x.End) || y.End.IsBetween(x.Begin, x.End);
            return tf;
        }
        /// <summary>
        /// 合併相交區間元素項，返回合併過後的新區間
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static IList<MergeRange<TV>> Merge<T, TV>(IList<T> sourceList)
            where TV : IComparable
            where T : IRangeItem<TV>
        {
            // 
            List<MergeRange<TV>> list = new List<MergeRange<TV>>();
            List<T> tmplist = new List<T>();
            tmplist.AddRange(sourceList);
            Comparison<T> tt = (x, y) => { return x.Begin.CompareTo(y.Begin); };
            tmplist.Sort(tt);

            MergeRange<TV> range = null;
            List<IRangeItem<TV>> includelist = new List<IRangeItem<TV>>();
            for (int i = 0; i < tmplist.Count - 1; i++)
            {
                var x = tmplist[i];
                var y = tmplist[i + 1];
                RangeComparable compare;
                if (range == null)
                {
                    range = new MergeRange<TV>();
                    range.Begin = x.Begin;
                    range.End = x.End;
                    includelist.Add(x);
                    list.Add(range);
                    compare = GetComparableReslut(x, y);
                }
                else
                {
                    compare = GetComparableReslut(range, y);
                }

                switch (compare)
                {
                    case RangeComparable.None:
                        range.List = includelist.ToArray();
                        includelist.Clear();
                        range = new MergeRange<TV> { Begin = y.Begin, End = y.End };
                        list.Add(range);
                        includelist.Add(y);
                        break;
                    case RangeComparable.Own: throw new IntersectException(compare, x, y);
                    case RangeComparable.Include:
                    case RangeComparable.Same:
                        includelist.Add(y);
                        break;
                    case RangeComparable.RightJoin:
                        range.End = y.End;
                        includelist.Add(y);
                        break;
                    case RangeComparable.LeftJoin:
                        range.Begin = y.Begin;
                        includelist.Add(y);
                        break;
                    case RangeComparable.UnInclude:
                        range.Begin = y.Begin;
                        range.End = y.End;
                        includelist.Add(y);
                        break;

                }
            }
            if (range != null)
            {
                range.List = includelist.ToArray();
            }
            return list;

        }
        /// <summary>
        /// 取兩個區間的相交情況
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RangeComparable GetComparableReslut<T>(this IRangeItem<T> x, IRangeItem<T> y) where T : IComparable
        {
            if (x == null || y == null) return RangeComparable.None;
            if (x.Begin.CompareTo(x.End) > 0) return RangeComparable.Own;
            if (y.Begin.CompareTo(y.End) > 0) return RangeComparable.Own;
            if (x.Begin.CompareTo(y.Begin) == 0 && x.End.CompareTo(y.End) == 0) return RangeComparable.Same;
            bool pointA = x.Begin.IsBetween(y.Begin, y.End);
            bool pointB = x.End.IsBetween(y.Begin, y.End);
            bool pointC = y.Begin.IsBetween(x.Begin, x.End);
            bool pointD = y.End.IsBetween(x.Begin, x.End);
            // point為是否被對方區間包含。通過被包含的Point來確定相交情況。
            //     A     B  C     D       C  A      B  D          A C      D B         A  C      B  D       C    A      D   B
            //以X為主
            //          不相交               被對方包含             包含對方              右邊點相交            左邊點相交
            //x     └──┘                 └───┘            └────┘         └────┘              └────┘  
            //y             └──┘       └─────┘            └──┘              └────┘      └─────┘  
            if (pointA && pointB) return RangeComparable.UnInclude;
            if (pointC && pointD) return RangeComparable.Include;
            if (pointB && pointC) return RangeComparable.RightJoin;
            if (pointA && pointD) return RangeComparable.LeftJoin;
            return RangeComparable.None;
        }

    }

}
