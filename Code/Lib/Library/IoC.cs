using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Library.Annotations;
using Library.HelperUtility;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IoC
    {
        static IoC()
        {
            Current = new IoC(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitorAssembly"></param>
        public IoC(bool monitorAssembly)
        {
            if (monitorAssembly)
            {
                AppDomain.CurrentDomain.AssemblyLoad += (x, y) =>
                {
                    if (y.LoadedAssembly != null) _assemblyManager.AddAssembly(y.LoadedAssembly);
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static IoC Current { get; private set; }

        readonly AssemblyManager _assemblyManager = new AssemblyManager();
        /// <summary>
        /// 
        /// </summary>
        public AssemblyManager AssemblyManager { get { return _assemblyManager; } }
        readonly Dictionary<string, object> _objdDictionary = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlpath"></param>
        public void LoadXmlFile(string xmlpath)
        {
            var stream = File.OpenRead(xmlpath);
            LoadXmlFile(stream);
        } /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void LoadXmlFile(Stream xml)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xml);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void LoadXml(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <exception cref="Exception"></exception>
        public void SetObject([NotNull] string name, [NotNull] object obj)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (obj == null) throw new ArgumentNullException("obj");
            if (_objdDictionary.ContainsKey(name)) throw new Exception();
            _objdDictionary.Add(name, obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetObject(string name)
        {
            if (_objdDictionary.ContainsKey(name)) return _objdDictionary[name];
            throw new Exception();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetObjectByNew(string name)
        {
            if (!_objdDictionary.ContainsKey(name)) throw new Exception();
            var obj = _objdDictionary[name];
            return obj.GetType().CreateInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iocname"></param>
        /// <param name="fulltypename"></param>
        /// <param name="assemblyname"></param>
        /// <returns></returns>
        public object Instance(string iocname, string fulltypename, string assemblyname)
        {
            if (iocname == null) throw new ArgumentNullException("iocname");
            if (_objdDictionary.ContainsKey(iocname))
            {
                return GetObject(iocname);
            }
            var assname = _assemblyManager.GetAssembly(assemblyname);
            if (assname == null) throw new FileNotFoundException("找不對應該的Dll");
            return Instance(iocname, fulltypename, assname);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iocname"></param>
        /// <param name="fulltypename"></param>
        /// <returns></returns>
        public object Instance(string iocname, [NotNull] string fulltypename)
        {
            if (iocname == null) throw new ArgumentNullException("iocname");
            if (_objdDictionary.ContainsKey(iocname))
            {
                return GetObject(iocname);
            }
            if (fulltypename == null) throw new ArgumentNullException("fulltypename");
            var type = _assemblyManager.GeType(fulltypename);
            return type.CreateInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iocname"></param>
        /// <param name="fulltypename"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public object Instance([NotNull] string iocname, [NotNull] string fulltypename, [NotNull] Assembly assembly)
        {
            if (iocname == null) throw new ArgumentNullException("iocname");
            if (_objdDictionary.ContainsKey(iocname))
            {
                return GetObject(iocname);
            }
            if (fulltypename == null) throw new ArgumentNullException("fulltypename");
            if (assembly == null) throw new ArgumentNullException("assembly");
            var type = _assemblyManager.GeType(fulltypename, assembly);
            if (type == null) throw new TypeAccessException();
            return type.CreateInstance();
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
            Assembly tmp = assembly;
            if (tmp != null) return assembly.GetType(fullname);
            return assemblies.Values.Select(value => value.GetType(fullname)).FirstOrDefault(type => type != null);
        }

        readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyname"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Assembly GetAssembly(string assemblyname)
        {
            if (assemblies.ContainsKey(assemblyname)) return assemblies[assemblyname];
            var name = assemblies.Keys.FirstOrDefault(n => string.Equals(n, assemblyname, StringComparison.OrdinalIgnoreCase));
            return name == null ? null : assemblies[name];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public void AddAssembly(Assembly assembly)
        {
            assemblies.Add(assembly.GetName().Name, assembly);
        }
    }
}
