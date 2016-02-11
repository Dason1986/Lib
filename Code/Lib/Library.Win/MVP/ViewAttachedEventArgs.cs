using System;

namespace Library.Win.MVP
{
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
        public object View { get; protected set; }

        /// <summary>
        /// The context.
        /// </summary>
        public object Context { get; protected set; }
    }
}