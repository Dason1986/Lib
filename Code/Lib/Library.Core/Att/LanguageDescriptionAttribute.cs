using System;
using System.ComponentModel;

namespace Library.Att
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LanguageDescriptionAttribute : DescriptionAttribute
    {
        private readonly Type _resourceType;
        private bool replaced;

        /// <summary>
        ///
        /// </summary>
        public override string Description
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    this.DescriptionValue = ResourceManagement.GetString(_resourceType, base.Description);
                }
                return base.Description;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>

        public LanguageDescriptionAttribute(string description)
            : base(description)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        /// <param name="resourceType"></param>
        public LanguageDescriptionAttribute(string description, Type resourceType)
            : base(description)
        {
            _resourceType = resourceType;
        }
    }
}