using Soa.Client.Configuration.JobConfiguration;
using Soa.Protocols.Service;

namespace Soa.Client.Configuration
{
    public interface ISoaClientConfiguration
    {
        public string Transport { get; set; }
        public string Token { get; set; }
        public HttpAddress[] Address { get; set; }
        public string[] AssemblyNames { get; set; }
        public HealthCheckConfiguration HealthCheck { get; set; }
        public ServiceDiscoveryConfiguration ServiceDiscovery { get; set; }
    }
}