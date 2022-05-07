using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Soa.Protocols.Attributes;
using Soa.Protocols.ServiceDesc;
using Soa.Protocols.Service;
using Soa.Protocols.ServiceDesc;
using Soa.Serializer;

namespace Soa.Server.Discovery
{
    public class InServerServiceDiscovery : IServiceDiscovery, ISingletonDependency
    {
        private static readonly List<SoaServiceRoute> _routes = new List<SoaServiceRoute>();

        private readonly ISerializer _serializer;
        public InServerServiceDiscovery(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public Task ClearAsync()
        {
            _routes.Clear();
            return Task.CompletedTask;
        }

        public Task ClearAsync(string address)
        {
            _routes.Clear();
            return Task.CompletedTask;
        }

        public Task ClearServiceAsync(string serviceId)
        {
            _routes.Remove(_routes.FirstOrDefault(x => x.ServiceDescriptor.Id == serviceId));
            return Task.CompletedTask;
        }

        public Task SetRoutesAsync(IEnumerable<SoaServiceRoute> routes)
        {
            // remote exists route
            _routes.RemoveAll(x => routes.Any(y => y.ServiceDescriptor.Id == x.ServiceDescriptor.Id));
            // add news routes
            _routes.AddRange(routes.ToList());
            return Task.CompletedTask;
        }

        public Task AddRouteAsync(List<SoaServiceRoute> routes)
        {
            foreach (var route in routes)
            {
                _routes.RemoveAll(x => x.ServiceDescriptor.Id == route.ServiceDescriptor.Id);
                _routes.Add(route);
            }
            return Task.CompletedTask;
        }

        public Task<List<SoaServiceRoute>> GetRoutesAsync()
        {
            return Task.FromResult(_routes);
        }

        [SoaService(Id = "Soa.ServiceDiscovery.InServer.GetRoutesDescAsync", CreatedBy = "linxiao", CreatedDate = "2018-06-01", Comment = "get all service routes in this server")]
        public Task<List<SoaServiceRouteDesc>> GetRoutesDescAsync()
        {
            var routeDescriptors = new List<SoaServiceRouteDesc>(_routes.Count());
            foreach (var route in _routes)
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

            return Task.FromResult(routeDescriptors);
        }

        public Task<List<SoaAddress>> GetAddressAsync()
        {
            return Task.FromResult(_routes.SelectMany(x => x.Address).Distinct().ToList());
        }
    }
}
