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
        public OrderCollection(IEnumerable<QueryOrder> orders)
        {
            ReSet(orders);
        }
        public OrderCollection()
        {

        }

        public void ReSet(IEnumerable<QueryOrder> orders)
        {
            this.Clear();
            if (orders != null)
                foreach (var order in orders)
                {
                    this.Add(order);
                }
        }
    }
}