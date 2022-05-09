namespace Soa.Server.Configuration
{
    public interface ISoaServerConfiguration
    {
        public string Name { get; set; }
        public string Transport { get; set; }
        public string Discovery { get; set; }
        public ConsulServiceDiscoveryConfiguration ConsulServiceDiscovery { get; set; }

        public string Ip { get; set; }
        public int Port { get; set; }
        public string[] AssemblyNames { get; set; }

    }
}