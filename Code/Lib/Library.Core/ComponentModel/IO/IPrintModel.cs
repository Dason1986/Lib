using System.Collections.Generic;

namespace Library.IO
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPrintModel
    {
        /// <summary>
        /// ȡ��ӡ����
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetPrintObjects();
    }
}