using System.Linq;

namespace Library.Domain.Data
{
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
}