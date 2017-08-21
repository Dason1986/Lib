using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Data;
using Library.Domain.Data.ModuleProviders;

namespace Library.Domain.DomainEvents
{
    /// <summary>
    ///
    /// </summary>
    public interface IDomainService : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        IModuleProvider ModuleProvider { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        void Handle(DomainEventArgs args);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventPublish
    {
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        void Publish(DomainEventArgs args);
    }
    /// <summary>
    ///
    /// </summary>
    public interface IDomainService<in TEvent> : IDomainService where TEvent : DomainEventArgs
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
        /// <returns></returns>
        IDomainService CreateService();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IDomainEventHandler<out TService> : IDomainEventHandler where TService : IDomainService
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
    public abstract class DomainEventArgs
    {
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class DomainEmtpyArgs : DomainEventArgs
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
        void Publish(DomainEventArgs args);
    }
}