using System;
using System.Text;
using Library.Annotations;

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
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetAllExceptionInfo(Exception ex)
        {
            StringBuilder sbexception = new StringBuilder();

            int i = 1;
            sbexception.Append(GetExceptionInfo(ex, i));

            while (ex.InnerException != null)
            {
                i++;
                ex = ex.InnerException;
                sbexception.Append(GetExceptionInfo(ex, i));
            }

            return sbexception.ToString();
        }

        private static string GetExceptionInfo(Exception ex, int count)
        {
            StringBuilder sbexception = new StringBuilder();
            sbexception.AppendLine();

            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(" Inner Exception : No.{0} ", count));
            sbexception.AppendLine(string.Format(" Error Message : {0} ", ex.Message));
            sbexception.AppendLine(string.Format(" Data parameters Count at Source :{0}", ex.Data.Count));
            try
            {
                string skey = string.Empty;
                if (ex.Data.Keys.Count > 0)
                {

                    sbexception.AppendLine(string.Format("=================================================="));
                    foreach (object key in ex.Data.Keys)
                    {
                        try
                        {
                            if (key != null)
                            {
                                skey = Convert.ToString(key);
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
                    } sbexception.AppendLine(string.Format("=================================================="));
                }
            }
            catch (Exception ex1)
            {
                sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", ex1.Message));
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