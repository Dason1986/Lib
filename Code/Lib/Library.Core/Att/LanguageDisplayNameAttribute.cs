using System;
using System.ComponentModel;
using System.Runtime;

namespace Library.Att
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LanguageDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly Type ResourceType;
        private bool replaced;

        /// <summary>
        ///
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    if (ResourceType != null)
                        this.DisplayNameValue = ResourceManagement.GetString(ResourceType, base.DisplayName);
                }
                return base.DisplayName;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="displayName"></param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public LanguageDisplayNameAttribute(string displayName)
            : base(displayName)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        /// <param name="resourceType"></param>
        public LanguageDisplayNameAttribute(string description, Type resourceType)
            : base(description)
        {
            ResourceType = resourceType;
        }
    }
}