using Library.Annotations;
using Library.HelperUtility;
using System;
using System.Data;
using System.Linq;

namespace Library.DBProvider
{
    /// <summary>
    ///
    /// </summary>
    public class SimpleDBExecute : IDBExecute
    {
        private readonly IDbConnection _connection;

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        public SimpleDBExecute([NotNull] IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        protected virtual IDbCommand CreateCommand(string commandText, params IDbDataParameter[] dataParameters)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = commandText;
            if (!dataParameters.HasRecord()) return cmd;
            foreach (IDbDataParameter parameter in dataParameters.Where(parameter => parameter.Value == null))
            {
                parameter.Value = DBNull.Value;
            }
            cmd.Connection = _connection;
            _connection.Open();

            return cmd;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        public int Execute(string commandText, params IDbDataParameter[] dataParameters)
        {
            var cmd = CreateCommand(commandText, dataParameters);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T GetSingle<T>(string commandText, params IDbDataParameter[] dataParameters)
        {
            var obj = GetSingle(commandText, dataParameters);
            return ObjectUtility.Cast<T>(obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetSingle(string commandText, params IDbDataParameter[] dataParameters)
        {
            var cmd = CreateCommand(commandText, dataParameters);
            return cmd.ExecuteScalar();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public DataSet GetDataSet(string commandText, params IDbDataParameter[] dataParameters)
        {
            var cmd = CreateCommand(commandText, dataParameters);
            var dr = cmd.ExecuteReader();
            if (dr.IsClosed) return null;
            DataSet set = new DataSet();
            if (dr.Depth != -1)
            {
                DataTable table = new DataTable("Table 1");
                table.Load(dr);
                table.AcceptChanges();
                set.Tables.Add(table);
            }
            int count = 2;
            while (dr.NextResult())
            {
                if (dr.Depth == -1) continue;
                DataTable table = new DataTable("Table " + count);
                table.Load(dr);
                table.AcceptChanges();
                set.Tables.Add(table);
                count++;
            }
            return set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public DataTable GetDataTable(string commandText, params IDbDataParameter[] dataParameters)
        {
            var cmd = CreateCommand(commandText, dataParameters);
            var dr = cmd.ExecuteReader();
            if (dr.IsClosed) return null;

            DataTable table = new DataTable("Table 1");
            table.Load(dr);
            table.AcceptChanges();
            return table;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T GetEntity<T>(string commandText, params IDbDataParameter[] dataParameters)
        {
            var obj = GetEntity(typeof(T), commandText, dataParameters);
            if (obj is T) return (T)obj;
            return default(T);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetEntity(Type type, string commandText, params IDbDataParameter[] dataParameters)
        {
            var cmd = CreateCommand(commandText, dataParameters);
            var dr = cmd.ExecuteReader();

            return dr.GetEntity(type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public System.Collections.Generic.IList<T> GetList<T>(string commandText, params IDbDataParameter[] dataParameters) where T : class, new()
        {
            var cmd = CreateCommand(commandText, dataParameters);
            var dr = cmd.ExecuteReader();
            return dr.GetList<T>();
        }
    }
}