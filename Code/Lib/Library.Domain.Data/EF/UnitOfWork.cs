using Library.ComponentModel.Model;
using Library.Domain.Data.Composite;
using Library.Domain.Data.ModuleProviders;
using Library.Domain.Data.Repositorys;
using Library.DynamicCode;
using Library.HelperUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.Data.EF
{
    /// <summary>
    /// Ef工作单元扩展
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void ClearCache(this IUnitOfWork unitOfWork)
        {
            var dbContext = unitOfWork as DbContext;
            if (dbContext != null && dbContext.ChangeTracker != null)
                dbContext.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Detached);
        }
    }
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbconntext;

        public Guid TraceId { get; private set; }

        public UnitOfWork(DbContext dbconntext)
        {
            if (dbconntext == null) throw new ArgumentNullException("dbconntext");
            _dbconntext = dbconntext;
            TraceId = Guid.NewGuid();

        }

        protected internal DbContext Dbconntext { get { return _dbconntext; } }

        #region IQueryableUnitOfWork

        public int Commit()
        {
            SaveChangesBefore();
            return _dbconntext.SaveChanges();


        }
        #region CommitAsync(异步提交)
        /// <summary>
        /// 异步提交,返回影响的行数
        /// </summary>
        public async Task<int> CommitAsync()
        {
            try
            {
                return await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DBConcurrencyException("Commit error", ex);
            }
        }
        #endregion

        #region SaveChangesAsync(异步保存更改)
        /// <summary>
        /// 异步保存更改
        /// </summary>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SaveChangesBefore();
            return _dbconntext.SaveChangesAsync(cancellationToken);
        }
        #endregion
        #region SaveChanges(保存更改)



        /// <summary>
        /// 保存更改前操作
        /// </summary>
        protected virtual void SaveChangesBefore()
        {
            if (InterceptManagment.GetInterceptCount() == 0) return;

            var intercepts = InterceptManagment.GetIntercept();
            foreach (var entry in this.Dbconntext.ChangeTracker.Entries())
            {
                foreach (var item in intercepts)
                {
                    if (item.CanVerification(entry))
                        item.Operation(entry);
                }

            }

        }


        #endregion



        public void RollbackChanges()
        {
            _dbconntext.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }



        public void Dispose()
        {
            if (Dbconntext == null || Dbconntext.Database == null || Dbconntext.Database.Connection == null) return;
            if (Dbconntext.Database.Connection.State == ConnectionState.Open)
            {
                Dbconntext.Database.Connection.Close();
                Dbconntext.Dispose();
                _dbconntext = null;
            }

        }

        #endregion IQueryableUnitOfWork
    }
    public interface ISqlCommand
    {
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
        DataTable ExecuteQuery(string sqlQuery, params object[] parameters);
    }
    public interface IVersion
    {
    }
    public static class InterceptManagment
    {
        static List<IInterceptOperation> List = new List<IInterceptOperation>();
        public static void Add(IInterceptOperation interceptOperation)
        {
            if (interceptOperation == null)
            {
                throw new ArgumentNullException(nameof(interceptOperation));
            }

            List.Add(interceptOperation);
        }

        public static int GetInterceptCount()
        {
            return List.Count;
        }

        public static IEnumerable<IInterceptOperation> GetIntercept()
        {
            return List;
        }
    }
    public interface IInterceptOperation
    {
        EntityState State { get; }
        bool CanVerification(DbEntityEntry entry);
        void Operation(DbEntityEntry entry);
    }
    public class DbContextWrapper<TEntity, TKey> : IDbContextWrapper<TEntity>
        where TEntity : class, ICreatedInfo, IAggregateRoot<TKey>

    {
        public DbContextWrapper(UnitOfWork unit)
        {
            Unit = unit;
            sets = CreateSet();
        }

        public UnitOfWork Unit { get; }
        DbSet<TEntity> sets;
        public DbSet<TEntity> CreateSet()
        {
            return Unit.Dbconntext.Set<TEntity>();
        }

        IQueryable<TEntity> IDbContextWrapper<TEntity>.CreateSet()
        {
            return CreateSet();
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        public IQueryable<TEntity> Find()
        {
            return CreateSet();
        }
        /// <summary>
        /// 获取未跟踪的实体集
        /// </summary>
        public IQueryable<TEntity> FindAsNoTracking()
        {
            return CreateSet().AsNoTracking();
        }
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id">实体标识</param>
        public TEntity Find(TKey id)
        {
            if (id == null)
                return null;
            return CreateSet().Find(id);
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id">实体标识</param>
        public void Remove(TKey id)
        {
            if (id == null) return;

            var set = CreateSet();
            var entity = set.Find(id);
            if (entity != null) set.Remove(entity);
        }
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id">实体标识</param>
        public async Task<TEntity> FindAsync(TKey id)
        {
            if (id == null)
                return null;
            return await CreateSet().FindAsync(id);
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="ids">实体标识集合</param>
        public IList<TEntity> FindByIds(params TKey[] ids)
        {
            return FindByIds((IEnumerable<TKey>)ids);
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="ids">实体标识集合</param>
        public IList<TEntity> FindByIds(IEnumerable<TKey> ids)
        {
            if (ids == null)
                return null;
            var array = ids.ToList();
            return FindAsNoTracking().Where(t => array.Contains(t.ID)).ToList();
        }

        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="ids">实体标识集合</param>
        public async Task<IList<TEntity>> FindByIdsAsync(params TKey[] ids)
        {
            return await FindByIdsAsync((IEnumerable<TKey>)ids);
        }
        /// <summary>
        /// 查找实体集合
        /// </summary>
        /// <param name="ids">实体标识集合</param>
        public async Task<IList<TEntity>> FindByIdsAsync(IEnumerable<TKey> ids)
        {
            if (ids == null)
                return null;
            return await FindAsNoTracking().Where(t => ids.Contains(t.ID)).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void Add(TEntity item)
        {
            sets.Add(item);
        }
        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        public void Add(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            sets.AddRange(entities);
        }


        public IEnumerable<TEntity> Find(ISpecification<TEntity> specification, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.FindAsNoTracking().Where(specification.ToExpression());
            int index = 0;
            #region sortings
            if (sortings != null && sortings.Length > 0)
            {
                foreach (var sorting in sortings)
                {
                    if (string.IsNullOrWhiteSpace(sorting.Field))
                        continue;
                    if (sorting.Direction == SortDescriptor.SortingDirection.Ascending)
                    {
                        query = CallMethod(query, index == 0 ? "OrderBy" : "ThenBy", sorting.Field);
                        index++;
                    }
                    else if (sorting.Direction == SortDescriptor.SortingDirection.Descending)
                    {
                        query = CallMethod(query, index == 0 ? "OrderByDescending" : "ThenByDescending", sorting.Field);
                        index++;
                    }
                }
            }
            if (index == 0)
            {
                query = query.OrderByDescending(n => n.Created);
            }
            #endregion

            #region 子屬性 join sql

            if (includeProperties != null && includeProperties.Any())
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            #endregion
            return query.ToList();
        }
        public IPagingList<TEntity> FindPageList(Expression<Func<TEntity, bool>> criteria,
          PageSizeDescriptor page, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            int index = 0;
            IQueryable<TEntity> query = this.FindAsNoTracking();
            query = query.Where(criteria);
            var totalRowCount = query.Count();
            #region sortings
            if (sortings != null && sortings.Length > 0)
            {
                foreach (var sorting in sortings)
                {
                    if (string.IsNullOrWhiteSpace(sorting.Field))
                        continue;
                    if (sorting.Direction == SortDescriptor.SortingDirection.Ascending)
                    {
                        query = CallMethod(query, index == 0 ? "OrderBy" : "ThenBy", sorting.Field);
                        index++;
                    }
                    else if (sorting.Direction == SortDescriptor.SortingDirection.Descending)
                    {
                        query = CallMethod(query, index == 0 ? "OrderByDescending" : "ThenByDescending", sorting.Field);
                        index++;
                    }
                }
            }
            if (index == 0)
            {
                query = query.OrderByDescending(n => n.Created);
            }
            #endregion

            #region 子屬性 join sql

            if (includeProperties != null && includeProperties.Any())
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            #endregion
            #region pagging
            query = query.Skip(page.Index * page.Size);
            query = query.Take(page.Size);
            #endregion
            var sourcelist = query.ToArray();
            var pagingList = new PagingList<TEntity>(sourcelist, page, totalRowCount);
            return pagingList;

        }
        #region CallMethod 


        /// <summary> 
        /// Calls the method. 
        /// </summary> 
        /// <param name="query">The query.</param> 
        /// <param name="methodName">Name of the method.</param> 
        /// <param name="memberName">Name of the member.</param> 
        /// <returns></returns> 
        protected virtual IOrderedQueryable<TEntity> CallMethod(IQueryable<TEntity> query, string methodName, string memberName)
        {
            var typeParams = new ParameterExpression[] { Expression.Parameter(typeof(TEntity), "") };


            System.Reflection.PropertyInfo pi = typeof(TEntity).GetProperty(memberName);


            return (IOrderedQueryable<TEntity>)query.Provider.CreateQuery(
                 Expression.Call(
                     typeof(Queryable),
                     methodName,
                     new Type[] { typeof(TEntity), pi.PropertyType },
                     query.Expression,
                     Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams))
             );
        }


        #endregion
    }
    public abstract class Repository : IRepository
    {
        protected Repository([ChecklArgsNulAttribute]DbContext dbcontext)
        {
            if (dbcontext == null) throw new ArgumentNullException("dbcontext");
            if (dbcontext.Database == null || dbcontext.Database.Connection == null) throw new ArgumentNullException("Connection");
            DbContext = dbcontext;
            UnitOfWork = new UnitOfWork(dbcontext);

        }
        [ExportAttribute]
        public IUnitOfWork UnitOfWork { get; private set; }

        [ExportAttribute]
        protected DbContext DbContext
        {
            get; private set;
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<TEntity>(sqlQuery, parameters).ToArray();
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
    }

    public abstract class Repository<TEntity, TKey> : Repository, IRepository<TEntity, TKey> where TEntity : class, ICreatedInfo, IAggregateRoot<TKey>
    {
        private readonly DbContextWrapper<TEntity, TKey> _wrapper;
        protected DbContextWrapper<TEntity, TKey> Wrapper { get { return _wrapper; } }
        public Repository([ChecklArgsNulAttribute]DbContext dbcontext) : base(dbcontext)
        {
            _wrapper = new DbContextWrapper<TEntity, TKey>(this.UnitOfWork as UnitOfWork);

        }
        public IPagingList<TEntity> FindPageList(Expression<Func<TEntity, bool>> criteria,
           PageSizeDescriptor page, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Wrapper.FindPageList(criteria, page, sortings, includeProperties);

        }


        public virtual TEntity Get(TKey id)
        {
            return Wrapper.Find(id);
        }



        public void Remove(TKey id)
        {
            Wrapper.Remove(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var set = Wrapper.FindAsNoTracking();
            return set;
        }

        public IEnumerable<TEntity> Find(ISpecification<TEntity> specification, SortDescriptor[] sortings, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Wrapper.Find(specification, sortings, includeProperties);

        }

        public void Add(TEntity item)
        {
            Wrapper.Add(item);
        }
    }
    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity> where TEntity : Entity
    {
        public Repository([ChecklArgsNulAttribute]DbContext dbcontext) : base(dbcontext)
        {

        }






    }

    public abstract class ModuleProvider : IModuleProvider
    {
        protected ModuleProvider([ChecklArgsNulAttribute]DbContext context)
        {
            DbContext = context;
            UnitOfWork = new UnitOfWork(context);
        }
        [ExportAttribute]
        public UnitOfWork UnitOfWork { get; private set; }
        [ExportAttribute]
        protected internal DbContext DbContext { get; private set; }

        protected Repository<TEntity> CreateRepository<TEntity>() where TEntity : Entity
        {
            return new Repository<TEntity>(DbContext);
        }


        IUnitOfWork IModuleProvider.UnitOfWork { get { return this.UnitOfWork; } }
    }



}
