using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;

namespace Library.Att
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LanguageDescriptionAttribute : DescriptionAttribute
    {
        private bool replaced;

        public override string Description
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    //  this.DescriptionValue = SR.GetString(base.Description);
                }
                return base.Description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public LanguageDescriptionAttribute(string description)
            : base(description)
        {
        }
    }    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LanguageDisplayNameAttribute : DisplayNameAttribute
    {
        private bool replaced;

        public override string DisplayName
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    //  this.DisplayNameValue = SR.GetString(base.DisplayName);
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
    }
}
