using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Library.DBProvider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDBUtility
    {
        /// <summary>
        /// 
        /// </summary>
        string ConnectionString { set; }
        /// <summary>
        /// 
        /// </summary>
        ITextExecute TextExecute { get; }
        /// <summary>
        /// 
        /// </summary>
        IDBExecute ProcedureExecute { get; }
        /// <summary>
        /// 
        /// </summary>
        IDBExecuteByCmd ExecuteByCmd { get; }
        /// <summary>
        /// 
        /// </summary>
        System.Version DBVersion { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDBExecute
    {
        /// <summary>
        /// 執行Sql語句
        /// </summary>
        /// <param name="commandText">命令</param>
        /// <param name="dataParameters">參數</param>
        /// <returns>影響行數</returns>
        int Execute(string commandText, params System.Data.IDbDataParameter[] dataParameters);

        /// <summary>
        /// 查詢，第一行第一列數據
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        T GetSingle<T>(string commandText, params System.Data.IDbDataParameter[] dataParameters);

        /// <summary>
        /// 查詢，第一行第一列數據
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns></returns>
        object GetSingle(string commandText, params System.Data.IDbDataParameter[] dataParameters);
        /// <summary>
        /// 查證，返回數據表
        /// </summary>
        /// <param name="commandText">命令</param>
        /// <param name="dataParameters">參數</param>
        /// <returns>返回數據表</returns>
        DataSet GetDataSet(string commandText, params System.Data.IDbDataParameter[] dataParameters);
        /// <summary>
        /// 查證，返回數據表
        /// </summary>
        /// <param name="commandText">命令</param>
        /// <param name="dataParameters">參數</param>
        /// <returns>返回數據表</returns>
        DataTable GetDataTable(string commandText, params System.Data.IDbDataParameter[] dataParameters);
        /// <summary>
        /// 查詢，返回第一條記錄實體
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns>實體</returns>
        T GetEntity<T>(string commandText, params System.Data.IDbDataParameter[] dataParameters);

        /// <summary>
        /// 查詢，返回第一條記錄實體
        /// </summary>
        /// <param name="type"></param>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns>實體</returns>
        object GetEntity(Type type, string commandText, params System.Data.IDbDataParameter[] dataParameters);
        /// <summary>
        /// 查詢，返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="dataParameters"></param>
        /// <returns>實體</returns>
        IList<T> GetList<T>(string commandText, params System.Data.IDbDataParameter[] dataParameters) where T : class,new();
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDBExecuteByCmd
    {
        /// <summary>
        /// 執行Sql語句
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>影響行數</returns>
        int Execute(IDbCommand command);

        /// <summary>
        /// 查詢，第一行第一列數據
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">命令</param>
        /// <returns></returns>
        T GetSingle<T>(IDbCommand command);

        /// <summary>
        /// 查詢，第一行第一列數據
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        object GetSingle(IDbCommand command);
        /// <summary>
        /// 查證，返回數據表
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>返回數據表</returns>
        DataSet GetDataSet(IDbCommand command);


        /// <summary>
        /// 查證，返回數據表
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>返回數據表</returns>
        DataTable GetDataTable(IDbCommand command);
        /// <summary>
        /// 查詢，返回第一條記錄實體
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">命令</param>
        /// <returns>實體</returns>
        T GetEntity<T>(IDbCommand command) where T : class,new();

        /// <summary>
        /// 查詢，返回第一條記錄實體
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>實體</returns>
        object GetEntity(IDbCommand command);
        /// <summary>
        /// 查詢，返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">命令</param>
        /// <returns>實體</returns>
        IList<T> GetList<T>(IDbCommand command) where T : class,new();
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ITextExecute : IDBExecute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        int ExecuteTransaction(System.Collections.Generic.IEnumerable<string> commands);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        int ExecuteTransactionMaybyHasGo(string command);
    }
}
