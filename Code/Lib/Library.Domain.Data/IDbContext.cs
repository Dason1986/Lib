namespace Library.Domain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext
    {
        //  IQueryable<TEntity> CreateSet<TEntity>() where TEntity : class;
        IUnitOfWork CreateUnitOfWork();
    }
}