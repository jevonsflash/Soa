namespace Soa.Protocols.Service
{
    public class DotNettyAddress : SoaAddress
    {
        public DotNettyAddress() : base("DotNetty")
        {
        }

        public DotNettyAddress(string ip, int port) : base("DotNetty")
        {
            Ip = ip;
            Port = port;

        }
    }
}
