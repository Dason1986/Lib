using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Library.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public class UDPTraceListener : TraceListener
    {
        private readonly Socket utpSocket;

        readonly IPEndPoint iep1;//255.255.255.255
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="name"></param>
        public UDPTraceListener(int port, string name = null)
            : base(name)
        {
            if (port <= 0) throw new ArgumentNullException("port");
            iep1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            utpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            utpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            Trace.Listeners.Add(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (utpSocket != null)
                {
                    utpSocket.Close();
                    utpSocket.Dispose();
                }
                Trace.Listeners.Remove(this);
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            utpSocket.SendTo(data, iep1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message + Environment.NewLine);
            utpSocket.SendTo(data, iep1);

        }

    }
}