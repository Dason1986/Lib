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
    public interface IUnitOfWork : IDisposable, ISqlCommand
    {
        Guid TraceId { get; }

        int Commit();

        void RollbackChanges();

    


    }
    public interface ISqlCommand
    {
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
    }
    public interface IVersion
    {
    }


    public interface IDbContext : IDisposable
    {
        IQueryable<TEntity> CreateSet<TEntity>() where TEntity : class;
    }
    public interface IDbContextWrapper<TEntity> where TEntity : class
    {
        IQueryable<TEntity> CreateSet();

        /// <summary>
        /// 查找实体
        /// </summary>
        IQueryable<TEntity> Find();
        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        IQueryable<TEntity> FindAsNoTracking();

        IEnumerable<TEntity> Find(ISpecification<TEntity> specification, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);

        IPagingList<TEntity> FindPageList(Expression<Func<TEntity, bool>> criteria,
         PageSizeDescriptor page, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);
    }
    namespace Composite
    {
        public interface ISpecification<T> where T : class
        {


            bool IsSatisifiedBy(T entity);
            Expression<Func<T, bool>> ToExpression();
            ISpecification<T> And(ISpecification<T> specification);
            ISpecification<T> Or(ISpecification<T> specification);
            ISpecification<T> Not(ISpecification<T> specification);
        }
        public interface ISpecification<T, TKey> where T : class 
        {
            ISpecification<T> AddCreateDataRange(string daterange_begin, string datarange_end);
            ISpecification<T> AddCreateDataRange(DateTime daterange_begin, DateTime datarange_end);
            ISpecification<T> SetStatusCode(StatusCode status = StatusCode.Enabled);
            ISpecification<T> SetStatusCode(StatusCode[] status);
        }
        public interface IExpressionSpecification<T> : ISpecification<T> where T : class
        {
            IExpressionSpecification<T> And(Expression<Func<T, bool>> expression);
            IExpressionSpecification<T> Or(Expression<Func<T, bool>> expression);
            IExpressionSpecification<T> Not(Expression<Func<T, bool>> expression);
        }


    }
    namespace Repositorys
    {
        public interface IRepository
        {
            IUnitOfWork UnitOfWork { get; }



        }
         
        public interface IRepository<TEntity, TKey> : IRepository where TEntity : class, ICreatedInfo, IAggregateRoot< TKey>
        {
            void Add(TEntity item);

            void Remove(TKey id);

            TEntity Get(TKey id);

            IEnumerable<TEntity> GetAll();



            IPagingList<TEntity> FindPageList(Expression<Func<TEntity, bool>> criteria
            , PageSizeDescriptor pageSize
            , SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);

            IEnumerable<TEntity> Find(ISpecification<TEntity> specification, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties);

        }
        public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : class, ICreatedInfo, IAggregateRoot< Guid>
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