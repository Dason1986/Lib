using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Library.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class DnsProxy : IDisposable
    {
        private const int DNS_PORT = 53;
        private const int BUFFER_SIZE = 4096;
        private const int RETRY_TIMES = 3;

        private readonly IPAddress[] _dnsServers;
        private readonly AddressFamily _addressFamily;
        private readonly int _listenerCount;
        private readonly bool _forceTcp;
        private readonly object _aysncRoot = new object();

        private Socket _udpSocket = null;
        private Socket _tcpSocket = null;
        private Socket _udpQuerySocket = null;
        private Socket _tcpQuerySocket = null;
        private int _serverIndex = 0;

        static DnsProxy()
        {
            DefaultV4 = new DnsProxy(new[] {
                    IPAddress.Parse("8.8.4.4"),         //google
                    IPAddress.Parse("208.67.220.220"),  //opendns
                    IPAddress.Parse("8.8.8.8"),         //google
                    IPAddress.Parse("208.67.222.222"),  //opendns
                }, AddressFamily.InterNetwork, 10, true);
            DefaultV6 = new DnsProxy(new[] {
                    IPAddress.Parse("2001:4860:4860::8844"),//google
                    IPAddress.Parse("2620:0:ccd::2"),       //opendns
                    IPAddress.Parse("2001:4860:4860::8888"),//google
                    IPAddress.Parse("2620:0:ccc::2"),       //opendns
                }, AddressFamily.InterNetworkV6, 10, true);
        }
        /// <summary>
        /// 
        /// </summary>
        public static DnsProxy DefaultV4 { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public static DnsProxy DefaultV6 { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dnsServers"></param>
        /// <param name="addressFamily"></param>
        /// <param name="listenerCount"></param>
        /// <param name="forceTcp"></param>
        public DnsProxy(IPAddress[] dnsServers, AddressFamily addressFamily, int listenerCount, bool forceTcp = false)
        {
            if (dnsServers == null)
                throw new ArgumentNullException("dnsServers");
            if (dnsServers.Length == 0)
                throw new ArgumentException("at least need one server address");
            if (dnsServers.Any(s => s.AddressFamily != addressFamily))
                throw new ArgumentException("some dns servers address not belong to specified address family");

            _dnsServers = dnsServers;
            _addressFamily = addressFamily;
            _listenerCount = listenerCount;
            _forceTcp = forceTcp;

            if (!Socket.OSSupportsIPv4 && addressFamily == AddressFamily.InterNetwork)
                throw new NotSupportedException("OS not supports IPv4 address family");
            if (!Socket.OSSupportsIPv6 && addressFamily == AddressFamily.InterNetworkV6)
                throw new NotSupportedException("OS not supports IPv6 address family");

            _udpSocket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
            _tcpSocket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            _udpQuerySocket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
            _tcpQuerySocket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            EndPoint ep = new IPEndPoint(_addressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, DNS_PORT);
            _udpSocket.Bind(ep);
            for (int i = 0; i < _listenerCount; i++)
            {
                AsyncState state = new AsyncState
                {
                    Buffer = new byte[BUFFER_SIZE],
                    EndPoint = new IPEndPoint(_addressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, DNS_PORT),
                };
                StartUdpListen(state);
            }
            _tcpSocket.Bind(ep);
            _tcpSocket.Listen(_listenerCount);
            for (int i = 0; i < _listenerCount; i++)
            {
                StartTcpListen();
            }
        }

        private void TcpAccept_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                byte[] buf = new byte[BUFFER_SIZE];
                int size = e.AcceptSocket.Receive(buf);

                buf = TcpQuery(buf.Take(size).ToArray());

                e.AcceptSocket.Send(buf);
                e.AcceptSocket.Disconnect(false);
                e.AcceptSocket.Dispose();
            }
            catch { }
            StartTcpListen();
        }

        private void UdpAsyncCallback(IAsyncResult ar)
        {
            var state = ar.AsyncState as AsyncState;
            try
            {
                int size = _udpSocket.EndReceiveFrom(ar, ref state.EndPoint);
                byte[] buf = state.Buffer;

                IEnumerable<byte> data = BitConverter.GetBytes((short)size);
                if (BitConverter.IsLittleEndian)
                    data = data.Reverse();

                buf = _forceTcp
                    ? TcpQuery(data.Concat(buf.Take(size)).ToArray()).Skip(2).ToArray()
                    : UdpQuery(buf.Take(size).ToArray());

                _udpSocket.SendTo(buf, state.EndPoint);
                state.EndPoint = new IPEndPoint(_addressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, DNS_PORT);
            }
            catch { }
            StartUdpListen(state);
        }

        private byte[] UdpQuery(byte[] message)
        {
            EndPoint ep = CreateServerEndPoint();
            byte[] buf = new byte[BUFFER_SIZE];
            int size = -1;
            int retry = 0;
            try
            {
                lock (_aysncRoot)
                    do
                    {
                        _udpQuerySocket.SendTo(message, ep);
                        size = _udpQuerySocket.ReceiveFrom(buf, ref ep);
                    } while (size == 0 && retry++ < RETRY_TIMES);
            }
            catch
            {
                _serverIndex = (_serverIndex + 1) % _dnsServers.Length;
            }
            return buf.Take(size).ToArray();
        }

        private byte[] TcpQuery(byte[] message)
        {
            EndPoint ep = CreateServerEndPoint();
            byte[] buf = new byte[BUFFER_SIZE];
            int size = -1;
            int retry = 0;
            try
            {
                lock (_aysncRoot)
                    do
                    {
                        if (size == 0 || !_tcpQuerySocket.Connected && _tcpQuerySocket.IsBound)
                        {
                            _tcpQuerySocket.Dispose();
                            _tcpQuerySocket = new Socket(_addressFamily, SocketType.Stream, ProtocolType.Tcp);
                        }
                        if (!_tcpQuerySocket.Connected)
                            _tcpQuerySocket.Connect(ep);
                        _tcpQuerySocket.Send(message);
                        size = _tcpQuerySocket.Receive(buf);
                    } while (size == 0 && retry++ < RETRY_TIMES);
            }
            catch
            {
                _serverIndex = (_serverIndex + 1) % _dnsServers.Length;
            }
            return buf.Take(size).ToArray();
        }

        private EndPoint CreateServerEndPoint()
        {
            return new IPEndPoint(_dnsServers[_serverIndex], DNS_PORT);
        }

        private SocketAsyncEventArgs CreateSocketAsyncEventArgs()
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(TcpAccept_Completed);
            return args;
        }

        private void StartUdpListen(AsyncState state)
        {
            try
            {
                _udpSocket.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ref state.EndPoint, UdpAsyncCallback, state);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch
            {
                StartUdpListen(state);
            }
        }

        private void StartTcpListen()
        {
            try
            {
                _tcpSocket.AcceptAsync(CreateSocketAsyncEventArgs());
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch
            {
                StartTcpListen();
            }
        }

        public void Stop()
        {
            _udpSocket.Shutdown(SocketShutdown.Both);
            _tcpSocket.Shutdown(SocketShutdown.Both);
        }

        #region IDisposable.Dispose
        void IDisposable.Dispose()
        {
            _udpSocket.Dispose();
            _tcpSocket.Dispose();
            _udpQuerySocket.Dispose();
            _tcpQuerySocket.Dispose();
        }
        #endregion

        private class AsyncState
        {
            public byte[] Buffer;
            public EndPoint EndPoint;
        }
    }

    /*
     
         DnsProxy.DefaultV4.Start();
         DnsProxy.DefaultV6.Start();
         new ManualResetEvent(false).WaitOne();
     
     
     */
}
