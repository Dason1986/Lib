using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Win.MVP
{
    /// <summary>
    /// EventArgs sent during activation.
    /// </summary>
    public class ActivationEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasInitialized"></param>
        public ActivationEventArgs(bool wasInitialized)
        {
            this.WasInitialized = wasInitialized;
        }

        /// <summary>
        /// Indicates whether the sender was initialized in addition to being activated.
        /// </summary>
        public bool WasInitialized { get; protected set; }
    }
    /// <summary>
    /// EventArgs sent during deactivation.
    /// </summary>
    public class DeactivationEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasClosed"></param>
        public DeactivationEventArgs(bool wasClosed)
        {
            this.WasClosed = wasClosed;
        }

        /// <summary>
        /// Indicates whether the sender was closed in addition to being deactivated.
        /// </summary>
        public bool WasClosed { get; protected set; }
    }

    /// <summary>
    /// The event args for the <see cref="IViewAware.ViewAttached"/> event.
    /// </summary>
    public class ViewAttachedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="context"></param>
        public ViewAttachedEventArgs(object view, object context)
        {
            this.View = view;
            this.Context = context;
        }

        /// <summary>
        /// The view.
        /// </summary>
        public object View { get;protected set; }

        /// <summary>
        /// The context.
        /// </summary>
        public object Context { get; protected set; }
    }
    /// <summary>
    /// Contains details about the success or failure of an item's activation through an <see cref="IConductor"/>.
    /// </summary>
    public class ActivationProcessedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="success"></param>
        public ActivationProcessedEventArgs(object item, bool success)
        {
            this.Item = item;
            this.Success = success;
        }

        /// <summary>
        /// The item whose activation was processed.
        /// </summary>
        public object Item { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activation was a success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; protected set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// 顯示名稱
        /// </summary>
        string DisplayName { get; }
    }
    /// <summary>
    /// Denotes an object that can be closed.
    /// </summary>
    public interface IClose
    {
        /// <summary>
        /// Tries to close this instance.
        /// </summary>
        void TryClose();
    }

    /// <summary>
    /// Denotes an instance which may prevent closing.
    /// </summary>
    public interface IGuardClose : IClose
    {
        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementer calls this action with the result of the close check.</param>
        void CanClose(Action<bool> callback);
    }
    /// <summary>
    /// Denotes an instance which requires activation.
    /// </summary>
    public interface IActivate
    {
        ///<summary>
        /// Indicates whether or not this instance is active.
        ///</summary>
        bool IsActive { get; }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        void Activate();

        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        event EventHandler<ActivationEventArgs> Activated;
    }
    /// <summary>
    /// Denotes an instance which requires deactivation.
    /// </summary>
    public interface IDeactivate
    {
        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        /// <param name="close">Indicates whether or not this instance is being closed.</param>
        void Deactivate(bool close);

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        event EventHandler<DeactivationEventArgs> Deactivated;
    }
    /// <summary>
    /// Denotes a class which is aware of its view(s).
    /// </summary>
    public interface IViewAware
    {
        /// <summary>
        /// Attaches a view to this instance.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context in which the view appears.</param>
        void AttachView(object view, object context = null);

        /// <summary>
        /// Gets a view previously attached to this instance.
        /// </summary>
        /// <param name="context">The context denoting which view to retrieve.</param>
        /// <returns>The view.</returns>
        object GetView(object context = null);

        /// <summary>
        /// Raised when a view is attached.
        /// </summary>
        event EventHandler<ViewAttachedEventArgs> ViewAttached;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        object View { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IPresenter : IDisplay, IViewAware, IActivate, IDeactivate, IGuardClose
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 激活
        /// </summary>
        void Activate();
        /// <summary>
        /// 禁用
        /// </summary>
        void Deactivate();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationFacade
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Notification(string message);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool MessageConfirm(string message);
        /// <summary>
        /// 
        /// </summary>
        void Message(string message);

        /// <summary>
        /// 
        /// </summary>
        void ShowLoading();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void SetLoadingText(string text);

        /// <summary>
        /// 
        /// </summary>
        void HideLoading();
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationFacade : IApplicationFacade
    {
        private ApplicationFacade()
        {

        }

        private static readonly ApplicationFacade _instance = new ApplicationFacade();
        /// <summary>
        /// 
        /// </summary>
        public static ApplicationFacade Instance { get { return _instance; } }

        public void Notification(string message)
        {
            throw new NotImplementedException();
        }

        public bool MessageConfirm(string message)
        {
            throw new NotImplementedException();
        }

        public void Message(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowLoading()
        {
            throw new NotImplementedException();
        }

        public void SetLoadingText(string text)
        {
            throw new NotImplementedException();
        }

        public void HideLoading()
        {
            throw new NotImplementedException();
        }
    }

}
