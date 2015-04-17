using System;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace Library.Att
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LanguageDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceName;
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
                    this.DescriptionValue = LanguageResourceManagement.GetString(base.Description, _resourceName ?? "Global");
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
        /// <param name="resourceName"></param>
        public LanguageDescriptionAttribute(string description, string resourceName)
            : base(description)
        {
            _resourceName = resourceName;
        }
    }
}
