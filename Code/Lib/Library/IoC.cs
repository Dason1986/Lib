using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Library.Annotations;
using Library.ComponentModel;
using Library.HelperUtility;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class IoCException : LibException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        /// <summary>
        /// 
        /// </summary>
        public IoCException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        protected IoCException(string message, double resultCode)
            : base(message, resultCode)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <param name="inner"></param>
        protected IoCException(string message, double resultCode, Exception inner)
            : base(message, resultCode, inner)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public IoCException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public IoCException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="formatages"></param>
        public IoCException(double resultCode, object[] formatages)
            : base(resultCode, formatages)
        {
            ResultCode = resultCode;
        }
     

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IoCException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

    }
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
        public IoC(bool monitorAssembly = false)
        {
            var useass = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in useass)
            {
                _assemblyManager.AddAssembly(assembly);
            }
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
        readonly Dictionary<string, TmpIoCItem> _objdDictionary = new Dictionary<string, TmpIoCItem>();

        class TmpIoCItem
        {
            public Type OjbType;
            public object objItem;

        }
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
            LoadDoc(document);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void LoadXml(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            LoadDoc(document);
        }

        private static readonly char[] spchar = { ',' };
        void LoadDoc(XmlDocument document)
        {
            var nodes = document.SelectNodes(@"//objects/object[@Disabled!='true']");
            if (!nodes.HasRecord()) return;
            foreach (XmlNode node in nodes)
            {
                //type,name
                var typeAtr = node.Attributes["type"];
                var nameAtr = node.Attributes["name"];
                if (string.IsNullOrEmpty(nameAtr.Value)) throw new IoCException();
                if (_objdDictionary.ContainsKey(nameAtr.Value)) throw new IoCException();
                if (string.IsNullOrEmpty(typeAtr.Value)) throw new IoCException();
                var typeVal = typeAtr.Value.Split(spchar, 2);
                var ass = _assemblyManager.GetAssembly(typeVal[1]);
                if (ass == null) throw new IoCException();
                Type objType = ass.GetType(typeVal[0]);
                if (objType == null) throw new IoCException();
                var obj = objType.CreateInstance();
                SetObject(nameAtr.Value, obj);
            }
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
            if (_objdDictionary.ContainsKey(name)) throw new IoCException();
            _objdDictionary.Add(name, new TmpIoCItem() { OjbType = obj.GetType(), objItem = obj });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetObject(string name)
        {
            if (_objdDictionary.ContainsKey(name)) return _objdDictionary[name];
            return _objdDictionary[name].objItem;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetObjectByNew(string name)
        {
            if (!_objdDictionary.ContainsKey(name)) throw new IoCException();
            return _objdDictionary[name].OjbType.CreateInstance();
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
            if (_objdDictionary.ContainsKey(iocname)) return GetObject(iocname);
            var assname = _assemblyManager.GetAssembly(assemblyname);
            if (assname == null) throw new IoCException("找不對應該的assemblyname");
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
            if (_objdDictionary.ContainsKey(iocname)) return GetObject(iocname);
            if (fulltypename == null) throw new IoCException("fulltypename");
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
            if (fulltypename == null) throw new ArgumentNullException("fulltypename");
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (_objdDictionary.ContainsKey(iocname))  return GetObject(iocname); 
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
