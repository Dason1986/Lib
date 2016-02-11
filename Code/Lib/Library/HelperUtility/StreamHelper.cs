using System;
using System.IO;
using System.Text;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bytes"></param>
        public static void WriteBytes(this Stream stream, byte[] bytes)
        {
            if (stream == null || !stream.CanWrite || !bytes.HasRecord()) return;
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="traget"></param>
        public static TryResult Copy(this Stream source, Stream traget)
        {
            if (source == null) return new ArgumentNullException("source");
            if (!source.CanRead) return new Exception("source Can't Read");
            if (source.Length == 0) return true;
            if (!source.CanSeek && source.Position != 0) return new Exception("source Can't Read");
            if (traget == null) return new ArgumentNullException("traget");
            if (!traget.CanWrite) return new Exception("traget Can't Write");
            if (!traget.CanSeek && traget.Position != 0) return new Exception("traget Can't Read");
            if (source is MemoryStream)
            {
                var sm = (MemoryStream)source;
                sm.WriteTo(traget);
            }
            else
            {
                if (!source.CanSeek) return new Exception("source Can't Seek");
                var buffer = traget.ToArray();
                source.WriteBytes(buffer);
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        public static TryResult WriteString(this Stream stream, string str, Encoding encoding = null)
        {
            if (stream == null) return new ArgumentNullException("stream");
            if (!stream.CanWrite) return new Exception("Can't Write");
            if (string.IsNullOrWhiteSpace(str)) return new ArgumentNullException("str");
            var useencoding = encoding ?? Encoding.UTF8;
            byte[] buffter = useencoding.GetBytes(str);
            stream.Write(buffter, 0, buffter.Length);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public static string ReadString(this Stream stream, Encoding encoding = null)
        {
            var buffter = stream.ToArray();
            if (buffter.Length == 0) return string.Empty;
            var useencoding = encoding ?? Encoding.UTF8;
            return useencoding.GetString(buffter, 0, buffter.Length);
        }

        private const int Nchar = 10;
        private const int Rchar = 13;
        private const int Lenght = 2048;

        /// <summary>
        /// 大文件时，读取最后一行
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadLastLine(this Stream stream, Encoding encoding = null)
        {
            if (stream == null || !stream.CanRead || !stream.CanSeek || stream.Length == 0) return string.Empty;
            var useencoding = encoding ?? Encoding.UTF8;
            stream.Seek(-1, SeekOrigin.End);

            bool islast = false;
            var hasMulLine = false;
            int index = Lenght;
            byte[] buffter = new byte[Lenght];
            StringBuilder stringBuilder = new StringBuilder();
            while (!islast)
            {
                if (stream.Position == 0) break;

                var by = (byte)stream.ReadByte();

                switch (by)
                {
                    case Nchar:
                    case Rchar:
                        islast = true; break;
                    default:
                        index--;
                        buffter[index] = by;
                        if (index == 0)
                        {
                            hasMulLine = true;
                            stringBuilder.Insert(0, useencoding.GetString(buffter, index, Lenght - index));
                            index = Lenght;
                        }
                        stream.Seek(-2, SeekOrigin.Current);
                        break;
                }
            }
            if (!hasMulLine) return useencoding.GetString(buffter, index, Lenght - index);

            stringBuilder.Insert(0, useencoding.GetString(buffter, index, Lenght - index));
            return stringBuilder.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        public static byte[] ToArray(this Stream stream)
        {
            if (stream is MemoryStream) return ((MemoryStream)stream).ToArray();
            if (stream == null || !stream.CanRead) return new byte[0];
            if (!stream.CanSeek && stream.Position != 0) return new byte[0];
            if (stream.CanSeek && stream.Position != 0) stream.Seek(0, SeekOrigin.Begin);

            byte[] buffter = new byte[stream.Length];
            stream.Read(buffter, 0, buffter.Length);
            //stream.Seek(0, SeekOrigin.Begin);
            return buffter;
        }

        /// <summary>
        /// 跳到起始位置
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool TrySeek(this Stream stream)
        {
            if (stream == null || !stream.CanRead) return false;
            if (!stream.CanSeek && stream.Position != 0) return false;
            if (stream.CanSeek && stream.Position != 0) stream.Seek(0, SeekOrigin.Begin);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"> </param>
        public static byte[] ToArray(this Stream stream, int length)
        {
            if (stream is MemoryStream) return ((MemoryStream)stream).ToArray();
            if (stream == null || !stream.CanRead) return new byte[0];
            if (!stream.CanSeek && stream.Position != 0) return new byte[0];
            if (stream.CanSeek && stream.Position != 0) stream.Seek(0, SeekOrigin.Begin);

            byte[] buffter = new byte[length];
            stream.Read(buffter, 0, length);
            //stream.Seek(0, SeekOrigin.Begin);
            return buffter;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadToEnd(this Stream stream)
        {
            if (stream == null || !stream.CanRead) return new byte[0];
            byte[] bytes = new byte[stream.Length - stream.Position];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}