using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Soa.Client.Discovery;
using Soa.Protocols.Service;
using Soa.Protocols.ServiceDesc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soa.Client.Gateway
{
    [Route("api/[controller]/[action]")]
    public class ServicesManagerController:AbpController
    {
        private readonly IClientServiceDiscovery clientServiceDiscovery;

        public ServicesManagerController(IClientServiceDiscovery clientServiceDiscovery)
        {
            this.clientServiceDiscovery = clientServiceDiscovery;
        }
      
        [HttpGet]
        public async Task<List<SoaAddress>> GetAddresses()
        {
            var addresses = await clientServiceDiscovery.GetAddressAsync();
            return addresses;

        }


        [HttpGet]
        public async Task<List<SoaServiceDesc>> GetServices(string server)
        {
            var routes = await clientServiceDiscovery.GetRoutesAsync();
            if (routes != null && routes.Any() && !string.IsNullOrEmpty(server))
            {
                return (from route in routes
                        where route.Address.Any(x => x.Code == server)
                        select route.ServiceDescriptor).ToList();
            }
            return (from route in routes select route.ServiceDescriptor).ToList();

        }

    }
}
