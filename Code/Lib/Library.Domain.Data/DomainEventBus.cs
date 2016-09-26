using Library.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.Data
{
    public class DomainEventBus : IDomainEventBus
    {
        public DomainEventBus(IIoc ioc)
        {
        }
        IIoc _ioc;
        public void Publish<TEvent>(TEvent @event)
            where TEvent : IDomainEvent
        {
            Thread thread = MyMethod(@event);
            thread.Start();

        }

        private Thread MyMethod<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            Thread thread = new Thread(n =>
            {
                var eventHandler = _ioc.GetService<IEventHandler<TEvent>>();
                eventHandler.Handle(@event);
            });

            return thread;
        }

        public void PublishAwait<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            Thread thread = MyMethod(@event);
            thread.Start();
            thread.Join();
        }
        static readonly DomainEventBus _domainEventBus;//= new DomainEventBus();
        public static DomainEventBus Instance()
        {
            return _domainEventBus;
        }
    }
}
