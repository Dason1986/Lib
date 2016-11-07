using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NLog.Revicer.Logic
{
    public abstract class LogListener
    {
        public LogListener()
        {
            Config = new LogViewConfig();
            Report = new ReprotLogLogic();
        }
        public void Start()
        {
            IsRunning = true;
            OnInit();
        }

        public void Stop()
        {
            IsRunning = false;
        }
        public LogViewConfig Config { get; protected set; }
        public ReprotLogLogic Report { get; protected set; }
        protected abstract void OnInit();
        public bool IsRunning { get; protected set; }

        public void SetConfig(LogViewConfig config, ReprotLogLogic report)
        {
            Config = config;
            Report = report;

        }
        protected virtual SourceLog GetLog(byte[] buff)
        {
            var log = System.Text.UTF8Encoding.UTF8.GetString(buff).Trim().Replace("\0", string.Empty);
            return SourceLog.NLog(log);
        }
        protected virtual SourceLog GetLog(string source)
        {
            return SourceLog.NLog(source);
        }

        protected virtual void AddLog(SourceLog source)
        {
            Report.AddLog(source);
        }
        protected virtual void ReceiveNewLog(byte[] data)
        {
            var log = GetLog(data);
            Report.AddLog(log);
            OnNewLog(log);
        }
        public event EventHandler<NewLogEventArgs> NewLog;

        protected void OnNewLog(SourceLog log)
        {
            var hanlder = NewLog;
            if (hanlder == null) return;
            hanlder.Invoke(this, new NewLogEventArgs(log));
        }
    }
    public class NewLogEventArgs : EventArgs
    {

        public SourceLog Log { get; private set; }

        public NewLogEventArgs(SourceLog log)
        {
            this.Log = log;
        }
    }
    public class UDPLogListener : LogListener
    {

        protected override void OnInit()
        {
            try
            {
                var ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.Port);
                System.Net.Sockets.Socket newsock = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //  socket.Connect();
                newsock.Bind(ip);
                int recv;
                byte[] data = new byte[9000];
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint Remote = (EndPoint)(sender);


                //发送信息

                while (IsRunning)
                {
                    try
                    {


                        data = new byte[Config.BuffSize];
                        //发送接受信息
                        recv = newsock.ReceiveFrom(data, ref Remote);
                        ReceiveNewLog(data);
                    }
                    catch (Exception)
                    {


                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.ToString());
                Console.ReadLine();
            }
        }


    }
    /*
    public class HTTPLogListener : LogListener
    {
            private static void HttpWeb(string[] arsg)
        {
            try
            {
                System.Net.HttpListener listener = new System.Net.HttpListener();

                if (arsg != null && arsg.Length > 0)
                {
                    foreach (var item in arsg)
                    {
                        listener.Prefixes.Add(item);
                    }

                }
                else
                {
                    listener.Prefixes.Add("http://localhost:4001/");
                }
                listener.Start();
                Console.WriteLine("Http log Start");
                foreach (var item in listener.Prefixes)
                {
                    Console.WriteLine(item);
                }
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    var request = context.Request;
                    var st = request.InputStream;
                    byte[] buff = new byte[request.ContentLength64];
                    st.Read(buff, 0, buff.Length);
                    ShowLog(buff);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.ToString());
                Console.ReadLine();
            }
        }

    }

    public class FileLogListener : LogListener
    {

    }*/
}
