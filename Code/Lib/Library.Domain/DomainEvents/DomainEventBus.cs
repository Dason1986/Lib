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
        /// <typeparam name="TService"></typeparam>
        /// <param name="args"></param>
        public void Publish<TService>(IDomainEventArgs args)

            where TService : IDomainService
        {
            Thread thread = MyMethod<TService, IDomainEventArgs>(args);
            thread.Start();

        }
        /// <summary>
        /// 
        /// </summary>
        public virtual Library.Domain.Data.IModuleProvider ModuleProvider { get; set; }
        private Thread MyMethod<TService, TEvent>(TEvent args) where TService : IDomainService where TEvent : IDomainEventArgs
        {
            if (ModuleProvider == null) throw new Exception("ModuleProvider 爲空");
            Thread thread = new Thread(n =>
            {
                var service = Bootstrap.Currnet.GetService<TService>();
                service.ModuleProvider = ModuleProvider;
                service.Handle(args);
                ModuleProvider.UnitOfWork.Commit();
            });

            return thread;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="args"></param>
        public void PublishAwait<TService>(IDomainEventArgs args)

            where TService : IDomainService
        {
            Thread thread = MyMethod<TService, IDomainEventArgs>(args);
            thread.Start();
            thread.Join();
        }
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
