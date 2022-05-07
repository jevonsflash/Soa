using System;
using System.Linq;
using System.Threading.Tasks;
using Soa.Protocols.Service;

namespace Soa.Client.LoadBalance
{
    public abstract class AddressSelectorBase : IAddressSelector
    {
        Task<SoaAddress> IAddressSelector.GetAddressAsyn(SoaServiceRoute serviceRoute)
        {
            if (serviceRoute == null)
                throw new ArgumentNullException(nameof(serviceRoute));
            if (!serviceRoute.Address.Any(x => x.IsHealth))
                throw new ArgumentException($"{serviceRoute.ServiceDescriptor.Id},not available server");
            return GetAddressAsync(serviceRoute);
        }

        public abstract Task<SoaAddress> GetAddressAsync(SoaServiceRoute serviceRoute);
    }
}