using Library.ComponentModel.Model;

namespace Library.Domain.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModuleProvider : System.IDisposable
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
        IRepository<TEntity> CreateRepository<TEntity>() where TEntity : Entity;


    }
}