using System;
using System.Linq;
using System.Linq.Expressions;
using Library.ComponentModel.Model;

namespace Library.Domain.Data
{
    /// <summary>
    ///
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        ///
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        void Remove(object item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        void SetModified(object item);
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
        /// <param name="item"></param>
        void Add(TEntity item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        void Remove(Guid id);

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
        IQueryable<TEntity> GetAll();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetEnabledAll();

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        void Attach(TEntity item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        int DelBatch(Expression<Func<TEntity, bool>> whereExpr);
    }
}