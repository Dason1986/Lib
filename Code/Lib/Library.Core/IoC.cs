using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public class IoC
    {
        readonly AssemblyManager _assemblyManager = new AssemblyManager();
        /// <summary>
        /// 
        /// </summary>
        public AssemblyManager AssemblyManager { get { return _assemblyManager; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fulltypename"></param>
        /// <param name="assemblyname"></param>
        /// <returns></returns>
        public object Instance(string fulltypename, string assemblyname)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fulltypename"></param>
        /// <returns></returns>
        public object Instance(string fulltypename)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fulltypename"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public object Instance(string fulltypename, Assembly assembly)
        {
            throw new NotSupportedException();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public Type GeType(string fullname, Assembly assembly = null)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public void AddAssembly(Assembly assembly)
        {

        }
    }
}
