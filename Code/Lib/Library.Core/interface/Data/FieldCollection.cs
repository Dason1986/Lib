using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Library.Data
{
    /// <summary>
    ///  
    /// </summary>
#if !SILVERLIGHT
    [ListBindable(false)]
#endif
    public class FieldCollection : Collection<QueryField>
    {
        /// <summary>
        /// 
        /// </summary>
        public FieldCollection()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        public FieldCollection(IEnumerable<QueryField> fields)
        {
            ReSet(fields);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        public void ReSet(IEnumerable<QueryField> fields)
        {
            this.Clear();
            if (fields == null) return;
            foreach (var field in fields)
            {
                this.Add(field);
            }
        }
    }
}