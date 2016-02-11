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
        public static string GetCallingAssemblyAllText(string path)
        {
            return GetAllText(Assembly.GetCallingAssembly(), path);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExecutingAssemblyAllText(string path)
        {
            return GetAllText(Assembly.GetExecutingAssembly(), path);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAllText(this Assembly assembly, string path)
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