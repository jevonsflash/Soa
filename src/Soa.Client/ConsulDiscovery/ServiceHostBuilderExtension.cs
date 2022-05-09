using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Logging;
using Consul;
using Soa.Client.Discovery;
using Soa.Common.Protocols.Service;
using Soa.Common.Protocols.ServiceDesc;
using Soa.Common.Serializer;

namespace Soa.Client.ConsulDiscovery
{
    public static class ServiceHostBuilderExtension
    {
        public static IServiceHostClientBuilder UseConsulForDiscovery(this IServiceHostClientBuilder serviceHostBuilder,
            string ip, int port, string serviceCategory)
        {
            serviceHostBuilder.AddInitializer(container =>
            {
                LogHelper.Logger.Info($"[config]use consul for services discovery, consul ip: {ip}:{port}, service cateogry: {serviceCategory}");

                var clientDiscovery = container.Resolve<IClientServiceDiscovery>();
                var serializer = container.Resolve<ISerializer>();
                clientDiscovery.AddRoutesGetter(async () =>
                {
                    var consul = new ConsulClient(config => { config.Address = new Uri($"http://{ip}:{port}"); });
                    var queryResult = await consul.KV.Keys(serviceCategory);
                    var keys = queryResult.Response;
                    if (keys == null)
                    {
                        return null;
                    }

                    var routes = new List<SoaServiceRoute>();
                    foreach (var key in keys)
                    {
                        var data = (await consul.KV.Get(key)).Response?.Value;
                        if (data == null)
                        {
                            continue;
                        }

                        var descriptors = serializer.Deserialize<byte[], List<SoaServiceRouteDesc>>(data);
                        if (descriptors != null && descriptors.Any())
                        {
                            foreach (var descriptor in descriptors)
                            {
                                List<SoaAddress> addresses =
                                    new List<SoaAddress>(descriptor.AddressDescriptors.ToArray().Count());
                                foreach (var addDesc in descriptor.AddressDescriptors)
                                {
                                    var addrType = Type.GetType(addDesc.Type);
                                    addresses.Add(serializer.Deserialize(addDesc.Value, addrType) as SoaAddress);
                                }

                                routes.Add(new SoaServiceRoute
                                {
                                    Address = addresses,
                                    ServiceDescriptor = descriptor.ServiceDescriptor
                                });
                            }
                        }

                    }

                    return routes;
                });
            });

            return serviceHostBuilder;
        }
    }
}
