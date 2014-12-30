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
        /// <param name="trageList"></param>
        /// <returns></returns>
        public static IList<RangeItem<TV>> Merge<T, TV>(IList<T> sourceList, IList<T> trageList)
            where TV : IComparable
            where T : IRangeItem<TV>
        {
            List<RangeItem<TV>> list = new List<RangeItem<TV>>();
            List<T> tmplist = new List<T>();
            tmplist.AddRange(sourceList);
            tmplist.AddRange(trageList);
            tmplist.Sort();
            throw new NotImplementedException();
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
            bool pointA = x.Begin.IsBetween(y.Begin, y.End);
            bool pointB = x.End.IsBetween(y.Begin, y.End);
            bool pointC = y.Begin.IsBetween(x.Begin, x.End);
            bool pointD = y.End.IsBetween(x.Begin, x.End);
            // point為是否被對方區間包含。通過被包含的Point來確定相交情況。
            //     A   B C  D       C  A  B  D         A C  D B         A  C   B  D      C    A   D   B
            //     └──┘                └────┘            └────┘         └──────┘              └──────┘  
            //           └──┘       └─────┘            └────┘              └──────┘      └────────┘  
            if (pointA && pointB) return RangeComparable.UnInclude;
            if (pointC && pointD) return RangeComparable.Include;
            if (pointB && pointC) return RangeComparable.RightJoin;
            if (pointA && pointD) return RangeComparable.LeftJoin;
            return RangeComparable.None;
        }

    }

}
