using Library.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Guid GetFileGuid(string path)
        {
            if (!File.Exists(path)) return Guid.Empty;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] bs = MD5.Create().ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            return new Guid(bs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Guid GetFileGuid(Stream stream)
        {
            if (stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);


            byte[] bs = MD5.Create().ComputeHash(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new Guid(bs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public static string GetFileSizeDisplay(long filesize)
        {
            if (filesize < 1024) return string.Format("{0}b", filesize);
            string[] unit = { "KB", "MB", "GB", "TB", "PB" };
            const int filter = 1024;
            long unitsize = 1;
            var flag = true;
            decimal size = filesize;
            int index = -1;
            while (flag)
            {
                size = size / filter;
                unitsize = unitsize * filter;
                flag = size > filter;
                index++;
                if (index >= unit.Length - 1) flag = false;
            }
            return string.Format("{0:f2}{1}", size, unit[index]);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static System.Text.Encoding GetType(Stream fs)
        {
            /*
                                Unicode    
                                ------------------
                                255    254

                                ======================
                                UnicodeBigEndian
                                -------------------
                                254    255

                                ======================
                                UTF8
                                -------------------
                                34 228
                                34 229
                                34 230
                                34 231
                                34 232
                                34 233
                                239    187
             
                                ======================
                                ANSI
                                -------------------
                                34 176
                                34 177
                                34 179
                                34 180
                                34 182
                                34 185
                                34 191
                                34 194
                                34 196
                                34 198
                                34 201
                                34 202
                                34 205
                                34 206
                                34 208
                                34 209
                                34 210
                                34 211
                                34 213
                                196 167
                                202 213
                                206 228
            */
            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            byte[] ss = r.ReadBytes(3);
            int lef = ss[0];
            int mid = ss[1];
            int rig = ss[2];
            r.Close();
            /*  文件头两个字节是255 254，为Unicode编码；
                文件头三个字节  254 255 0，为UTF-16BE编码；
                文件头三个字节  239 187 191，为UTF-8编码；*/
            if (lef == 255 && mid == 254)
            {
                return Encoding.Unicode;
            }
            else if (lef == 254 && mid == 255 && rig == 0)
            {
                return Encoding.BigEndianUnicode;
            }
            else if (lef == 254 && mid == 255)
            {
                return Encoding.BigEndianUnicode;
            }
            else if (lef == 239 && mid == 187 && rig == 191)
            {
                return Encoding.UTF8;
            }
            else if (lef == 239 && mid == 187)
            {
                return Encoding.UTF8;
            }
            else if (lef == 196 && mid == 167
                || lef == 206 && mid == 228
                || lef == 202 && mid == 213)
            {
                return Encoding.Default;
            }
            else
            {
                if (lef == 34)
                {
                    if (mid < 220) return Encoding.Default;
                    else return Encoding.UTF8;
                }
                else
                {
                    if (lef < 220) return Encoding.Default;
                    else return Encoding.UTF8;
                }
            }
        }

    }
}