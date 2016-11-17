using NLog.Revicer.Models;
using System.Runtime.Serialization.Json;

namespace NLog.Revicer
{

    public class NLogJsonProvider : ILogProvider
    {

      
        public SourceLog Log(string source)
        {
        return     Newtonsoft.Json.JsonConvert.DeserializeObject<SourceLog>(source);
         

        }
        public SourceLog Log(byte[] buff)
        {
            
       
            return Log(System.Text.Encoding.UTF8.GetString(buff)); ;
        }
    }
}