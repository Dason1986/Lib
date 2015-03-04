using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EditableModel : PropertyChangeModel, IEditableObject
    {
        private bool _canEdit = false;
        IDictionary<string, object> _cacheValue;
        private bool _isrejecting;

        /// <summary>
        /// 
        /// </summary>
        protected void CanEdit()
        {
            if (!_canEdit) throw new EditableObjectException("沒啟動修改模式");
        }

        public void BeginEdit()
        {
            _canEdit = true;
            _cacheValue = new Dictionary<string, object>();
        }

        public void CancelEdit()
        {
            if (_cacheValue == null || !_canEdit) return;
            _isrejecting = true;
            Type type = this.GetType();

            foreach (var changeItem in _cacheValue)
            {
                var propertyinfo = type.GetProperty(changeItem.Key);
                if (propertyinfo == null || !propertyinfo.CanWrite) continue;
                propertyinfo.SetValue(this, changeItem.Value, null);
            }
            _isrejecting = false;
            _canEdit = false;
        }

        public void EndEdit()
        {
            _canEdit = false;
            _cacheValue.Clear();
            _cacheValue = null;
        }

        protected internal void OnSaveBaseValue(string propertyName, string oldValue)
        {
            if (_isrejecting) return;
            if (_canEdit == false) throw new EditableObjectException("沒啟動修改模式");
            if (_cacheValue.ContainsKey(propertyName)) return;
            _cacheValue.Add(propertyName, oldValue);
        }
    }
}