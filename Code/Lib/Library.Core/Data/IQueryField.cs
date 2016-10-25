using System;
using System.Collections.Generic;

namespace Library.Data
{
    /// <summary>
    ///
    /// </summary>
    public interface IQueryField
    {
        /// <summary>
        ///
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool IsSelected { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class QueryField : IQueryField
    {
        /// <summary>
        ///
        /// </summary>
        public QueryField()
        {
            IsSelected = true;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool Same
        {
            get
            {
                return string.Equals(Name, DisplayName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(DisplayName);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static IEqualityComparer<QueryField> Comparer = new ComparerName();

        private class ComparerName : IEqualityComparer<QueryField>
        {
            public bool Equals(QueryField x, QueryField y)
            {
                if (x != null && y != null && string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase))
                    return true;
                return false;
            }

            public int GetHashCode(QueryField obj)
            {
                if (obj == null) return -1;
                return obj.GetHashCode();
            }
        }
    }
}