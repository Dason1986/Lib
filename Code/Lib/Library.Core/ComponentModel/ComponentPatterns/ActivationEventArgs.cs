using System;

namespace Library.ComponentModel.ComponentPatterns
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
}