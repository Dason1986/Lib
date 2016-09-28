using System.Data.Common;
using System.Data.Entity;

namespace Library.Domain.Data.EF
{
    public abstract class EFContext : DbContext, IDbContext
    {

        public EFContext()
        {

        }

        protected EFContext(DbConnection existingConnection)
            : base(existingConnection, true)
        {
        }

        protected EFContext(string connection)
            : base(connection)
        {
        }
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