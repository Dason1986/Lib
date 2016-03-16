using System.Reflection;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class AssemblyUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetResourcesFileAllTextByCallingAssembly(string path)
        {
            return GetResourcesFileAllText(Assembly.GetCallingAssembly(), path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetResourcesFileAllTextByExecutingAssembly(string path)
        {
            return GetResourcesFileAllText(Assembly.GetExecutingAssembly(), path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetResourcesFileAllText(this Assembly assembly, string path)
        {
            if (assembly == null) return null;
            var stream = assembly.GetManifestResourceStream(path);
            if (stream == null) return null;
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            var str = reader.ReadToEnd();
            reader.Dispose();
            return str;
        }
    }
}