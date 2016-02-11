namespace Library.Data
{
    /// <summary>
    ///
    /// </summary>
    public static class QueryHelper
    {
        /// <summary>
        /// 创建字段名大于值的过滤条件(例如 USR > '')
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddGreaterThanFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.GreaterThan);
        }

        /// <summary>
        /// 创建字段名包含的过滤条件（例如 USR IN ('','','')）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static IQueryFilter AddInFilter(this IQueryFilter filter, string filed, params object[] values)
        {
            return filter.TryAddFilter(filed, values, Condition.In);
        }

        /// <summary>
        /// 创建字段名大于等于的过滤条件（例如 USR >= ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddGreaterThanOrEqualFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.GreaterThanOrEqual);
        }

        /// <summary>
        /// 创建字段名小于的过滤条件（例如 USR &lt; ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddLessThanFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.LessThan);
        }

        /// <summary>
        /// 创建字段名小于等于的过滤条件（例如 USR &lt;= ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddLessThanOrEqualFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.LessThanOrEqual);
        }

        /// <summary>
        /// 创建字段名等于的过滤条件（例如 USR = ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddEqualFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value);
        }

        /// <summary>
        /// 创建字段名不等于的过滤条件（例如 USR &lt;> ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddNotEqualFilte(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.NotEqual);
        }

        /// <summary>
        /// 创建字段名模糊匹配的过滤条件（例如 USR like ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddLikeFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.Like);
        }

        /// <summary>
        /// 创建字段名区间匹配的过滤条件（例如 USR between '' and ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddBetweenFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.Between);
        }

        /// <summary>
        /// 创建字段名不包含的过滤条件（例如 USR NOT IN ('','','')）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static IQueryFilter AddNotInFilter(this IQueryFilter filter, string filed, params object[] values)
        {
            return filter.TryAddFilter(filed, values, Condition.NotIn);
        }

        /// <summary>
        /// 创建字段名非模糊匹配的过滤条件（例如 USR NOT LIKE ''）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IQueryFilter AddNotLikeFilter(this IQueryFilter filter, string filed, object value)
        {
            return filter.TryAddFilter(filed, value, Condition.NotLike);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filed">字段名</param>
        /// <param name="value">值</param>
        /// <param name="condition">逻辑操作符枚举（=、小于、>、LIKE.....）</param>
        /// <param name="relation">关系操作符枚举（AND、OR、左括号、右括号）</param>
        /// <returns></returns>
        public static IQueryFilter TryAddFilter(this IQueryFilter filter, string filed, object value, Condition condition = Condition.Equal, Relation relation = Relation.And)
        {
            if (filter != null)
            {
                filter.Condition = condition;
                filter.Value = value;
                filter.Filed = filed;
            }
            return filter;
        }
    }
}