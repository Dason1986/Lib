using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;

namespace Library.ComponentModel.Model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryablePage : IEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 
        /// </summary>
        int Total { get; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryablePage<T> : IEnumerable<T>, IQueryablePage
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryablePage<T> : List<T>, IQueryablePage<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public QueryablePage(IEnumerable<T> collection, int pageIndex, int pageSize) : base(collection)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            int inndexBegin = pageIndex * pageSize;

        }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public int Total { get; protected set; }

    }
    /// <summary>
    ///
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        Guid ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        StatusCode StatusCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICreatedInfo
    {
        /// <summary>
        /// 創建日期
        /// </summary>
        DateTime Created { get;  }

        /// <summary>
        /// 創建者
        /// </summary>
        string CreatedBy { get;  }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IModifiedInfo
    {
        /// <summary>
        /// 修改日期
        /// </summary>
        DateTime Modified { get;  }

        /// <summary>
        /// 修改者
        /// </summary>
        string ModifiedBy { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditedEntity: ICreatedInfo,IModifiedInfo
    {
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiedBy"></param>
        void SetChangedInfo(string modifiedBy);
    }
}