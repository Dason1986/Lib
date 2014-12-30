using System;

namespace Library.Draw.Print
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class PrintActionAttribute : Attribute
    {
        public Type ClassType { get; protected set; }


        public PrintActionAttribute(Type classType)
        {
            ClassType = classType;
        }
    }
}