using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public class RevertibleChangeModel : PropertyChangeModel, IRevertibleChangeTracking
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propetyname"></param>
        /// <param name="value"></param>
        protected internal void OnSaveBaseValue(string propetyname, object value)
        {
            this.OnPropertyChanged(propetyname);
            if (_isrejecting) return;
            if (_cacheValue == null) _cacheValue = new Dictionary<string, object>();
            if (_cacheValue.ContainsKey(propetyname)) return;
            _cacheValue.Add(propetyname, value);

        }

        private bool _isrejecting;
        IDictionary<string, object> _cacheValue;
        /// <summary>
        /// 
        /// </summary>
        public void RejectChanges()
        {
            if (_cacheValue == null) return;
            _isrejecting = true;
            Type type = this.GetType();

            foreach (var changeItem in _cacheValue)
            {
                var propertyinfo = type.GetProperty(changeItem.Key);
                if (propertyinfo == null) continue;
                propertyinfo.SetValue(this, changeItem.Value, null);
            }
            _isrejecting = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void AcceptChanges()
        {
            _cacheValue = null;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get { return _cacheValue == null; }
        }
    }
}