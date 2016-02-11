using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Library
{
    /// <summary>
    /// 序號生成器
    /// </summary>
    public interface ISerialNumberBuilder
    {
        /// <summary>
        /// 當前號碼
        /// </summary>
        int CurrentNumber { get; }

        /// <summary>
        /// 序號格式
        /// </summary>
        string SerialNumberFormat { get; }

        /// <summary>
        /// 生成一批序號
        /// </summary>
        void CreateSerialNumber();

        /// <summary>
        /// 序號數量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 取出最新序號
        /// </summary>
        /// <returns></returns>
        string Dequeue();
    }

    /// <summary>
    ///
    /// </summary>
    public class SerialNumberBuilder : ISerialNumberBuilder
    {
        /// <summary>
        /// /
        /// </summary>
        public int CurrentNumber { get; private set; }

        /// <summary>
        /// /
        /// </summary>
        public string SerialNumberFormat { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int Buildrecord { get; private set; }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="format"></param>
        /// <param name="currentNumber"></param>
        /// <param name="buildrecord"></param>
        public void SetOption(string format, int currentNumber, int buildrecord)
        {
            if (format == null) throw new ArgumentNullException("format");
            SerialNumberFormat = format;
            CurrentNumber = currentNumber;
            Buildrecord = buildrecord;
        }

        /// <summary>
        ///
        /// </summary>
        protected internal static readonly Regex VariableRegex = new Regex(@"({(?<Key>[^=]*?):(?<Param>[^=]*?)})|({(?<Key>[^=]*?)})");

        private static readonly object Lockobj = new object();

        private readonly Queue<string> _numberQueue = new Queue<string>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>

        protected internal static IEnumerable<GroupCollection> GetEnumerateVariables(string s)
        {
            var matchCollection = VariableRegex.Matches(s);

            for (int i = 0; i < matchCollection.Count; i++)
            {
                yield return matchCollection[i].Groups;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void CreateSerialNumber()
        {
            lock (Lockobj)
            {
                if (SerialNumberFormat == null) throw new ArgumentException("SerialNumberFormat");
                var matches = GetEnumerateVariables(SerialNumberFormat);

                var groupCollections = matches as GroupCollection[] ?? matches.ToArray();
                for (int i = 1; i <= Buildrecord; i++)
                {
                    String numberstr = SerialNumberFormat;
                    foreach (var variable in groupCollections)
                    {
                        var param = variable["Param"].Value;

                        switch (variable["Key"].Value)
                        {
                            case "Date":
                                if (string.IsNullOrEmpty(param))
                                {
                                    param = "ddMMyyyy";
                                }

                                numberstr = numberstr.Replace(variable[0].Value, DateTime.Now.ToString(param));

                                break;

                            case "Number":

                                if (string.IsNullOrEmpty(param))
                                {
                                    param = "d8";
                                }

                                numberstr = numberstr.Replace(variable[0].Value, (CurrentNumber + i).ToString(param));

                                break;
                        }
                    }
                    _numberQueue.Enqueue(numberstr);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int Count
        {
            get { return _numberQueue.Count; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string Dequeue()
        {
            lock (Lockobj)
            {
                if (Count == 0)
                {
                    this.CreateSerialNumber();
                }

                var sn = _numberQueue.Dequeue();
                CurrentNumber++;
                return sn;
            }
        }
    }
}