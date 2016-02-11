using Library.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public class FileUtility
    {
        /// <summary>
        /// 取文件MD5值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FileMD5(string path)
        {
            if (!File.Exists(path)) return string.Empty;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] bs = MD5.Create().ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            return BitConverter.ToString(bs).Replace("-", "");
        }

        /// <summary>
        /// 取文件MD5值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string FileMD5(Stream stream)
        {
            if (!stream.TrySeek()) return string.Empty;
            byte[] bs = MD5.Create().ComputeHash(stream);
            stream.TrySeek();

            return BitConverter.ToString(bs).Replace("-", "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffter"></param>
        /// <returns></returns>
        public static string FileMD5(byte[] buffter)
        {
            var md5B = MD5.Create().ComputeHash(buffter);
            return BitConverter.ToString(md5B).Replace("-", "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetFileToDictionary(string path)
        {
            var upgrade = new Dictionary<string, string>(CompareUtility.StringEqualityComparer);
            if (File.Exists(path))
            {
                var dr = new StreamReader(path);
                while (!dr.EndOfStream)
                {
                    var readLine = dr.ReadLine();
                    if (string.IsNullOrEmpty(readLine) || readLine.StartsWith("--") || readLine.StartsWith("//") || readLine.StartsWith("##"))
                        continue;
#if !SILVERLIGHT
                    var arr = readLine.Split(new[] { '=' }, 2);
#else
                    var arr = readLine.Split(new[] { '=' });
#endif
                    if (arr.Length == 2)
                    {
                        upgrade.Add(arr[0], arr[1]);
                    }
                }
                dr.Close();
                dr.Dispose();
            }
            return upgrade;
        }

        private static readonly IDictionary<string, string[]> CodeDic = new Dictionary<string, string[]>
                          {
                              {"255216", new[] {"jpg"}},
                              {"071073", new[] {"gif"}},
                              {"137080", new[] {"png"}},
                              {"067087", new[] {"swf"}},
                              {"082097", new[] {"rar"}},
                              {"080075", new[] {"zip", "docx", "xlsx", "pptx", "jad", "apk", "xap"}},
                              {"077090", new[] {"exe", "dll"}},
                              {"055122", new[] {"7z"}},
                              {"208207", new[] {"xls", "doc", "msi", "ppt"}},
                              {"000001", new[] {"mdb"}},
                          };

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string[] MaybeExtension(string code)
        {
            return CodeDic.GetValue(code, EmptyUtility<string>.EmptyArray);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetFileExtensionCode([NotNull] string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!File.Exists(path)) throw new FileLoadException("path");
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            System.IO.BinaryReader br = new BinaryReader(stream);

            byte buffer = br.ReadByte();
            string fileclass = buffer.ToString(CultureInfo.InvariantCulture);
            buffer = br.ReadByte();
            fileclass += buffer.ToString(CultureInfo.InvariantCulture);

            br.Close();
            stream.Close();
            return fileclass;
        }

        /// <summary>
        ///  通过文件头取扩展名编号
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetFileCode(Stream stream)
        {
            if (!stream.TrySeek()) return string.Empty;
            var r = new BinaryReader(stream);

            byte buffer = r.ReadByte();
            string bx = buffer.ToString("D3");
            buffer = r.ReadByte();
            bx += buffer.ToString("D3");

            stream.TrySeek();

            return bx;
        }

        /// <summary>
        ///  通过文件头取扩展名编号
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileCode(string path)
        {
            if (!File.Exists(path)) return string.Empty;

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (fs.Length <= 0)
            {
                fs.Close();
                return string.Empty;
            }
            string code = GetFileCode(fs);
            fs.Close();
            fs.Dispose();
            return code;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetFileCode(byte[] buffter)
        {
            if (buffter == null) throw new ArgumentNullException("buffter");
            Stream stream = new MemoryStream(buffter);

            string fileclass = GetFileCode(stream);
            stream.Dispose();

            return fileclass;
        }
    }
}