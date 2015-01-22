
using Library.HelperUtility;

namespace Library.Draw.Print
{
    /// <summary>
    /// 
    /// </summary>
    public static class PrintBuilderHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IPrintBuilder FactoryBuilder(object model)
        {
            if (model == null) throw new PrintException("打印目标为空", 14001.001);
            var att = Library.HelperUtility.AttributeHelper.GetAttribute<PrintActionAttribute>(model);
            if (att == null) throw new PrintException("对象没有指定[PrintActionAttribute]", 14001.011);
            if (att.ClassType == null) throw new PrintException("对象没有指定[IPrintBuilder]", 14001.012);
            var builder = att.ClassType.CreateInstance<IPrintBuilder>();
            if (builder == null) throw new PrintException("创建Builder为空", 14001.021);

            return builder;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        public static void PrintToImageFile(IPrintBuilder builder,string path)
        {

        }

    }
}