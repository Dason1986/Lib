using Library.ComponentModel.Model;
using Library.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAggregateRoot 
    {
        /// <summary>
        /// 
        /// </summary>
        Guid ID { get; }
  
        /// <summary>
        /// 
        /// </summary>
        void Commit();

        /// <summary>
        /// 
        /// </summary>
        void Publish();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        void AddEvent(IDomainEventHandler eventHandler);
    }

    
}
