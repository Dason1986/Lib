using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.ComponentPatterns;

namespace Library.Win.MVP
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPresenter : IDisplay, IViewAware, IActivate, IDeactivate, IGuardClose
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class Presenter : IPresenter
    {
        public string DisplayName { get; protected set; }

        public void AttachView(object view, object context = null)
        {
            throw new NotImplementedException();
        }

        public object GetView(object context = null)
        {
            throw new NotImplementedException();
        }


        public virtual object View { get; protected set; }

        public virtual bool IsActive { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ViewAttachedEventArgs> ViewAttached;
        
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ActivationEventArgs> Activated;

    
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivated;

        public void Activate()
        {
            OnActivate();
        }

        public void Deactivate(bool close)
        {
            OnDeactivate();
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnActivate();
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnDeactivate();
        public void TryClose()
        {

        }

        public void CanClose(Action<bool> callback)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasclose"></param>
        protected virtual void OnDeactivated(bool wasclose)
        {
            var handler = Deactivated;
            if (handler != null) handler(this, new DeactivationEventArgs(wasclose));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasInitialized"></param>
        protected virtual void OnActivated(bool wasInitialized)
        {
            var handler = Activated;
            if (handler != null) handler(this, new ActivationEventArgs(wasInitialized));
        }
    }
}
