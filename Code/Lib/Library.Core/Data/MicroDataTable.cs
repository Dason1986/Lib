using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class MicroDataTable
    {
        /// <summary>
        /// 
        /// </summary>
        public MicroDataTable()
        {
            Rows = new List<MicroDataRow>();

            Columns = new List<MicroDataColumn>();
        }
        /// <summary>
        /// 整个查询语句结果的总条数，而非本DataTable的条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MicroDataColumn> Columns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MicroDataRow> Rows { get; set; }
      
        /// <summary>
        /// 
        /// </summary>
        public MicroDataColumn[] PrimaryKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MicroDataRow NewRow()
        {
            return new MicroDataRow(this.Columns, new object[Columns.Count]);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MicroDataColumn
    {
        /// <summary>
        /// 
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Type ColumnType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MicroDataRow
    {
        private object[] _ItemArray;
        /// <summary>
        /// 
        /// </summary>
        public List<MicroDataColumn> Columns { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="itemArray"></param>
        public MicroDataRow(List<MicroDataColumn> columns, object[] itemArray)
        {
            this.Columns = columns;
            this._ItemArray = itemArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get { return _ItemArray[index]; }
            set { _ItemArray[index] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object this[string columnName]
        {
            get
            {
                int i = 0;
                foreach (MicroDataColumn column in Columns)
                {
                    if (column.ColumnName == columnName)
                        break;
                    i++;
                }
                return _ItemArray[i];
            }
            set
            {
                int i = 0;
                foreach (MicroDataColumn column in Columns)
                {
                    if (column.ColumnName == columnName)
                        break;
                    i++;
                }
                _ItemArray[i] = value;
            }
        }
    }
}
