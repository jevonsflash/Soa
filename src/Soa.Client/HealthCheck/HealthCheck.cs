using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Abp.Dependency;
using Soa.Client.Discovery;

namespace Soa.Client.HealthCheck
{
    public class HealthCheck : IHealthCheck, ISingletonDependency
    {
        private readonly IClientServiceDiscovery serviceDiscovery;

        public HealthCheck(IClientServiceDiscovery serviceDiscovery)
        {
            this.serviceDiscovery = serviceDiscovery;
        }

        public async Task CheckHealth(int timeout)
        {
            if (serviceDiscovery != null)
            {
                var routes = (await serviceDiscovery.GetRoutesAsync()).ToList();
                var servers = (from route in routes
                               from address in route.Address
                                   //where address.IsHealth
                               select address).Distinct().ToList();
                foreach (var server in servers)
                {
                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { SendTimeout = timeout })
                    {
                        try
                        {
                            await socket.ConnectAsync(server.CreateEndPoint());
                            server.IsHealth = true;
                        }
                        catch
                        {
                            server.IsHealth = false;
                        }
                    }
                }
                await serviceDiscovery.UpdateServerHealthAsync(servers);
            }     
        }
    }

}
