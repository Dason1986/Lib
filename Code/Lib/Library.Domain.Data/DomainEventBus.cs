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
        public DomainEventBus()
        {
        }

        public void Publish<TService>(IDomainEventArgs args)

            where TService : IDomainService
        {
            Thread thread = MyMethod<TService, IDomainEventArgs>(args);
            thread.Start();

        }

        private Thread MyMethod<TService, TEvent>(TEvent args) where TService : IDomainService where TEvent : IDomainEventArgs
        {
            Thread thread = new Thread(n =>
            {
                var service = Bootstrap.Currnet.GetService<TService>();
                service.Handle(args);
            });

            return thread;
        }

        public void PublishAwait<TService>(IDomainEventArgs args)

            where TService : IDomainService
        {
            Thread thread = MyMethod<TService, IDomainEventArgs>(args);
            thread.Start();
            thread.Join();
        }
        IList<IDomainEventHandler> handers = new List<IDomainEventHandler>();
        public void AddEvent(IDomainEventHandler eventHandler)
        {
            handers.Add(eventHandler);
        }

        public void Publish()
        {
            Thread thread = new Thread(n =>
           {
               foreach (var item in handers)
               {
                   var service = item.CreateService();
                   service.Handle(item.Args);
               }
               handers.Clear();
           });
            thread.Start();
        }
    }
}
