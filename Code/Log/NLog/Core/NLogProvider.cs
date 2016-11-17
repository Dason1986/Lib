using NLog.Revicer.Models;
using System;

namespace NLog.Revicer
{
    public class NLogProvider : ILogProvider
    {

        SourceLog Log(string[] source)
        {
            if (source == null || source.Length < 4) throw new Exception();//format error
            var log = new SourceLog(source);
            log.Time = DateTime.Parse(source[0]);
            log.LogType = source[1];
            log.Logger = source[2];
            log.Message = source[3];
            if (source.Length > 4) log.Error =new LogException { Message = source[4] };
            return log;
        }
        public SourceLog Log(string source)
        {
            return Log(source.Split('|'));
        }
        public SourceLog Log(byte[] buff)
        {
            return Log(System.Text.Encoding.UTF8.GetString(buff).Split('|'));
        }
    }
}