using System.Collections.Generic;
using System.Linq;
using Abp.Logging;

using Soa.Common.Protocols.Service;
using Soa.Common.Protocols.ServiceDesc;
using Soa.Common.Serializer;
using Soa.Server.Discovery;
using Soa.Server.OAuth.Middlewares;
using Soa.Server.TransportServer;

namespace Soa.Server.OAuth
{
    public static partial class ServiceHostBuilderExtension
    {
        public static IServiceHostServerBuilder UseJoseJwtForOAuth<T>(this IServiceHostServerBuilder serviceHostBuilder, JwtAuthorizationOptions options) where T : SoaAddress, new()
        {
            serviceHostBuilder.AddInitializer(container =>
            {
                LogHelper.Logger.Info($"[config]use jose.jwt for OAuth");

                var server = container.Resolve<IServer>();
                var serializer = container.Resolve<ISerializer>();
                server.UseMiddleware<JwtAuthorizationMiddleware>(options, serializer);

                if (string.IsNullOrEmpty(options.TokenEndpointPath)) return;
                var discovery = container.Resolve<IServiceDiscovery>();
                var addr = new T
                {
                    Ip = options.ServerIp,
                    Port = options.ServerPort
                };
                var tokenRoute = new List<SoaServiceRoute> {
                    new SoaServiceRoute
                    {
                        Address = new List<SoaAddress>{
                            addr
                        },
                        ServiceDescriptor = new SoaServiceDesc
                        {
                            Id=options.GetServiceId(),
                            RoutePath = options.TokenEndpointPath
                        }
                    }
                };
                discovery.ClearServiceAsync(tokenRoute.First().ServiceDescriptor.Id);
                //discovery.SetRoutesAsync(tokenRoute);
                discovery.AddRouteAsync(tokenRoute);
            });
            return serviceHostBuilder;
        }
    }
}
