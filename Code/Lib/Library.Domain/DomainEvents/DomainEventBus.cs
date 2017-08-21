using Library.Domain.Data;
using Library.Domain.Data.ModuleProviders;
using Library.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.InThread
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class DomainEventPublish<TService> : IDomainEventPublish
        where TService : IDomainService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="domainModuleProvider"></param>
        public DomainEventPublish(IModuleProvider domainModuleProvider)
        {
            if (domainModuleProvider == null) throw new Exception("ModuleProvider 爲空");
            ModuleProvider = domainModuleProvider;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IModuleProvider ModuleProvider { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void Publish(DomainEventArgs args)
        {
            Thread thread = new Thread(n =>
            {
                var service = Bootstrap.Currnet.GetService<TService>();
                service.ModuleProvider = ModuleProvider;
                service.Handle(args);
                ModuleProvider.UnitOfWork.Commit();
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
        public virtual IModuleProvider ModuleProvider { get; set; }

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
        public virtual void Publish(DomainEventArgs args)
        {
            PublishThread(args).Start();
        }

        private Thread PublishThread(DomainEventArgs args)
        {
            if (ModuleProvider == null) throw new Exception("ModuleProvider 爲空");
            Thread thread = new Thread(n =>
            {
                foreach (var item in handers)
                {
                    var service = item.CreateService();
                    service.ModuleProvider = ModuleProvider;
                    service.Handle(args);
                }
                ModuleProvider.UnitOfWork.Commit();
                handers.Clear();
            });

            return thread;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void PublishAwait(DomainEventArgs args)
        {
            Thread th = PublishThread(args);
            th.Start();
            th.Join();
        }
    }
}

namespace Library.Domain.EF
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class EfDomainEventPublish<TService> : IDomainEventPublish
        where TService : IDomainService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="domainModuleProvider"></param>
        public EfDomainEventPublish(IModuleProvider domainModuleProvider)
        {
            if (domainModuleProvider == null) throw new Exception("ModuleProvider 爲空");
            ModuleProvider = domainModuleProvider;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IModuleProvider ModuleProvider { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void Publish(DomainEventArgs args)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class EfDomainEventBus : IDomainEventBus
    {
        /// <summary>
        ///
        /// </summary>
        public EfDomainEventBus()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IModuleProvider ModuleProvider { get; set; }

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
            if (eventHandler is DomainEventHandler == false) throw new Exception();
            AddEvent(eventHandler as DomainEventHandler);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Publish(DomainEventArgs args)
        {
            throw new NotImplementedException();
        }


    }
}