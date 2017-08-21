namespace Library.Domain.DomainEvents
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class DomainEventHandler<TService> : DomainEventHandler, IDomainEventHandler<TService> where TService : IDomainService
    {
      

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual TService CreateService()
        {
            return Library.Bootstrap.Currnet.GetService<TService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override IDomainService OnCreateService()
        {
            return CreateService();
        }

        IDomainService IDomainEventHandler.CreateService()
        {
            return CreateService();
        }
    }
}