using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public class MethodInvokerFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IMethodInvoker Create(MethodInfo key)
        {
            return new MethodInvoker(key);
        }

        #region IFastReflectionFactory<MethodInfo,IMethodInvoker> Members

        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }

        #endregion
    }
}
