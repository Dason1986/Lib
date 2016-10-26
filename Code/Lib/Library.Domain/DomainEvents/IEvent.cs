using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Data;

namespace Library.Domain.DomainEvents
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainService : IDisposable
    {
        IModuleProvider ModuleProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void Handle(IDomainEventArgs args);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainService<TEvent> where TEvent : IDomainEventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void Handle(TEvent args);

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        IDomainEventArgs Args { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDomainService CreateService();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IDomainEventHandler<TService> : IDomainEventHandler where TService : IDomainService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new TService CreateService();
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class DomainEventHandler : IDomainEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
     //   protected internal DomainEventBus Bus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public DomainEventHandler(IDomainEventArgs args)
        {
            Args = args;
        }
        /// <summary>
        /// 
        /// </summary>
        public IDomainEventArgs Args
        {
            get; protected set;
        }
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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class DomainEventHandler<TService> : DomainEventHandler, IDomainEventHandler<TService> where TService : IDomainService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public DomainEventHandler(IDomainEventArgs args) : base(args)
        {

        }


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
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventArgs
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
        /// <param name="eventHandler"></param>
        void AddEvent(IDomainEventHandler eventHandler);
        /// <summary>
        /// 
        /// </summary>
        void Publish();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="args"></param>
        void Publish<TService>(IDomainEventArgs args)
            where TService : IDomainService;
    }



}
