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

        #endregion IDisposable.Dispose

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

    public class Ping
    {
        private const int SOCKET_ERROR = -1;
        private const int ICMP_ECHO = 8;

        //{
        //    ping p = new ping();
        //    Console.WriteLine("请输入要 Ping 的IP或者主机名字：");
        //    string MyUrl = Console.ReadLine();
        //    Console.WriteLine("正在 Ping " + MyUrl + " ……");
        //    Console.Write(p.PingHost(MyUrl));
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public string PingHost(string host)
        {
            // 声明 IPHostEntry
            IPHostEntry ServerHE, fromHE;
            int nBytes = 0;
            int dwStart = 0, dwStop = 0;

            //初始化ICMP的Socket
            Socket socket =
             new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
            // 得到Server EndPoint
            try
            {
                ServerHE = Dns.GetHostEntry(host);
            }
            catch (Exception)
            {
                return "没有发现主机";
            }

            // 把 Server IP_EndPoint转换成EndPoint
            IPEndPoint ipepServer = new IPEndPoint(ServerHE.AddressList[0], 0);
            EndPoint epServer = (ipepServer);

            // 设定客户机的接收Endpoint
            fromHE = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint ipEndPointFrom = new IPEndPoint(fromHE.AddressList[0], 0);
            EndPoint EndPointFrom = (ipEndPointFrom);

            int PacketSize = 0;
            IcmpPacket packet = new IcmpPacket();

            // 构建要发送的包
            packet.Type = ICMP_ECHO; //8
            packet.SubCode = 0;
            packet.CheckSum = 0;
            packet.Identifier = 45;
            packet.SequenceNumber = 0;
            int PingData = 24; // sizeof(IcmpPacket) - 8;
            packet.Data = new Byte[PingData];

            // 初始化Packet.Data
            for (int i = 0; i < PingData; i++)
            {
                packet.Data[i] = (byte)'#';
            }

            //Variable to hold the total Packet size
            PacketSize = 32;
            Byte[] icmp_pkt_buffer = new Byte[PacketSize];
            Int32 Index = 0;
            //again check the packet size
            Index = Serialize(
             packet,
             icmp_pkt_buffer,
             PacketSize,
             PingData);
            //if there is a error report it
            if (Index == -1)
            {
                return "Error Creating Packet";
            }
            // convert into a UInt16 array

            //Get the Half size of the Packet
            Double double_length = Convert.ToDouble(Index);
            Double dtemp = Math.Ceiling(double_length / 2);
            int cksum_buffer_length = Index / 2;
            //Create a Byte Array
            UInt16[] cksum_buffer = new UInt16[cksum_buffer_length];
            //Code to initialize the Uint16 array
            int icmp_header_buffer_index = 0;
            for (int i = 0; i < cksum_buffer_length; i++)
            {
                cksum_buffer[i] =
                 BitConverter.ToUInt16(icmp_pkt_buffer, icmp_header_buffer_index);
                icmp_header_buffer_index += 2;
            }
            //Call a method which will return a checksum
            UInt16 u_cksum = checksum(cksum_buffer, cksum_buffer_length);
            //Save the checksum to the Packet
            packet.CheckSum = u_cksum;

            // Now that we have the checksum, serialize the packet again
            Byte[] sendbuf = new Byte[PacketSize];
            //again check the packet size
            Index = Serialize(
             packet,
             sendbuf,
             PacketSize,
             PingData);
            //if there is a error report it
            if (Index == -1)
            {
                return "Error Creating Packet";
            }

            dwStart = System.Environment.TickCount; // Start timing
            //send the Packet over the socket
            if ((nBytes = socket.SendTo(sendbuf, PacketSize, 0, epServer)) == SOCKET_ERROR)
            {
                return "Socket Error: cannot send Packet";
            }
            // Initialize the buffers. The receive buffer is the size of the
            // ICMP header plus the IP header (20 bytes)
            Byte[] ReceiveBuffer = new Byte[256];
            nBytes = 0;
            //Receive the bytes
            bool recd = false;
            int timeout = 0;

            //loop for checking the time of the server responding
            while (!recd)
            {
                nBytes = socket.ReceiveFrom(ReceiveBuffer, 256, 0, ref EndPointFrom);
                if (nBytes == SOCKET_ERROR)
                {
                    return "主机没有响应";
                }
                else if (nBytes > 0)
                {
                    dwStop = System.Environment.TickCount - dwStart; // stop timing
                    return "Reply from " + epServer.ToString() + " in "
                    + dwStop + "ms.  Received: " + nBytes + " Bytes.";
                }
                timeout = System.Environment.TickCount - dwStart;
                if (timeout > 1000)
                {
                    return "超时";
                }
            }

            //close the socket
            socket.Close();
            return "";
        }

        /// <summary>
        ///  This method get the Packet and calculates the total size
        ///  of the Pack by converting it to byte array
        /// </summary>
        private Int32 Serialize(IcmpPacket packet, Byte[] Buffer,
          Int32 PacketSize, Int32 PingData)
        {
            Int32 cbReturn = 0;
            // serialize the struct into the array
            int Index = 0;

            Byte[] b_type = new Byte[1];
            b_type[0] = (packet.Type);

            Byte[] b_code = new Byte[1];
            b_code[0] = (packet.SubCode);

            Byte[] b_cksum = BitConverter.GetBytes(packet.CheckSum);
            Byte[] b_id = BitConverter.GetBytes(packet.Identifier);
            Byte[] b_seq = BitConverter.GetBytes(packet.SequenceNumber);

            Array.Copy(b_type, 0, Buffer, Index, b_type.Length);
            Index += b_type.Length;

            Array.Copy(b_code, 0, Buffer, Index, b_code.Length);
            Index += b_code.Length;

            Array.Copy(b_cksum, 0, Buffer, Index, b_cksum.Length);
            Index += b_cksum.Length;

            Array.Copy(b_id, 0, Buffer, Index, b_id.Length);
            Index += b_id.Length;

            Array.Copy(b_seq, 0, Buffer, Index, b_seq.Length);
            Index += b_seq.Length;

            // copy the data
            Array.Copy(packet.Data, 0, Buffer, Index, PingData);
            Index += PingData;
            if (Index != PacketSize/* sizeof(IcmpPacket)  */)
            {
                cbReturn = -1;
                return cbReturn;
            }

            cbReturn = Index;
            return cbReturn;
        }

        /// <summary>
        ///  This Method has the algorithm to make a checksum
        /// </summary>
        private UInt16 checksum(UInt16[] buffer, int size)
        {
            Int32 cksum = 0;
            int counter;
            counter = 0;

            while (size > 0)
            {
                UInt16 val = buffer[counter];

                cksum += buffer[counter];
                counter += 1;
                size -= 1;
            }

            cksum = (cksum >> 16) + (cksum & 0xffff);
            cksum += (cksum >> 16);
            return (UInt16)(~cksum);
        }
    }

    /// 类结束
    /// <summary>
    ///  Class that holds the Pack information
    /// </summary>
    public class IcmpPacket
    {
        public Byte Type;    // type of message
        public Byte SubCode;    // type of sub code
        public UInt16 CheckSum;   // ones complement checksum of struct
        public UInt16 Identifier;      // identifier
        public UInt16 SequenceNumber;     // sequence number
        public Byte[] Data;
    } // class IcmpPacket
}