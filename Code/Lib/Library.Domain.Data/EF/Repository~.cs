using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Extensions;

namespace Library.Domain.Data.EF
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : Repository, IRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Repository(EFContext context) : base(context)
        {
            Set = CreateSet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(TEntity item)
        {
            Set.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        protected DbSet<TEntity> Set { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual DbSet<TEntity> CreateSet()
        {
            return EfContext.Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Attach(TEntity item)
        {
            EfContext.Entry(item).State = EntityState.Modified;

            if (this.Set.Find(item.ID) == null)
            {
                Set.Attach(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return CreateSet().AsNoTracking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Get(Guid id)
        {
            if (typeof(Library.ComponentModel.Model.IModifiedInfo).IsAssignableFrom(  typeof(TEntity) ))
               return Set.FirstOrDefault(n => n.ID == id);
            return Set.AsNoTracking().FirstOrDefault(n => n.ID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public virtual void Remove(Guid id)
        {
            var entity = Set.FirstOrDefault(n => n.ID == id);
            if (entity != null)
                Remove(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        public virtual int DelBatch(Expression<Func<TEntity, bool>> whereExpr)
        {
            return Set.Where(whereExpr).Delete();

        }
    }
}
