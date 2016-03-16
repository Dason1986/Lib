using Library.ComponentModel;
using Library.ComponentModel.ComponentPatterns;
using System;

namespace Library.Win.MVP
{
    /// <summary>
    /// </summary>
    public interface IPresenter : IDisplay, IViewAware, IActivate, IDeactivate, IGuardClose
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public interface IPresenter<TView> : IPresenter
       where TView : IView 
    {
        

        /// <summary>
        ///
        /// </summary>
        TView GetView();
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class Presenter : IPresenter
    {
        /// <summary>
        ///
        /// </summary>
        public event EventHandler<ActivationEventArgs> Activated;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivated;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<ViewAttachedEventArgs> ViewAttached;

        /// <summary>
        ///
        /// </summary>
        public string DisplayName { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsActive { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public virtual object View { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public void Activate()
        {
            OnActivate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="context"></param>
        public void AttachView(object view, object context = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="callback"></param>
        public void CanClose(Action<bool> callback)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="close"></param>
        public void Deactivate(bool close)
        {
            OnDeactivate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object GetView(object context = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public void TryClose()
        {
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnActivate();

        /// <summary>
        ///
        /// </summary>
        /// <param name="wasInitialized"></param>
        protected virtual void OnActivated(bool wasInitialized)
        {
            var handler = Activated;
            if (handler != null) handler(this, new ActivationEventArgs(wasInitialized));
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnDeactivate();

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
        /// <param name="e"></param>
        protected virtual void OnViewAttached(ViewAttachedEventArgs e)
        {
            var handler = ViewAttached;
            if (handler != null) handler(this, e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public abstract class Presenter<TView> : Presenter, IPresenter<TView> where TView : IView 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  virtual TView GetView()
        {
            throw new Exception();
        }
    }
}