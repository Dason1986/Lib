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
    public interface IEntity : IAggregateRoot<Guid>
    {
      
        /// <summary>
        /// 
        /// </summary>
        StatusCode StatusCode { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IAggregateRoot<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        TKey ID { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityDeleted
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manaegment"></param>
        void Delete(ICreatedInfo manaegment);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ICreatedInfo
    {
        /// <summary>
        /// 創建日期
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// 創建者
        /// </summary>
        string CreatedBy { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IModifiedInfo
    {
        /// <summary>
        /// 修改日期
        /// </summary>
        DateTime Modified { get; }

        /// <summary>
        /// 修改者
        /// </summary>
        string ModifiedBy { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditedEntity : IEntity, ICreatedInfo, IModifiedInfo
    {


    }
}