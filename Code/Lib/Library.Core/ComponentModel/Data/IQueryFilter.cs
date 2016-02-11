using System;

namespace Library.Data
{
    /// <summary>
    ///
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        ///
        /// </summary>
        Desc,

        /// <summary>
        ///
        /// </summary>
        Asc,
    }

    /// <summary>
    /// 条件过滤器接口（用于生成查询条件过滤的抽象）
    /// </summary>
    public interface IQueryFilter
    {
        /// <summary>
        ///
        /// </summary>
        string Filed { get; set; }

        /// <summary>
        ///
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        ///
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///
        /// </summary>
        Condition Condition { get; set; }

        /// <summary>
        ///
        /// </summary>
        Relation Relation { get; set; }

        /// <summary>
        ///
        /// </summary>
        object Value2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        QueryFiledType FiledType { get; set; }

        /// <summary>
        ///
        /// </summary>
        string FunctionName { get; set; }
    }

#if !SILVERLIGHT

    /// <summary>
    /// 查询过滤条件
    /// </summary>
    [Serializable]
#endif
    public enum QueryFiledType
    {
        /// <summary>
        ///
        /// </summary>
        Filter,

        /// <summary>
        ///
        /// </summary>
        Functon,
    }

    /// <summary>
    ///
    /// </summary>
    public class QueryFilter : IQueryFilter
    {
        private Condition _condition = Condition.Equal;
        private readonly FilterCollection _filters = new FilterCollection();

        /// <summary>
        ///
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Filed { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object Value2 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Condition Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Relation Relation { get; set; }

        /// <summary>
        ///
        /// </summary>
        public QueryFiledType FiledType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public FilterCollection Filters
        {
            get { return _filters; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public QueryFilter Clone()
        {
            return (QueryFilter)this.MemberwiseClone();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static QueryFilter Begin()
        {
            return new QueryFilter { Relation = Relation.Begin };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static QueryFilter End()
        {
            return new QueryFilter { Relation = Relation.End };
        }
    }

#if !SILVERLIGHT

    /// <summary>
    /// 关系
    /// </summary>
    [Serializable]
#endif
    [Flags]
    public enum Relation
    {
        /// <summary>
        ///
        /// </summary>

        And = 0,
        /// <summary>
        ///
        /// </summary>

        Or = 1,
        /// <summary>
        ///
        /// </summary>

        Begin = 2,
        /// <summary>
        ///
        /// </summary>

        End = 4,
    }

#if !SILVERLIGHT

    /// <summary>
    ///  条件
    /// </summary>
    [Serializable]
#endif
    [Flags]
    public enum Condition
    {
        /// <summary>
        /// 等于
        /// </summary>

        Equal = 1,

        /// <summary>
        /// 不等于
        /// </summary>

        NotEqual = 2,

        /// <summary>
        /// 像
        /// </summary>

        Like = 4,

        /// <summary>
        /// 像
        /// </summary>

        LikeStart = 8,

        /// <summary>
        /// 像
        /// </summary>

        LikeEnd = 16,

        /// <summary>
        /// 不像
        /// </summary>

        NotLike = 32,
        /// <summary>
        /// 不像
        /// </summary>

        NotLikeStart = 64,
        /// <summary>
        /// 不像
        /// </summary>

        NotLikeEnd = 128,

        /// <summary>
        /// 之间
        /// </summary>

        Between = 256,

        /// <summary>
        /// 小于
        /// </summary>

        LessThan = 512,

        /// <summary>
        /// 小于等于
        /// </summary>

        LessThanOrEqual = 1024,

        /// <summary>
        /// 大于
        /// </summary>

        GreaterThan = 2048,

        /// <summary>
        /// 大于等于
        /// </summary>

        GreaterThanOrEqual = 4096,

        /// <summary>
        /// 包含
        /// </summary>

        In = 8192,

        /// <summary>
        /// 不包含
        /// </summary>

        NotIn = 16384,

        /// <summary>
        /// 为空
        /// </summary>

        IsEmpty = 32768,
    }
}