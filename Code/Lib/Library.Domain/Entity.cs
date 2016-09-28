using System;
using Library.ComponentModel.Model;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IEntity
    {
        [Required]
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public StatusCode StatusCode { get; set; }


    }
}