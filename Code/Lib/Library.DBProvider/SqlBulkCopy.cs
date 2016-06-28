using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Library.DBProvider
{
    /// <summary>
    ///
    /// </summary>
    public class ListBulkInsertProvider<T> where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        public ListBulkInsertProvider(string tablename)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapping"></param>
        public void Add(SqlBulkCopyColumnMapping mapping)
        {
        }

        //private System.ComponentModel.DataAnnotations.Schema.TableAttribute tableAttribute;

        ///// <summary>
        /////
        ///// </summary>
        //public ListBulkInsertProvider(string connection)
        //{
        //    tableAttribute = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
        //    if (tableAttribute == null) throw new Exception("無定義表名");
        //}
    }

    /// <summary>
    ///
    /// </summary>
    public class DBBulkCopy
    {
        /// <summary>
        ///
        /// </summary>
        public int BatchSize { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///
        /// </summary>
        public void Insert(DataTable dataTable)
        {
            // TableAttribute
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.UseInternalTransaction, connection.BeginTransaction());
            sqlBulkCopy.BatchSize = BatchSize;
            sqlBulkCopy.DestinationTableName = TableName;
            sqlBulkCopy.WriteToServer(dataTable);
        }
    }
}