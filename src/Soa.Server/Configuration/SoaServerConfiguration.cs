namespace Soa.Server.Configuration
{
    public class SoaServerConfiguration : ISoaServerConfiguration
    {
        public string Transport { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public string[] AssemblyNames { get; set; }

    }
}