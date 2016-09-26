using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.DomainEvents
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEvent
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventBus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <typeparam name="TEvent"></typeparam>
        void Publish<TEvent>(TEvent @event)
            where TEvent : IDomainEvent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<in TEvent>
      where TEvent : IDomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        void Handle(TEvent @event);
    }
}
