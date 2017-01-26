using System;
using System.Configuration;
using System.Configuration.Provider;
using System.IO;
using System.Xml;

namespace Library.ExceptionProviders
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CustomExceptionProvider : ProviderBase
    {
        /// <summary>
        ///
        /// </summary>
        public static CustomExceptionCollection Providers
        {
            get
            {
                Initialize();
                return _provider;
            }
        }

        private static CustomExceptionCollection _provider;

        /// <summary>
        ///
        /// </summary>
        public const string DefaultSection = "customExceptions";

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract Exception ProvideFault(Exception error, ref string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public abstract bool HandleError(Exception error);
    }

    /// <summary>
    ///
    /// </summary>
    public class CustomExceptionSection : ConfigurationSection
    {
        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("exceptions")]
        public CustomExceptionCollection CustomExceptions
        {
            get { return (CustomExceptionCollection)this["exceptions"]; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("file", DefaultValue = "customExceptions.config")]
        public string File
        {
            get { return (string)this["file"]; }
            set { this["file"] = value; }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationCollection(typeof(CustomExceptionElement))]
    public class CustomExceptionCollection : ConfigurationElementCollection
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustomExceptionElement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomExceptionElement)element).Name;
        }

        internal void Add(CustomExceptionElement element)
        {
            BaseAdd(element);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CustomExceptionElement : ConfigurationElement
    {
        private CustomExceptionProvider _provider;

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [ConfigurationProperty("type", DefaultValue = "", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
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