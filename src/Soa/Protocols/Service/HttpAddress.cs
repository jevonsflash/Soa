namespace Soa.Protocols.Service
{
    public class HttpAddress : SoaAddress
    {
        public HttpAddress() : base("Http")
        {
        }

        public HttpAddress(string ip, int port) : base("Http")
        {
            Ip = ip;
            Port = port;
        }
    }
}
