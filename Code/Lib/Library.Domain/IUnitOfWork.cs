using Library.ComponentModel.Model;
using Library.Domain.Data.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Library.Domain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        Guid TraceId { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// 
        /// </summary>
        void RollbackChanges();

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IVersion
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IAggregateRoot<TEntity, TKey> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        TKey Id { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext  
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity> CreateSet<TEntity>() where TEntity : class;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDbContextWrapper<TEntity>
    {
        /// <summary>
        /// 查找实体
        /// </summary>
        IQueryable<TEntity> Find();
        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        IQueryable<TEntity> FindAsNoTracking();
    }
    namespace Composite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ISpecification<T> where T : class
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="status"></param>
            /// <returns></returns>
            ISpecification<T> SetStatusCode(StatusCode status = StatusCode.Enabled);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="entity"></param>
            /// <returns></returns>
            bool IsSatisifiedBy(T entity);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Expression<Func<T, bool>> ToExpression();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="specification"></param>
            /// <returns></returns>
            ISpecification<T> And(ISpecification<T> specification);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="specification"></param>
            /// <returns></returns>
            ISpecification<T> Or(ISpecification<T> specification);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="specification"></param>
            /// <returns></returns>
            ISpecification<T> Not(ISpecification<T> specification);
        }

        public interface IExpressionSpecification<T> : ISpecification<T> where T : class
        {
            IExpressionSpecification<T> And(Func<T, bool> expression);
            IExpressionSpecification<T> Or(Func<T, bool> expression);
            IExpressionSpecification<T> Not(Func<T, bool> expression);
        }


    }
    namespace Repositorys
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



        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public interface IRepository<TEntity> : IRepository where TEntity : class, ICreatedInfo, IAggregateRoot<TEntity, Guid>
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
            IEnumerable<TEntity> GetAll();


            /// <summary>
            /// 
            /// </summary>
            /// <param name="criteria"></param>
            /// <param name="pageSize"></param>
            /// <param name="sortings"></param>
            /// <param name="includeProperties"></param>
            /// <returns></returns>
            IPagingList<TEntity> FindPageList(Expression<Func<TEntity, bool>> criteria
            , PageSizeDescriptor pageSize
            , SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="specification"></param>
            /// <returns></returns>
            IEnumerable<TEntity> Find(ISpecification<TEntity> specification);

        }



    }
    namespace ModuleProviders
    {
        /// <summary>
        /// 
        /// </summary>
        public interface IModuleProvider
        {
            /// <summary>
            /// 
            /// </summary>
            IUnitOfWork UnitOfWork { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="TEntity"></typeparam>
            /// <returns></returns>
            Repositorys.IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, ICreatedInfo, IAggregateRoot<TEntity, Guid>;
        }
    }
}