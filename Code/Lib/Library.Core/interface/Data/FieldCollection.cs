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
        public FieldCollection()
        {

        }
        public FieldCollection(IEnumerable<QueryField> fields)
        {
            ReSet(fields);
        }
        public void ReSet(IEnumerable<QueryField> fields)
        {
            this.Clear();
            if (fields != null)
                foreach (var field in fields)
                {
                    this.Add(field);
                }
        }
    }
}