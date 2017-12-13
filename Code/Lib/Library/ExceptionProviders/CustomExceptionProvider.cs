using System;
using System.Configuration;
using System.Configuration.Provider;
using System.IO;
using System.Linq;
using System.Xml;

namespace Library.ExceptionProviders
{
    public abstract class CustomExceptionProvider : ProviderBase
    {
        public static CustomExceptionCollection Providers
        {
            get
            {
                Initialize();
                return _provider;
            }
        }

        private static CustomExceptionCollection _provider;
        public const string DefaultSection = "customExceptions";
        public string GetLogerName(Exception exception)
        {

            string logname = string.Format("{0} => {1}", exception.TargetSite.DeclaringType.FullName, exception.TargetSite.Name);
            return logname;
        }
        public static void Add(CustomExceptionProvider provider)
        {
            if (_provider == null)
            {
                _provider = new CustomExceptionCollection();
            }
            _provider.Add(new CustomExceptionElement { Name = provider.Name, Type = provider.GetType().Name, Provider = provider });
        }
        private static void Initialize()
        {
            if (_provider != null) return;

            CustomExceptionSection config = ConfigurationManager.GetSection(DefaultSection) as CustomExceptionSection;

            if (config == null) return;
            if (!string.IsNullOrEmpty(config.File))
            {
                var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.File);
                if (File.Exists(filepath))
                {
                    ConfigXmlDocument xml = new ConfigXmlDocument();
                    xml.Load(filepath);
                    var adds = xml.GetElementsByTagName("add");
                    _provider = new CustomExceptionCollection();
                    foreach (XmlNode add in adds)
                    {
                        _provider.Add(new CustomExceptionElement() { Type = add.Attributes["type"].Value, Name = add.Attributes["name"].Value });
                    }
                }
            }
            if (_provider == null || _provider.Count == 0)
            {
                _provider = config.CustomExceptions;
            }

            foreach (CustomExceptionElement customException in _provider)
            {
                if (string.IsNullOrEmpty(customException.Type))
                {
                    throw new TypeAccessException("type is empty");
                }
                Type type = Type.GetType(customException.Type);
                if (type == null || type.IsAssignableFrom(typeof(CustomExceptionProvider)))
                {
                    throw new NotImplementedException(customException.Type);
                }
                customException.Provider = (CustomExceptionProvider)Activator.CreateInstance(type);
            }
        }

        public abstract Exception ProvideFault(Exception error, ref string message);

        public abstract bool HandleError(Exception error);
    }

    public class CustomExceptionSection : ConfigurationSection
    {
        [ConfigurationProperty("exceptions")]
        public CustomExceptionCollection CustomExceptions
        {
            get { return (CustomExceptionCollection)this["exceptions"]; }
        }

        [ConfigurationProperty("file", DefaultValue = "customExceptions.config")]
        public string File
        {
            get { return (string)this["file"]; }
            set { this["file"] = value; }
        }
    }

    [ConfigurationCollection(typeof(CustomExceptionElement))]
    public class CustomExceptionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustomExceptionElement();
        }

        public CustomExceptionElement this[int index]
        {
            get { return (CustomExceptionElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomExceptionElement)element).Name;
        }

        internal void Add(CustomExceptionElement element)
        {
            BaseAdd(element);
        }

        public bool HandleError(Exception error)
        {
            return this.OfType<CustomExceptionElement>().Select(n => n.Provider.HandleError(error)).Any(tmpfe => tmpfe == true);
        }

      
    }

    public class CustomExceptionElement : ConfigurationElement
    {
        private CustomExceptionProvider _provider;

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        public CustomExceptionProvider Provider
        {
            get
            {
                if (_provider == null && !string.IsNullOrEmpty(this.Type))
                {
                    Type type = System.Type.GetType(Type);
                    if (type == null || type.IsAssignableFrom(typeof(CustomExceptionProvider)))
                    {
                        throw new NotImplementedException(Type);
                    }
                    _provider = (CustomExceptionProvider)Activator.CreateInstance(type);
                }
                return _provider;
            }
            internal set { _provider = value; }
        }
    }
}