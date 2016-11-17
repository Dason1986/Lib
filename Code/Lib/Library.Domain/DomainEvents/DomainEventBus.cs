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
        /// <param name="moduleProvider"></param>
        public DomainEventPublish(IModuleProvider moduleProvider)
        {
            if (moduleProvider == null) throw new Exception("ModuleProvider 爲空");
            ModuleProvider = moduleProvider;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual IModuleProvider ModuleProvider { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Publish(IDomainEventArgs args)
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

        IList<IDomainEventHandler> handers = new List<IDomainEventHandler>();
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
            if (ModuleProvider == null) throw new Exception("ModuleProvider 爲空");
            Thread thread = new Thread(n =>
            {

                foreach (var item in handers)
                {
                    var service = item.CreateService();
                    service.ModuleProvider = ModuleProvider;
                    service.Handle(item.Args);
                }
                ModuleProvider.UnitOfWork.Commit();
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
