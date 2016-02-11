using System;
using System.ComponentModel;

namespace Library.Att
{
    /// <summary>
    ///
    /// </summary>
    public class LanguageCategoryAttribute : CategoryAttribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        public LanguageCategoryAttribute(string description)
            : base(description)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        /// <param name="resourceType"></param>
        public LanguageCategoryAttribute(string description, Type resourceType)
            : base(description)
        {
            _resourceType = resourceType;
        }

        private readonly Type _resourceType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetLocalizedString(string value)
        {
            return ResourceManagement.GetString(_resourceType, value);
        }
    }
}