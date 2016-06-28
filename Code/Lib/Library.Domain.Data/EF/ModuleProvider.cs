namespace Library.Domain.Data.EF
{
    public abstract class ModuleProvider : IModuleProvider
    {
        protected ModuleProvider(EFContext context)
        {
            Context = context;
            UnitOfWork = new UnitOfWork(context);
        }

        public UnitOfWork UnitOfWork { get; private set; }
        protected EFContext Context { get; private set; }

        public Repository<TEntity> CreateRepository<TEntity>() where TEntity : Entity
        {
            return new Repository<TEntity>(Context);
        }

        IRepository<TEntity> IModuleProvider.CreateRepository<TEntity>()
        {
            return this.CreateRepository<TEntity>();
        }

        IUnitOfWork IModuleProvider.UnitOfWork { get { return this.UnitOfWork; } }
    }
}