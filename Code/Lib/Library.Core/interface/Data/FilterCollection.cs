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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public FilterCollection(IEnumerable<QueryFilter> filters)
        {
            ReSet(filters);

        }
        /// <summary>
        /// 
        /// </summary>
        public FilterCollection()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public void ReSet(IEnumerable<QueryFilter> filters)
        {
            this.Clear();
            if (filters == null) return;
            foreach (var filter in filters)
            {
                this.Add(filter);
            }
        }
    }
}