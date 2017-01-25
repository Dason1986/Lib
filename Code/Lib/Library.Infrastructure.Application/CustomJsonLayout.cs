using NLog;
using NLog.Config;
using NLog.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Application
{
    /// <summary>
    ///
    /// </summary>
    [AppDomainFixedOutput]
    [Layout("CustomJsonLayout")]
    [ThreadAgnostic]
    public class CustomJsonLayout : JsonLayout
    {
        /// <summary>
        ///
        /// </summary>
        public CustomJsonLayout()
        {
            var jsonLayout = new JsonLayout
            {
                Attributes =
             {
                new JsonAttribute("Type", "${exception:format=Type}"),
                new JsonAttribute("Message", "${exception:format=Message}"),
                new JsonAttribute("InnerException", new JsonLayout
                                {
                                    Attributes =
                                    {
                                        new JsonAttribute("Type", "${exception:format=:innerFormat=Type:MaxInnerExceptionLevel=1:InnerExceptionSeparator=}"),
                                        new JsonAttribute("Message", "${exception:format=:innerFormat=Message:MaxInnerExceptionLevel=1:InnerExceptionSeparator=}"),
                                    },RenderEmptyObject = false
                                },

                                false)
                            },
                RenderEmptyObject = false
            };

            Attributes.Add(new JsonAttribute("ID", Layout.FromString("${guid}")));
            Attributes.Add(new JsonAttribute("Time", Layout.FromString("${longdate}")));
            Attributes.Add(new JsonAttribute("LogType", Layout.FromString("${level:upperCase=true}")));
            Attributes.Add(new JsonAttribute("Logger", Layout.FromString("${logger}")));
            Attributes.Add(new JsonAttribute("Message", Layout.FromString("${Message}")));

            Attributes.Add(new JsonAttribute("Error", jsonLayout, false));
            Attributes.Add(new JsonAttribute("Appdomain", Layout.FromString("${appdomain}")));
            Attributes.Add(new JsonAttribute("Callsite", Layout.FromString("${callsite}")));
            Attributes.Add(new JsonAttribute("Context", Layout.FromString("${all-event-properties}")));
            Attributes.Add(new JsonAttribute("Machinename", Layout.FromString("${machinename}")));
            Attributes.Add(new JsonAttribute("Processname", Layout.FromString("${processname}")));
            Attributes.Add(new JsonAttribute("Processtime", Layout.FromString("${processtime}")));
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static class LoggerHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="dictionary"></param>
        public static void Error(this ILogger logger, Exception ex, string message, IDictionary dictionary)
        {
            Log(logger, LogLevel.Error, message, dictionary, ex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="dictionary"></param>
        /// <param name="ex"></param>
        public static void Log(this ILogger logger, LogLevel level, string message, IDictionary dictionary, Exception ex = null)
        {
            LogEventInfo theEvent = new LogEventInfo()
            {
                Message = message,
                Level = level,
                Exception = ex,
                LoggerName = logger.Name,
            };
            if (dictionary != null)
                foreach (var item in dictionary.Keys)
                {
                    theEvent.Properties.Add(item, dictionary[item]);
                }
            logger.Log(theEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public static void InfoByContent(this ILogger logger, string message, object content)
        {
            Dictionary<string, object> dic = new Dictionary<string, object> { { "content", content } };
            Log(logger, LogLevel.Info, message, dic);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public static void WarnByContent(this ILogger logger, string message, object content)
        {
            Dictionary<string, object> dic = new Dictionary<string, object> { { "content", content } };
            Log(logger, LogLevel.Warn, message, dic);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public static void TraceByContent(this ILogger logger, string message, object content)
        {
            Dictionary<string, object> dic = new Dictionary<string, object> { { "content", content } };
            Log(logger, LogLevel.Trace, message, dic);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public static void ErrorByContent(this ILogger logger, Exception ex, string message, object content)
        {
            Dictionary<string, object> dic = new Dictionary<string, object> { { "content", content } };
            Log(logger, LogLevel.Error, message, dic, ex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public static void DebugByContent(this ILogger logger, string message, object content)
        {
            Dictionary<string, object> dic = new Dictionary<string, object> { { "content", content } };
            Log(logger, LogLevel.Debug, message, dic);
        }
    }
}