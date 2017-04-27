namespace Library.Domain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext : System.IDisposable
    {
        //  IQueryable<TEntity> CreateSet<TEntity>() where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork();
    }
}