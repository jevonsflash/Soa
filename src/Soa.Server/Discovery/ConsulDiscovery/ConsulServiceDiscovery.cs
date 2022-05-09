using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Consul;
using Soa.Protocols.Service;
using Soa.Protocols.ServiceDesc;
using Soa.Serializer;
using Soa.Server.Discovery;

namespace Soa.Server.ConsulDiscovery
{
    public class ConsulServiceDiscovery : IServiceDiscovery, IDisposable
    {
        private readonly ConsulClient _consul;
        private readonly ISerializer _serializer;
        private readonly string _serviceCategory;
        private readonly string _serverAddress;

        private List<SoaServiceRoute> _routes;
        public ConsulServiceDiscovery(string ip, int port, string serviceCategory, string serverAddress, ISerializer serializer)
        {
            _routes = new List<SoaServiceRoute>();
            _serializer = serializer;
            _serviceCategory = serviceCategory;
            _serverAddress = serverAddress;
            _consul = new ConsulClient(config =>
            {
                config.Address = new Uri($"http://{ip}:{port}");
            });
        }

        public void Dispose()
        {
            _consul.Dispose();
        }

        public async Task<List<SoaServiceRoute>> GetRoutesAsync()
        {
            if (_routes != null && _routes.Any())
            {
                return _routes.ToList();
            }
            var data = (await _consul.KV.Get(GetKey())).Response?.Value;
            if (data == null)
            {
                return _routes;
            }

            var descriptors = _serializer.Deserialize<byte[], List<SoaServiceRouteDesc>>(data);
            if (descriptors != null && descriptors.Any())
            {
                foreach (var descriptor in descriptors)
                {
                    List<SoaAddress> addresses = new List<SoaAddress>(descriptor.AddressDescriptors.ToArray().Count());
                    foreach (var addDesc in descriptor.AddressDescriptors)
                    {
                        var addrType = Type.GetType(addDesc.Type);
                        addresses.Add(_serializer.Deserialize(addDesc.Value, addrType) as SoaAddress);
                    }

                    _routes.Add(new SoaServiceRoute
                    {
                        Address = addresses,
                        ServiceDescriptor = descriptor.ServiceDescriptor
                    });
                }
            }

            return _routes;
        }

        public async Task<List<SoaAddress>> GetAddressAsync()
        {
            var routes = await GetRoutesAsync();
            var addresses = new List<SoaAddress>();
            if (routes != null && routes.Any())
            {
                addresses = routes.SelectMany(x => x.Address).Distinct().ToList();
            }
            return addresses;
        }

        public async Task ClearAsync()
        {
            await _consul.KV.Delete(GetKey());
            //var queryResult = await _consul.KV.List(_serviceCategory);
            //var response = queryResult.Response;
            //if (response != null)
            //{
            //    foreach (var result in response)
            //    {
            //        await _consul.KV.DeleteCAS(result);
            //    }
            //}
        }

        public async Task SetRoutesAsync(IEnumerable<SoaServiceRoute> routes)
        {
            //var existingRoutes = await GetRoutes(routes.Select(x => GetKey(x.ServiceDescriptor.Id)));
            _routes = routes.ToList();
            var routeDescriptors = new List<SoaServiceRouteDesc>(routes.Count());
            foreach (var route in routes)
            {
                routeDescriptors.Add(new SoaServiceRouteDesc
                {
                    ServiceDescriptor = route.ServiceDescriptor,
                    AddressDescriptors = route.Address?.Select(x => new SoaAddressDesc
                    {
                        Type = $"{x.GetType().FullName}, {x.GetType().Assembly.GetName()}",
                        Value = _serializer.Serialize<string>(x)
                    })
                });
            }

            //await SetRoutesAsync(routeDescriptors);
            var nodeData = _serializer.Serialize<byte[]>(routeDescriptors);
            var keyValuePair = new KVPair(GetKey()) { Value = nodeData };
            await _consul.KV.Put(keyValuePair);
        }

        public async Task AddRouteAsync(List<SoaServiceRoute> routes)
        {
            var curRoutes = await GetRoutesAsync();
            foreach (var route in routes)
            {
                curRoutes.RemoveAll(x => x.ServiceDescriptor.Id == route.ServiceDescriptor.Id);
                curRoutes.Add(route);
            }
            await SetRoutesAsync(curRoutes);
        }

        private string GetKey()
        {
            return $"{_serviceCategory}-{_serverAddress}";
        }

        private async Task<SoaServiceRoute> GetRoute(string path)
        {
            var queryResult = await _consul.KV.Keys(path);
            if (queryResult.Response == null)
            {
                return null;
            }
            var data = (await _consul.KV.Get(path)).Response?.Value;
            if (data == null)
            {
                return null;
            }
            var descriptor = _serializer.Deserialize<byte[], SoaServiceRouteDesc>(data);

            List<SoaAddress> addresses = new List<SoaAddress>(descriptor.AddressDescriptors.ToArray().Count());
            foreach (var addDesc in descriptor.AddressDescriptors)
            {
                var addrType = Type.GetType(addDesc.Type);
                addresses.Add(_serializer.Deserialize(addDesc.Value, addrType) as SoaAddress);
            }

            return new SoaServiceRoute
            {
                Address = addresses,
                ServiceDescriptor = descriptor.ServiceDescriptor
            };
        }

        ///// <summary>
        ///// clean specify address service
        ///// </summary>
        ///// <param name="address"></param>
        ///// <returns></returns>
        //public async Task ClearAsync(string address)
        //{
        //    await _consul.KV.Delete(GetKey());
        //    //var routes = await GetRoutesAsync();
        //    //var remoteRouteIds = (from route in routes
        //    //                      where route.Address.Count() == 1 && (route.Address.Any(x => x == null || route.Address.First().Code == address))
        //    //                      select route.ServiceDescriptor.Id).ToArray();
        //    //foreach (var remoteRouteId in remoteRouteIds)
        //    //{
        //    //    await _consul.KV.Delete(GetKey(remoteRouteId));
        //    //}
        //}

        public async Task ClearServiceAsync(string serviceId)
        {
            var routes = await GetRoutesAsync();
            var hasRemove = routes.RemoveAll(x => x.ServiceDescriptor.Id == serviceId);
            if (hasRemove > 0)
                await SetRoutesAsync(routes);
        }
    }
}
