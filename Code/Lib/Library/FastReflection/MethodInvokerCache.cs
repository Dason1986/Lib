using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public class MethodInvokerCache : FastReflectionCache<MethodInfo, IMethodInvoker>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override IMethodInvoker Create(MethodInfo key)
        {
            return FastReflectionFactories.MethodInvokerFactory.Create(key);
        }
    }
}
