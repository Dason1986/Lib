using System;

namespace Library.Draw.Print
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class PrintActionAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public Type ClassType { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="classType"></param>
        public PrintActionAttribute(Type classType)
        {
            ClassType = classType;
        }
    }
}