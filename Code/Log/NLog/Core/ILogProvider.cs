using NLog.Revicer.Models;

namespace NLog.Revicer
{
    public interface ILogProvider
    {
      
        SourceLog Log(string source);
        SourceLog Log(byte[] buff);
    }
}