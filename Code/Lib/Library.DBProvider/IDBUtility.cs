using System;
using System.Collections.Generic;
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

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IDBExecuteByCmd
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public interface ITextExecute : IDBExecute
    {

    }
}
