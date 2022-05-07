using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Logging;
using Soa.Protocols.Service;

namespace Soa.Client.LoadBalance
{
    /// <summary>
    ///     load balancing by polling algorithm
    /// </summary>
    public class PollingAddressSelector : AddressSelectorBase, ISingletonDependency
    {
        private readonly ConcurrentDictionary<string, Lazy<ServerIndexHolder>> _addresses =
            new ConcurrentDictionary<string, Lazy<ServerIndexHolder>>();


        public PollingAddressSelector()
        {
        }

        public override Task<SoaAddress> GetAddressAsync(SoaServiceRoute serviceRoute)
        {
            var serverIndexHolder = _addresses.GetOrAdd(serviceRoute.ServiceDescriptor.Id,
                key => new Lazy<ServerIndexHolder>(() => new ServerIndexHolder()));
            var address = serverIndexHolder.Value.GetAddress(serviceRoute.Address.Where(x => x.IsHealth).ToList());
            LogHelper.Logger.Debug($"{serviceRoute.ServiceDescriptor.Id}, request address: {serviceRoute.ServiceDescriptor.Id}: {address.Code}");
            return Task.FromResult(address);
        }

        private class ServerIndexHolder
        {
            private int _latestIndex;
            private int _lock;

            public SoaAddress GetAddress(List<SoaAddress> addresses)
            {
                while (true)
                {
                    if (Interlocked.Exchange(ref _lock, 1) != 0)
                    {
                        default(SpinWait).SpinOnce();
                        continue;
                    }

                    _latestIndex = (_latestIndex + 1) % addresses.Count;
                    var address = addresses[_latestIndex];
                    Interlocked.Exchange(ref _lock, 0);
                    return address;
                }
            }
        }
    }
}