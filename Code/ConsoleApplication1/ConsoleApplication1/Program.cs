using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            UDP();
           // HttpWeb(args);

            Console.Read();
        }
        private static void UDP()
        {
            try
            {
                var ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4001);
                System.Net.Sockets.Socket newsock = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //  socket.Connect();
                newsock.Bind(ip);
                int recv;
                byte[] data = new byte[9000];
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint Remote = (EndPoint)(sender);
              

                //发送信息
              
                while (true)
                {
                    data = new byte[1024];
                    //发送接受信息
                    recv = newsock.ReceiveFrom(data, ref Remote);
                    ShowLog(data);
                  
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.ToString());
                Console.ReadLine();
            }
        }
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

        private static void ShowLog(byte[] buff)
        {
            var log = System.Text.UTF8Encoding.UTF8.GetString(buff).Trim().Replace("\0",string.Empty);
            if (log.Contains("|ERROR|"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (log.Contains("|WARN|"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (Console.ForegroundColor != ConsoleColor.Gray)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine(log);
        }
    }

}
