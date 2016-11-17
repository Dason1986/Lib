using System;
using System.Net;
using System.Net.Sockets;

namespace NLog.Revicer.Listeners
{
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
                    catch (Exception ex)
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
}