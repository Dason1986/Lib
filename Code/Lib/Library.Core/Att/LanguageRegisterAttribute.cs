using System;
using System.Resources;
using Library.Annotations;

namespace Library.Att
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class LanguageRegisterAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type ResourceType { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResourcePath { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string ResourceName { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourcePath"></param>
        /// <param name="resourceName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LanguageRegisterAttribute([NotNull] Type resourceType, [NotNull] string resourcePath, string resourceName)
        {
            ResourceType = resourceType;
            ResourcePath = resourcePath;
            if (resourceType == null) throw new ArgumentNullException("resourceType");
            ResourceName = resourceName ?? ResourceType.Name;

            var manager = new ResourceManager(resourcePath, resourceType.Assembly);
            LanguageResourceManagement.AddResource(ResourceName, manager);
        }
    }
}