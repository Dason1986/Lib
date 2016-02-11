using System;

namespace Library.Data
{
    /// <summary>
    ///
    /// </summary>
    public interface IQueryOrder
    {
        /// <summary>
        ///
        /// </summary>
        string Filed { get; set; }

        /// <summary>
        ///
        /// </summary>
        OrderType Order { get; set; }
    }

#if !SILVERLIGHT

    /// <summary>
    /// 排序
    /// </summary>
    [Serializable]
#endif
    public struct QueryOrder : IQueryOrder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="order"></param>
        public QueryOrder(string filed, OrderType order)
            : this()
        {
            Filed = filed;
            Order = order;
        }

        /// <summary>
        ///
        /// </summary>
        public string Filed { get; set; }

        /// <summary>
        ///
        /// </summary>
        public OrderType Order { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filed"></param>
        /// <returns></returns>
        public static QueryOrder Desc(string filed)
        {
            return new QueryOrder(filed, OrderType.Desc);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filed"></param>
        /// <returns></returns>
        public static QueryOrder Asc(string filed)
        {
            return new QueryOrder(filed, OrderType.Asc);
        }
    }
}