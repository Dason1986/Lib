using System;
using Library.ComponentModel.Model;
using System.ComponentModel.DataAnnotations;
using Library.Domain.DomainEvents;
using Library.ComponentModel.ComponentPatterns;

namespace Library.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="created"></param>
        public static void Create(this Entity entity, ICreatedInfo created)
        {
            entity.ID = Library.IdentityGenerator.NewGuid();
            entity.StatusCode = StatusCode.Enabled;
            entity.CreatedBy = created.CreatedBy;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="modifiedBy"></param>
        public static void SetChangedInfo(this AuditedEntity entity, string modifiedBy)
        {
            entity.ModifiedBy = modifiedBy;
            entity.Modified = DateTime.Now;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : PropertyChangeModel, IEntity, ICreatedInfo, IAggregateRoot<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createinfo"></param>
        public Entity(ICreatedInfo createinfo)
        {
            ID = IdentityGenerator.NewGuid();
            Created = DateTime.Now;
            CreatedBy = createinfo.CreatedBy;
            StatusCode = StatusCode.Enabled;
        }
        /// <summary>
        /// 
        /// </summary>
        public Entity()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return ID == Guid.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public StatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CreatedBy { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class AuditedEntity : Entity, IAuditedEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public AuditedEntity()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createinfo"></param>
        public AuditedEntity(ICreatedInfo createinfo) : base(createinfo)
        {
            Modified = DateTime.Now;
            ModifiedBy = createinfo.CreatedBy;


        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime Modified { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ModifiedBy { get; set; }







    }

}