using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Soa.Protocols.Service;

namespace Soa.Client.Discovery
{
    public class ClientServiceDiscovery : IClientServiceDiscovery, ISingletonDependency
    {
        private readonly List<Func<Task<List<SoaServiceRoute>>>> _routesGetters;
        private readonly ConcurrentQueue<SoaAddress> _addresses;
        private readonly ConcurrentQueue<SoaServiceRoute> _routes;

        public ClientServiceDiscovery()
        {
            _routesGetters = new List<Func<Task<List<SoaServiceRoute>>>>();
            _addresses = new ConcurrentQueue<SoaAddress>();
            _routes = new ConcurrentQueue<SoaServiceRoute>();
        }


        private void ClearQueue<T>(ConcurrentQueue<T> queue)
        {
            foreach (var q in queue)
            {
                queue.TryDequeue(out var tmpQ);
            }
        }

        private void QueueAdd<T>(ConcurrentQueue<T> queue, List<T> elems)
        {
            foreach (var elem in elems)
            {
                if (!queue.Any(x => x.Equals(elem)))
                    queue.Enqueue(elem);
            }
        }


        public void AddRoutesGetter(Func<Task<List<SoaServiceRoute>>> getter)
        {
            _routesGetters.Add(getter);
        }

        public Task<List<SoaAddress>> GetAddressAsync()
        {
            //if (_addresses.Any())
            //    return _addresses.ToList();
            //await UpdateRoutes();
            return Task.FromResult(_addresses.ToList());
        }

        public Task<List<SoaServiceRoute>> GetRoutesAsync()
        {
            return Task.FromResult(_routes.ToList());
        }

        public Task UpdateServerHealthAsync(List<SoaAddress> addresses)
        {
            foreach (var addr in addresses)
            {
                _routes.Where(x => x.Address.Any(y => y.Code == addr.Code)).ToList()
                    .ForEach(x => x.Address.First(a => a.Code == addr.Code).IsHealth = addr.IsHealth);
                var address = _addresses.FirstOrDefault(x => x.Code == addr.Code);
                if (address != null)
                {
                    address.IsHealth = addr.IsHealth;
                }
            }

            return Task.CompletedTask;
        }
        public async Task UpdateRoutes()
        {
            var routes = new List<SoaServiceRoute>();
            foreach (var routesGetter in _routesGetters)
            {
                var list = await routesGetter();
                if (list != null && list.Any())
                    routes.AddRange(list);
            }
            // merge service and its address by service id
            ClearQueue(_routes);
            foreach (var route in routes)
            {
                var oldAddr = _addresses.FirstOrDefault(x => route.Address.Any(y => y.Code == x.Code && y.IsHealth != x.IsHealth));
                if (oldAddr != null)
                {
                    route.Address.Where(x => x.Code == oldAddr.Code).ToList().ForEach(x => x.IsHealth = oldAddr.IsHealth);
                }
                if (_routes.Any(x => x.ServiceDescriptor.Id == route.ServiceDescriptor.Id))
                {
                    var existService = _routes.First(x => x.ServiceDescriptor.Id == route.ServiceDescriptor.Id);
                    existService.Address = existService.Address.Concat(route.Address.Except(existService.Address)).ToList();
                }
                else
                {
                    _routes.Enqueue(route);
                }
            }
            // update local server address from newest routes
            ClearQueue(_addresses);
            QueueAdd(_addresses, _routes.SelectMany(x => x.Address).Distinct().ToList());
        }

    }
}
