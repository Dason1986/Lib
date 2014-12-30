using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// 
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Value { get; set; }
    }
}
