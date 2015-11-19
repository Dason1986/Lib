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
    public class OrderCollection : Collection<QueryOrder>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        public OrderCollection(IEnumerable<QueryOrder> orders)
        {
            ReSet(orders);
        }
        /// <summary>
        /// 
        /// </summary>
        public OrderCollection()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        public void ReSet(IEnumerable<QueryOrder> orders)
        {
            this.Clear();
            if (orders == null) return;
            foreach (var order in orders)
            {
                this.Add(order);
            }
        }
    }
}