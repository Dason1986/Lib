using System;

namespace Library.ComponentPatterns
{
    /// <summary>
    /// Contains details about the success or failure of an item's activation through an 
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
}