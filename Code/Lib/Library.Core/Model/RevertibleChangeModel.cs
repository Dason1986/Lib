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

        public void AcceptChanges()
        {
            _cacheValue = null;
        }

        public bool IsChanged
        {
            get { return _cacheValue == null; }
        }
    }
}