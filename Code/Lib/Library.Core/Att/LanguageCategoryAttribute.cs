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
        /// <param name="resourceName"></param>
        public LanguageCategoryAttribute(string description, string resourceName)
            : base(description)
        {
            _resourceName = resourceName;
        }
        private readonly string _resourceName;
        protected override string GetLocalizedString(string value)
        {
            return LanguageResourceManagement.GetString(value, _resourceName ?? "Global");
        }
    }
}