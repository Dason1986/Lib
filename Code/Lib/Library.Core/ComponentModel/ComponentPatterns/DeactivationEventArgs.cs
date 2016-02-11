using System;

namespace Library.ComponentModel.ComponentPatterns
{
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
}