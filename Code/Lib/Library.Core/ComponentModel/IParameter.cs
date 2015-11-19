using System;
using System.Collections.Generic;
using System.Data;
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
        /// 組
        /// </summary>
        string Group { get; }

        /// <summary>
        /// 說明
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 鍵
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 只讀
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// 值
        /// </summary>
        string Value { get; }
        /// <summary>
        /// 付值
        /// </summary>
        /// <param name="value"></param>
        void SetValue(string value);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ParameterItem : IParameter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="value"></param>
        public ParameterItem(string group, string key, string description, bool isReadOnly, string value)
        {
            Group = group;
            Key = key;
            Description = description;
            IsReadOnly = isReadOnly;
            Value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Group { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(string value)
        {
            if (IsReadOnly) throw new ReadOnlyException(Key);
            Value = value;
        }
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

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
