namespace Library.Domain.DomainEvents
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DomainEventHandler : IDomainEventHandler
    {


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract IDomainService OnCreateService();

        IDomainService IDomainEventHandler.CreateService()
        {
            return OnCreateService();
        }
    }
}