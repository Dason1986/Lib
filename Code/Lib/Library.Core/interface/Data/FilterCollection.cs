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
    public class FilterCollection : Collection<QueryFilter>
    {
        public FilterCollection(IEnumerable<QueryFilter> filters)
        {
            ReSet(filters);

        }
        public FilterCollection()
        {

        }
        public void ReSet(IEnumerable<QueryFilter> filters)
        {
            this.Clear();
            if (filters != null)
                foreach (var filter in filters)
                {
                    this.Add(filter);
                }
        }
    }
}