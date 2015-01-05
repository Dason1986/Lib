using System;
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
    /// <summary>
    /// 參數讀取供應器
    /// </summary>
    public interface IParameterProvider
    {

    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ParameterAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ParameterAttribute()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public Type Provider { get; set; }
    }
}
