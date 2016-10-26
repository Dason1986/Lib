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
    public abstract class Entity : PropertyChangeModel, IEntity
    {
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


    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class CreateEntity : Entity, ICreatedInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateEntity()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="create"></param>
        public CreateEntity(ICreatedInfo create)
        {
            ID = Library.IdentityGenerator.NewGuid();
            StatusCode = StatusCode.Enabled;
            SetCreateInfo(create.CreatedBy);
        }
        DateTime ICreatedInfo.Created { get { return Created; } }
        string ICreatedInfo.CreatedBy { get { return CreatedBy; } }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdBy"></param>
        protected virtual void SetCreateInfo(string createdBy)
        {
            Created = DateTime.Now;
            CreatedBy = createdBy;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class AuditedEntity : CreateEntity, IAuditedEntity
    {


        DateTime IModifiedInfo.Modified { get { return Modified; } }

        string IModifiedInfo.ModifiedBy { get { return ModifiedBy; } }

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

        /// <summary>
        /// 
        /// </summary>
        public AuditedEntity()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="create"></param>
        public AuditedEntity(ICreatedInfo create) : base(create)
        {


            SetChangedInfo(create.CreatedBy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiedBy"></param>
        public virtual void SetChangedInfo(string modifiedBy)
        {
            this.ModifiedBy = modifiedBy;
            this.Modified = DateTime.Now;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class AggregateRoot : IAggregateRoot, IActivate, IDeactivate
    {
        /// <summary>
        /// 
        /// </summary>
        public AggregateRoot()
        {
            Bus = new DomainEventBus();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid ID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        protected DomainEventBus Bus { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        protected bool IsActive { get; private set; }
        bool IActivate.IsActive
        {
            get
            {
                return IsActive;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivated;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ActivationEventArgs> Activated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnActivated(ActivationEventArgs args)
        {
            var handler = Activated;
            if (handler == null) return;
            Activated.Invoke(this, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public virtual void AddEvent(DomainEventHandler eventHandler)
        {
            Bus.AddEvent(eventHandler);
        }
        void IAggregateRoot.AddEvent(IDomainEventHandler eventHandler)
        {

            AddEvent(eventHandler as DomainEventHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract void Commit();

        /// <summary>
        /// 
        /// </summary>
        public void Publish()
        {
            Bus.Publish();
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnActivate()
        {

        }
        void IActivate.Activate()
        {
            OnActivate();
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDeactivate(bool close)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual Func<bool> CanClose { get { return _canClose; } }
        Func<bool> _canClose = () => { return true; };
        void IDeactivate.Deactivate(bool close)
        {
            OnDeactivate(close);
        }
    }
}