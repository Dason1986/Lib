using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;
using Library.Annotations;

namespace Library.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public class RichTextBoxTraceListener : TraceListener
    {
        private readonly RichTextBox _target;
        private readonly StringSendDelegate _invokeWrite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="name"></param>
        public RichTextBoxTraceListener([NotNull] RichTextBox target, string name = null)
            : base(name)
        {
            if (target == null) throw new ArgumentNullException("target");
            _target = target;
            _target.ReadOnly = true;
            _target.Disposed += _target_Disposed;
            _invokeWrite = new StringSendDelegate(SendString);
        }

        void _target_Disposed(object sender, EventArgs e)
        {
            if (_target != null) _target.Disposed -= _target_Disposed;
            Trace.Listeners.Remove(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            try
            {
                _target.Invoke(_invokeWrite, new object[] { message, false });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            try
            {
                _target.Invoke(_invokeWrite, new object[] { message, true });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        private delegate void StringSendDelegate(string message, bool isnewline);
        private void SendString(string message, bool isnewline)
        {
            // No need to lock text box as this function will only 
            // ever be executed from the UI thread

            _target.AppendText(message);
            if (color != Color.Black)
            {
                _target.Select(_target.TextLength - message.Length, message.Length);
                var old = _target.SelectionColor;
                _target.SelectionColor = color;
               // _target.SelectionBackColor = Color.Silver;
                _target.Select(_target.TextLength, 0);
                //_target.SelectionBackColor = Color.White;
                _target.SelectionColor = old;
            }
            if (isnewline)
                _target.AppendText(Environment.NewLine);

        }

        /// <summary>
        /// 
        /// </summary>
        public void BindTrace()
        {
            Trace.Listeners.Add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        [ComVisible(false)]
        public override void TraceData(TraceEventCache eventCache, String source, TraceEventType eventType, int id, object data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;

            WriteHeader(source, eventType, id);
            string datastring = String.Empty;
            if (data != null)
                datastring = data.ToString();

            WriteLine(datastring);
            WriteFooter(eventCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        [ComVisible(false)]
        public override void TraceData(TraceEventCache eventCache, String source, TraceEventType eventType, int id, params object[] data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
                return;

            WriteHeader(source, eventType, id);

            StringBuilder sb = new StringBuilder();
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    if (data[i] != null)
                        sb.Append(data[i].ToString());
                }
            }
            WriteLine(sb.ToString());

            WriteFooter(eventCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        [ComVisible(false)]
        public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        [ComVisible(false)]
        public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            WriteHeader(source, eventType, id);
            WriteLine(message);

            WriteFooter(eventCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [ComVisible(false)]
        public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                return;

            WriteHeader(source, eventType, id);
            if (args != null)
                WriteLine(String.Format(CultureInfo.InvariantCulture, format, args));
            else
                WriteLine(format);

            WriteFooter(eventCache);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="relatedActivityId"></param>
        [ComVisible(false)]
        public override void TraceTransfer(TraceEventCache eventCache, String source, int id, string message, Guid relatedActivityId)
        {
            TraceEvent(eventCache, source, TraceEventType.Transfer, id, message + ", relatedActivityId=" + relatedActivityId.ToString());
        }
        Color color = Color.Black;
        private void WriteHeader(String source, TraceEventType eventType, int id)
        {

            switch (eventType)
            {
                case TraceEventType.Critical: color = Color.Maroon; break;
                case TraceEventType.Stop:
                case TraceEventType.Error: color = Color.Red; break;
                case TraceEventType.Resume: break;
                case TraceEventType.Start: color = Color.Green; break;
                case TraceEventType.Transfer: color = Color.MediumTurquoise; break;
                case TraceEventType.Suspend:
                case TraceEventType.Warning: color = Color.GreenYellow; break;
            }



            var str = String.Format(CultureInfo.InvariantCulture, "{0} {1}: {2} : ", source, eventType.ToString(),
                id.ToString(CultureInfo.InvariantCulture));
            Write(str);
            color = Color.Black;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCache"></param>
        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
        private void WriteFooter(TraceEventCache eventCache)
        {
            if (eventCache == null)
                return;

            IndentLevel++;
            if (IsEnabled(TraceOptions.ProcessId))
                WriteLine("ProcessId=" + eventCache.ProcessId);

            if (IsEnabled(TraceOptions.LogicalOperationStack))
            {
                Write("LogicalOperationStack=");
                Stack operationStack = eventCache.LogicalOperationStack;
                bool first = true;
                foreach (Object obj in operationStack)
                {
                    if (!first)
                        Write(", ");
                    else
                        first = false;

                    Write(obj.ToString());
                }
                WriteLine(String.Empty);
            }

            if (IsEnabled(TraceOptions.ThreadId))
                WriteLine("ThreadId=" + eventCache.ThreadId);

            if (IsEnabled(TraceOptions.DateTime))
                WriteLine("DateTime=" + eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));

            if (IsEnabled(TraceOptions.Timestamp))
                WriteLine("Timestamp=" + eventCache.Timestamp);

            if (IsEnabled(TraceOptions.Callstack))
                WriteLine("Callstack=" + eventCache.Callstack);
            IndentLevel--;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        internal bool IsEnabled(TraceOptions opts)
        {
            return (opts & TraceOutputOptions) != 0;
        }
    }
}
