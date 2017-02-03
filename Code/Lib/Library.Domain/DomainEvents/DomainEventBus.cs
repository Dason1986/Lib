using Library.Domain.Data;
using Library.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class DomainEventPublish<TService>
        where TService : IDomainService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="domainModuleProvider"></param>
        public DomainEventPublish(IDomainModuleProvider domainModuleProvider)
        {
            if (domainModuleProvider == null) throw new Exception("DomainModuleProvider 爲空");
            DomainModuleProvider = domainModuleProvider;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IDomainModuleProvider DomainModuleProvider { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void Publish(IDomainEventArgs args)
        {
            Thread thread = new Thread(n =>
            {
                var service = Bootstrap.Currnet.GetService<TService>();
                service.DomainModuleProvider = DomainModuleProvider;
                service.Handle(args);
                DomainModuleProvider.UnitOfWork.Commit();
            });
            thread.Start();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class DomainEventBus : IDomainEventBus
    {
        /// <summary>
        ///
        /// </summary>
        public DomainEventBus()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IDomainModuleProvider DomainModuleProvider { get; set; }

        private IList<IDomainEventHandler> handers = new List<IDomainEventHandler>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventHandler"></param>
        public void AddEvent(DomainEventHandler eventHandler)
        {
            handers.Add(eventHandler);
        }

        void IDomainEventBus.AddEvent(IDomainEventHandler eventHandler)
        {
            AddEvent(eventHandler as DomainEventHandler);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Publish()
        {
            PublishThread().Start();
        }

        private Thread PublishThread()
        {
            if (DomainModuleProvider == null) throw new Exception("DomainModuleProvider 爲空");
            Thread thread = new Thread(n =>
            {
                foreach (var item in handers)
                {
                    var service = item.CreateService();
                    service.DomainModuleProvider = DomainModuleProvider;
                    service.Handle(item.Args);
                }
                DomainModuleProvider.UnitOfWork.Commit();
                handers.Clear();
            });

            return thread;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void PublishAwait()
        {
            Thread th = PublishThread();
            th.Start();
            th.Join();
        }
    }
}