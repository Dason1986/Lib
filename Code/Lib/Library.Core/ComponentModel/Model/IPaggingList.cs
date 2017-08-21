using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.ComponentModel.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPagingList
    {
        /// <summary>
        /// 
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        int Total { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable Items { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagingList<T> : IPagingList
    {
        /// <summary>
        /// 
        /// </summary>
        new IEnumerable<T> Items { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PagingList<T> : IPagingList<T>
    {

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paging"></param>
        /// <param name="total"></param>
        public PagingList(IEnumerable<T> source, PageSizeDescriptor paging, int total)
        {
            this.Items = source;
            PageIndex = paging.Index;
            PageSize = paging.Size;
            Total = total;
        }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Items { get;private set; }

        IEnumerable IPagingList.Items
        {
            get
            {
                return Items;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct QueryDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FilterDescriptor[] Filters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SortDescriptor[] Sorting { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct FilterDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct PageSizeDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Size { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct SortDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public SortingDirection Direction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public enum SortingDirection
        {
            /// <summary>
            /// 
            /// </summary>
            Ascending,
            /// <summary>
            /// 
            /// </summary>
            Descending
        }
    }
}
