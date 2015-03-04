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
        private bool replaced;
        private readonly string _resourceName;

        public override string DisplayName
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    this.DisplayNameValue = LanguageResourceManagement.GetString(base.DisplayName, _resourceName ?? "Global");

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
        /// <param name="resourceName"></param>
        public LanguageDisplayNameAttribute(string description, string resourceName)
            : base(description)
        {
            _resourceName = resourceName;
        }
    }
}