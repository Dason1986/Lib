using System.Data.Entity;

namespace Library.Domain.Data.EF
{
    public abstract class EFContext : DbContext, IDbContext
    {
        public UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(this);
        }

        IUnitOfWork IDbContext.CreateUnitOfWork()
        {
            return CreateUnitOfWork();
        }
    }
}