using System;
using System.Data.Entity;
using System.Linq;

namespace Library.Domain.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbconntext;

        public UnitOfWork(DbContext dbconntext)
        {
            if (dbconntext == null) throw new ArgumentNullException("dbconntext");
            _dbconntext = dbconntext;
        }

        #region IQueryableUnitOfWork

        public void Commit()
        {

            _dbconntext.SaveChanges();


        }

        public void RollbackChanges()
        {
            _dbconntext.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        #endregion IQueryableUnitOfWork
    }
}