using Library.Infrastructure.Application;
using NLog;
using System;

namespace Nlog.Send
{
    internal class Program
    {
        static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            //if (LogManager.Configuration != null)
            //    foreach (NLog.Targets.TargetWithLayout item in LogManager.Configuration.AllTargets)
            //    {
            //        item.Layout = new CustomJsonLayout();
            //    }
            logger.Info("start");
            Random random = new Random();
            while (true)
            {


                switch (random.Next(1, 7))
                {
                    case 1:
                        {
                            LogClassTrace trace = new LogClassTrace();
                            trace.Log();
                            break;
                        }
                    case 2:
                        {
                            LogClassDebug trace = new LogClassDebug();
                            trace.Log(); break;
                        }
                    case 3:
                        {
                            LogClassInfo trace = new LogClassInfo();
                            trace.Log(); break;
                        }
                    case 4:
                        {
                            LogClassWarn trace = new LogClassWarn();
                            trace.Log(); break;
                        }
                    case 5:
                        {
                            LogClassError trace = new LogClassError();

                            trace.Log(); break;
                        }
                    case 6:
                        {
                            LogClassFatal trace = new LogClassFatal();
                            trace.Log(); break;
                        }
                    default:
                        break;
                }
                var code = Console.ReadLine();
                if (code == "exit")
                {
                    LogManager.Shutdown();
                }
            }
        }


    }
    class LogClassError
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Log()
        {
            LogEventInfo theEvent = new LogEventInfo()
            {
                Message = "Pass my custom value",
                Level = LogLevel.Error,
                Exception = new Exception("fff", new NotImplementedException("a1")),
                LoggerName = logger.Name
            };
            theEvent.Properties["a1"] = "My custom ";
            theEvent.Properties["a2"] = "string";
            //    theEvent.Context["TheAnswer"] = 42;
            logger.Log(theEvent);
            //  logger.Error(theEvent.Exception,theEvent.Message);
        }
    }
    class LogClassDebug
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Log()
        {
            LogEventInfo theEvent = new LogEventInfo()
            {
                Message = "LogClassDebug",
                Level = LogLevel.Debug,

                LoggerName = logger.Name
            };
            theEvent.Properties["context"] = "My custom ";
            //    theEvent.Context["TheAnswer"] = 42;
            logger.Log(theEvent);

        }
    }
    class LogClassInfo
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Log()
        {
            logger.InfoByContent( "LogClassInfo", "695");
          
        }
    }
    class LogClassWarn
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Log()
        {
            logger.WarnByContent("LogClassWarn",DateTime.Now);
      
        }
    }
    class LogClassTrace
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Log()
        {
            logger.Trace("LogClassTrace");
        }
    }
    class LogClassFatal
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Log()
        {
            logger.Fatal("LogClassFatal");
        }
    }
}
