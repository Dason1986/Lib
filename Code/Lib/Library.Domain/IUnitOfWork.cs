using Library.ComponentModel.Model;
using Library.Domain.Data.Composite;
using Library.HelperUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library.Domain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork : IDisposable
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
    public interface ISqlCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
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
    public interface IDbContext : IDisposable
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
    public interface IDbContextWrapper<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> CreateSet();

        /// <summary>
        /// 查找实体
        /// </summary>
        IQueryable<TEntity> Find();
        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        IQueryable<TEntity> FindAsNoTracking();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="sortings"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(ISpecification<TEntity> specification, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="sortings"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IPagingList<TEntity> FindPageList(Expression<Func<TEntity, bool>> criteria,
         PageSizeDescriptor page, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);
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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        public interface ISpecification<T, TKey> where T : class, IAggregateRoot<TKey>, ICreatedInfo
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="daterange_begin"></param>
            /// <param name="datarange_end"></param>
            /// <returns></returns>
            ISpecification<T> AddCreateDataRange(string daterange_begin, string datarange_end);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="daterange_begin"></param>
            /// <param name="datarange_end"></param>
            /// <returns></returns>
            ISpecification<T> AddCreateDataRange(DateTime daterange_begin, DateTime datarange_end); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IExpressionSpecification<T> : ISpecification<T> where T : class
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            IExpressionSpecification<T> And(Expression<Func<T, bool>> expression);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            IExpressionSpecification<T> Or(Expression<Func<T, bool>> expression);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            IExpressionSpecification<T> Not(Expression<Func<T, bool>> expression);
        }


    }
    namespace Repositorys
    {
        /// <summary>
        /// 
        /// </summary>
        public interface IRepository:ISqlCommand
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
        /// <typeparam name="TKey"></typeparam>
        public interface IRepository<TEntity, TKey> : IRepository where TEntity : class, ICreatedInfo, IAggregateRoot<TKey>
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
            void Remove(TKey id);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            TEntity Get(TKey id);
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
            /// <param name="sortings"></param>
            /// <param name="includeProperties"></param>
            /// <returns></returns>
            IEnumerable<TEntity> Find(ISpecification<TEntity> specification, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : class, ICreatedInfo, IAggregateRoot<Guid>
        {


        }



    }
    namespace ModuleProviders
    {
        public interface IModuleProvider
        {
            IUnitOfWork UnitOfWork { get; }


        }
    }
}