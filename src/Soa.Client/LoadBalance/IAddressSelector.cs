using System.Threading.Tasks;
using Abp.Dependency;
using Soa.Protocols.Service;

namespace Soa.Client.LoadBalance
{
    /// <summary>
    ///     server selector
    /// </summary>
    public interface IAddressSelector
    {
        /// <summary>
        ///     get server for specify route
        /// </summary>
        /// <param name="serviceRoute"></param>
        /// <returns></returns>
        Task<SoaAddress> GetAddressAsyn(SoaServiceRoute serviceRoute);
    }
}