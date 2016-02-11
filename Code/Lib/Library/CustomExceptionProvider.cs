using System;
using System.Configuration;
using System.Configuration.Provider;

namespace Library
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
                init();
                return _provider;
            }
        }

        private static CustomExceptionCollection _provider;

        private static void init()
        {
            if (_provider != null) return;

            CustomExceptionSection config = ConfigurationManager.GetSection("customExceptions") as CustomExceptionSection;

            if (config == null) return;

            _provider = config.CustomExceptions;
            foreach (CustomExceptionElement customException in config.CustomExceptions)
            {
                if (string.IsNullOrEmpty(customException.Type))
                {
                    throw new Exception();
                }
                Type type = Type.GetType(customException.Type);
                if (type == null || type.IsAssignableFrom(typeof(CustomExceptionProvider)))
                {
                    throw new Exception();
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
    }

    /// <summary>
    ///
    /// </summary>
    public class CustomExceptionElement : ConfigurationElement
    {
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
        public CustomExceptionProvider Provider { get; internal set; }
    }
}