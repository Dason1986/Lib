using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Library.Annotations;

namespace Library.HelperUtility
{
    /// <summary>
    /// 
    /// </summary>
    public interface IErrorMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        Type ErrorType { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        string GetMessage(Exception exception);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool CanExcute(Exception exception);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ErrorMessageBuilder : IErrorMessageBuilder
    {
        private readonly Func<Exception, string> _fun;
        public Type ErrorType { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="fun"></param>
        public ErrorMessageBuilder([NotNull] Type errorType, [NotNull] Func<Exception, string> fun)
        {
            if (errorType == null) throw new ArgumentNullException("errorType");
            this.ErrorType = errorType;
            if (fun == null) throw new ArgumentNullException("fun");
            _fun = fun;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public string GetMessage(Exception exception)
        {
            return _fun(exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool CanExcute(Exception exception)
        {
            return ErrorType.IsInstanceOfType(exception);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetAllExceptionInfo(Exception ex)
        {
            StringBuilder sbexception = new StringBuilder();
            var e = ex;
            int i = 1;
            sbexception.Append(GetExceptionInfo(e, i));

            while (e.InnerException != null)
            {
                i++;
                e = e.InnerException;
                sbexception.Append(GetExceptionInfo(e, i));
            }

            return sbexception.ToString();
        }
        static readonly IList<IErrorMessageBuilder> ErrorFuncs = new List<IErrorMessageBuilder>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void AddMessageBuilder([NotNull] IErrorMessageBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException("builder");
            ErrorFuncs.Add(builder);
        }

        static TypeHelper()
        {
            ErrorFuncs.Add(new ErrorMessageBuilder(typeof(ArgumentException), n => string.Format(" ParamName : {0} ", ((ArgumentException)n).ParamName)));
            ErrorFuncs.Add(new ErrorMessageBuilder(typeof(SqlException), n =>
            {
                var sql = (SqlException)n;
                StringWriter builder = new StringWriter();
                builder.WriteLine("ErrorCode:{0}", sql.ErrorCode);
                builder.WriteLine("LineNumber:{0}", sql.LineNumber);
                builder.WriteLine("Number:{0}", sql.Number);
                builder.WriteLine("Procedure:{0}", sql.Procedure);
                builder.WriteLine("Server:{0}", sql.Server);
                builder.WriteLine("Source:{0}", sql.Source);
                builder.WriteLine("State:{0}", sql.State);
                if (sql.Errors.HasRecord())
                {
                    int index = 1;
                    builder.WriteLine("=================SQL Error {0}=================================", index);
                    foreach (SqlError sqlError in sql.Errors)
                    {

                        builder.WriteLine("LineNumber:{0}", sqlError.LineNumber);
                        builder.WriteLine("Number:{0}", sqlError.Number);
                        builder.WriteLine("Procedure:{0}", sqlError.Procedure);
                        builder.WriteLine("Server:{0}", sqlError.Server);
                        builder.WriteLine("Source:{0}", sqlError.Source);
                        builder.WriteLine("State:{0}", sqlError.State);
                        index++;
                    }
                }
                return builder.ToString();
            }));
            ErrorFuncs.Add(new ErrorMessageBuilder(typeof(OdbcException), n =>
            {
                var sql = (OdbcException)n;
                StringWriter builder = new StringWriter();
                builder.WriteLine("ErrorCode:{0}", sql.ErrorCode);
                builder.WriteLine("Source:{0}", sql.Source);
                if (sql.Errors.HasRecord())
                {
                    int index = 1;
                    builder.WriteLine("=================SQL Error {0}=================================", index);
                    foreach (OdbcError sqlError in sql.Errors)
                    {

                        builder.WriteLine("Message:{0}", sqlError.Message);
                        builder.WriteLine("NativeError:{0}", sqlError.NativeError);
                        builder.WriteLine("SQLState:{0}", sqlError.SQLState);
                        builder.WriteLine("Source:{0}", sqlError.Source);
                        index++;
                    }
                }
                return builder.ToString();
            }));
            ErrorFuncs.Add(new ErrorMessageBuilder(typeof(OleDbException), n =>
            {
                var sql = (OleDbException)n;
                StringWriter builder = new StringWriter();
                builder.WriteLine("ErrorCode:{0}", sql.ErrorCode);
                builder.WriteLine("Source:{0}", sql.Source);
                if (sql.Errors.HasRecord())
                {
                    int index = 1;
                    builder.WriteLine("=================SQL Error {0}=================================", index);
                    foreach (OleDbError sqlError in sql.Errors)
                    {
                        builder.WriteLine("Message:{0}", sqlError.Message);
                        builder.WriteLine("NativeError:{0}", sqlError.NativeError);
                        builder.WriteLine("SQLState:{0}", sqlError.SQLState);
                        builder.WriteLine("Source:{0}", sqlError.Source);
                        index++;
                    }
                }
                return builder.ToString();
            }));

        }
        private static string GetExceptionInfo(Exception ex, int count)
        {
            StringBuilder sbexception = new StringBuilder();
            sbexception.AppendLine();

            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(" Inner Exception : No.{0} ", count));
            sbexception.AppendLine(string.Format(" Error Message : {0} ", ex.Message));
            foreach (var dicitem in ErrorFuncs)
            {

                if (dicitem != null && dicitem.CanExcute(ex))
                {
                    sbexception.AppendLine(dicitem.GetMessage(ex));
                }
            }
            if (ex.Data.HasRecord())
            {
                sbexception.AppendLine(string.Format(" Data parameters Count at Source :{0}", ex.Data.Count));
                try
                {
                    if (ex.Data.Keys.Count > 0)
                    {

                        sbexception.AppendLine("==================================================");
                        foreach (object key in ex.Data.Keys)
                        {
                            try
                            {
                                if (key != null)
                                {
                                    var skey = Convert.ToString(key);
                                    sbexception.AppendLine(string.Format(" Key :{0} , Value:{1}", skey, Convert.ToString(ex.Data[key])));
                                }
                                else
                                {
                                    sbexception.AppendLine(string.Format(" Key is null"));
                                }
                            }
                            catch (Exception e1)
                            {
                                sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", e1.Message));
                            }
                        } sbexception.AppendLine("==================================================");
                    }
                }
                catch (Exception ex1)
                {
                    sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", ex1.Message));
                }
            }


            sbexception.AppendLine(string.Format(" Source : {0} ", ex.Source));
            sbexception.AppendLine(string.Format(" StackTrace : {0} ", ex.StackTrace));
            sbexception.AppendLine(string.Format(" TargetSite : {0} ", ex.TargetSite));
            sbexception.AppendLine(string.Format(" Finished Writting Exception info :{0} ", count));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine();

            return sbexception.ToString();

        }

        public static Type RemoveNullabl([NotNull] this Type obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var type = obj;

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNumberType([NotNull] this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            switch (type.FullName)
            {
                case "System.Char":
                case "System.Decimal":
                case "System.Double":
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UDouble":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.IntPtr":
                case "System.Single":
                    return true;
            }
            return false;
        }
    }
}