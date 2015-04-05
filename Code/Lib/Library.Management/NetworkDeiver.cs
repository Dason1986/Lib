namespace Library.Management
{
    public class NetworkDeiver
    {
        public string MacAddress { get; protected internal set; }
        public string Description { get; protected internal set; }
        public bool DHCPEnabled { get; protected internal set; }
        public string DHCPServer { get; protected internal set; }
        public string ServiceName { get; protected internal set; }
        public string Caption { get; protected internal set; }
        public string[] IPAddress { get; protected internal set; }
        public string[] DefaultIPGateway { get; protected internal set; }
    }
}