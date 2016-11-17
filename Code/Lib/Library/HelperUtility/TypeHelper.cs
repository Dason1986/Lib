using Library.Annotations;
using Library.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static string GetFullName(this Type classType)
        {

            return string.Format("{0},{1}", classType.FullName, classType.Assembly.GetName().Name);
        }
        #region 类型搜索
        /// <summary>
        /// 获取子类型
        /// </summary>
        /// <param name="type">父类型</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubTypes(Type type)
        {
            var assemblies =AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            {
                Assembly assembly = type.GetType().Assembly;
                //基类所在程序集或依赖于基类的其他程序集
                return a.FullName == assembly.FullName || a.GetReferencedAssemblies().Any(ra => ra.FullName == assembly.FullName);
            });

            var typeInfo = type;

            return assemblies.SelectMany(a =>
            {
                return a.GetTypes().Where(t =>
                {
                    if (type == t)
                    {
                        return false;
                    }

                    var tInfo = t;

                    if (tInfo.IsAbstract || !tInfo.IsClass || !tInfo.IsPublic)
                    {
                        return false;
                    }

                    //if (typeInfo.IsGenericTypeDefinition)
                    //{
                    //    return type.ge(t);
                    //}

                    return type.IsAssignableFrom(t);
                });
            });
        }

        /// <summary>
        /// 获取子类型
        /// </summary>
        /// <typeparam name="T">父类型</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubTypes<T>()
        {
            return GetSubTypes(typeof(T));
        }
        #endregion
        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetAllExceptionInfo(this Exception ex)
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
        /*
        private static readonly IList<IErrorMessageBuilder> ErrorFuncs = new List<IErrorMessageBuilder>();

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
            ErrorFuncs.Add(new ErrorMessageBuilder(typeof(ArgumentException),
                n => string.Format(" ParamName : {0} ", ((ArgumentException)n).ParamName)));
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

        

        private static readonly Dictionary<Type, Func<Type, object>> TypeDefaultdictionary = new Dictionary<Type, Func<Type, object>>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="funk"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetTypeDefault([NotNull] Type type, [NotNull] Func<Type, object> funk)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (funk == null) throw new ArgumentNullException("funk");
            if (TypeDefaultdictionary.ContainsKey(type))
            {
                TypeDefaultdictionary[type] = funk;
                return;
            }
            TypeDefaultdictionary.Add(type, funk);
        }
      
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetDefaultValue([NotNull] this Type obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (TypeDefaultdictionary.ContainsKey(obj)) return TypeDefaultdictionary[obj](obj);
            return obj.IsClass ? null : obj.CreateInstance();
        }
  */
        private static string GetExceptionInfo(Exception ex, int count)
        {
            StringBuilder sbexception = new StringBuilder();
            sbexception.AppendLine();

            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(" Inner Exception : No.{0} ", count));
            sbexception.AppendLine(string.Format(" Error Message : {0} ", ex.Message));
            //foreach (var dicitem in ErrorFuncs)
            //{
            //    if (dicitem != null && dicitem.CanExcute(ex))
            //    {
            //        sbexception.AppendLine(dicitem.GetMessage(ex));
            //    }
            //}
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
                                    sbexception.AppendLine(string.Format(" Key :{0} , Value:{1}", skey,
                                        Convert.ToString(ex.Data[key])));
                                }
                                else
                                {
                                    sbexception.AppendLine(string.Format(" Key is null"));
                                }
                            }
                            catch (Exception e1)
                            {
                                sbexception.AppendLine(
                                    string.Format("**  Exception occurred when writting log *** [{0}] ", e1.Message));
                            }
                        }
                        sbexception.AppendLine("==================================================");
                    }
                }
                catch (Exception ex1)
                {
                    sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ",
                        ex1.Message));
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
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listAccessors"></param>
        /// <returns></returns>
        public static PropertyDescriptorCollection GetListItemProperties(object list, PropertyDescriptor[] listAccessors = null)
        {
            PropertyDescriptorCollection pdc = null;

            object target = list;

            if (target is ITypedList)
            {
                return (target as ITypedList).GetItemProperties(listAccessors);
            }
            var type = target.GetType();
            if (IsListBasedType(type))
            {
                var basetype = GetListGenericType(type);

                pdc = TypeDescriptor.GetProperties(basetype);
            }

            return pdc;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsListBasedType(Type type)
        {
            // check for IList, ITypedList, IListSource
            if (typeof(IList).IsAssignableFrom(type) ||
                typeof(ITypedList).IsAssignableFrom(type) ||
                typeof(IListSource).IsAssignableFrom(type))
            {
                return true;
            }

            // check for IList<>:
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                if (typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    return true;
                }
            }

            // check for SomeObject<T> : IList<T> / SomeObject : IList<(SpecificListObjectType)>
            foreach (Type curInterface in type.GetInterfaces())
            {
                if (curInterface.IsGenericType)
                {
                    if (typeof(IList<>).IsAssignableFrom(curInterface.GetGenericTypeDefinition()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetListGenericType(Type type)
        {
            // check for IList<>:
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                if (typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    return type.GetGenericArguments()[0];
                }
            }

            // check for SomeObject<T> : IList<T> / SomeObject : IList<(SpecificListObjectType)>
            foreach (Type curInterface in type.GetInterfaces())
            {
                if (curInterface.IsGenericType)
                {
                    if (typeof(IList<>).IsAssignableFrom(curInterface.GetGenericTypeDefinition()))
                    {
                        return curInterface.GetGenericArguments()[0];
                    }
                }
            }

            return null;
        }
    }
}