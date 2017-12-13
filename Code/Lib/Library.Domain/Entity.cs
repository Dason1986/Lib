using System;
using Library.ComponentModel.Model;
using System.ComponentModel.DataAnnotations;
using Library.Domain.DomainEvents;
using Library.ComponentModel.ComponentPatterns;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

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
            entity.Created = DateTime.Now;
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
    public abstract class IDEntity : PropertyChangeModel, IAggregateRoot<int>
    {
        /// <summary>
        /// 
        /// </summary>      
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 創建日期
        /// </summary>
        [Required]
        [DisplayName("創建日期"), Description("創建日期")]
        DateTime Created { get; }
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
        [DisplayName("數據狀態"), Description("數據狀態")]
        public StatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DisplayName("創建日期"), Description("創建日期")]
        public DateTime Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("創建者"), Description("創建者")]
        public string CreatedBy { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class AuditedEntity : Entity, IAuditedEntity, IEntityDeleted
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
        [DisplayName("修改日期"), Description("修改日期")]
        public DateTime Modified { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("修改者"), Description("修改者")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manaegment"></param>
        public void Delete(ICreatedInfo manaegment)
        {
            Modified = DateTime.Now;
            StatusCode = StatusCode.Delete;
            ModifiedBy = manaegment.CreatedBy;
        }
    }

}