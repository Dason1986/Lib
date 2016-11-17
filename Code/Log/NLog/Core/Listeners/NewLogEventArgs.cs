using NLog.Revicer.Models;
using System;

namespace NLog.Revicer.Listeners
{
    public class NewLogEventArgs : EventArgs
    {

        public SourceLog Log { get; private set; }

        public NewLogEventArgs(SourceLog log)
        {
            this.Log = log;
        }
    }
}