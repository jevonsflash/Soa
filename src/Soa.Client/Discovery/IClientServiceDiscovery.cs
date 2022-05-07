using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Soa.Protocols.Service;

namespace Soa.Client.Discovery
{
    public interface IClientServiceDiscovery
    {
        void AddRoutesGetter(Func<Task<List<SoaServiceRoute>>> getter);
        /// <summary>
        ///     get all registered service routes
        /// </summary>
        /// <returns></returns>
        Task<List<SoaServiceRoute>> GetRoutesAsync();

        /// <summary>
        ///     get all registered service's server
        /// </summary>
        /// <returns></returns>
        Task<List<SoaAddress>> GetAddressAsync();

        Task UpdateServerHealthAsync(List<SoaAddress> addresses);
        Task UpdateRoutes();
    }
}
