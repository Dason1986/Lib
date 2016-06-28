using System;
using Library.ComponentModel.Model;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public StatusCode StatusCode { get; set; }


    }
}