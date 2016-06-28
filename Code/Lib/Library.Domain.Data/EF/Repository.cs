using System.Data.Entity;

namespace Library.Domain.Data.EF
{
    /// <summary>
    /// 
    /// </summary>
    public class Repository : IRepository
    {
        public Repository(EFContext context) 
        {
            EfContext = context;
            UnitOfWork = context.CreateUnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        protected UnitOfWork UnitOfWork { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        protected EFContext EfContext { get; private set; }

        IUnitOfWork IRepository.UnitOfWork
        {
            get { return UnitOfWork; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual  void SetModified(object item)
        {
            if (item == null) return;
            EfContext.Entry(item).State = EntityState.Modified;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Remove(object item)
        {
            if (item == null) return;
            EfContext.Entry(item).State = EntityState.Deleted;
        }

     
    }
}