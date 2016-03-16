using Library.ComponentModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRepository : IDisposable
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository where TEntity : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        IUnitOfWork Unit { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Add(TEntity item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Remove(TEntity item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryablePage<TEntity> GetFilter(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryablePage<TEntity> GetFilter(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        void Commit();
        /// <summary>
        /// 
        /// </summary>
        void RollbackChanges();
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IRepositoryProvider
    {
        /// <summary>
        /// 
        /// </summary>
        IUnitOfWork Unit { get; }
    }
}
