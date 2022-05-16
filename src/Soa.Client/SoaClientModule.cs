using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Hangfire;
using Abp;
using Soa.Client.Discovery;
using Soa.Client.RemoteCaller;
using Soa.Client.Configuration;
using Microsoft.AspNetCore.Hosting;
using Soa.Common;
using Microsoft.Extensions.Configuration;
using Abp.Threading.BackgroundWorkers;
using Soa.Client.HealthCheck;
using System.Runtime.Loader;
using Soa.Client.Proxy;
using DotNetty.Transport.Bootstrapping;
using Soa.Client.TransportClient;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Codecs;
using Soa.Client.Transport.DotNetty.ChannelHandlerAdapter;
using DotNetty.Common.Utilities;
using Soa.Client.Transport.DotNetty;
using Soa.Client.Transport.Http;
using Microsoft.Extensions.Hosting;
using Soa.Client.Token;
using Soa.Client.Configuration.JobConfiguration;
using Soa.Protocols.ServiceDesc;
using Soa.Protocols.Service;
using Soa.Protocols.Attributes;
using Soa.Serializer;
using Soa.TypeConverter;
using Consul;
using Soa.Configuration;
using Abp.AspNetCore.Configuration;
using Castle.MicroKernel.Registration;

namespace Soa
{
    [DependsOn(typeof(SoaModule))]
    public class SoaClientModule : AbpModule
    {
        private IEnumerable<Type> serviceTypes;
        private IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment env;

        public SoaClientModule(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public override void PreInitialize()
        {

            _appConfiguration = AppConfigurations.Get(
                typeof(SoaClientModule).GetAssembly().GetDirectoryPathOrNull(), env.EnvironmentName, env.IsDevelopment());

            IocManager.Register<ISoaClientConfiguration, SoaClientConfiguration>();

            Configuration.Modules.AbpAspNetCore()
        .CreateControllersForAppServices(
            typeof(SoaClientModule).GetAssembly()



        );

            Configuration.Modules.SoaClient().Address = _appConfiguration.GetSection("SoaClient:Address").Get<HttpAddress[]>();
            Configuration.Modules.SoaClient().Transport = _appConfiguration["SoaClient:Transport"];
            Configuration.Modules.SoaClient().AssemblyNames = _appConfiguration["SoaClient:AssemblyNames"].Split(',');
            Configuration.Modules.SoaClient().Token = _appConfiguration["SoaClient:Token"];
            Configuration.Modules.SoaClient().HealthCheck = _appConfiguration.GetSection("SoaClient:HealthCheck").Get<HealthCheckConfiguration>();
            Configuration.Modules.SoaClient().InServiceDiscovery = _appConfiguration.GetSection("SoaClient:InServiceDiscovery").Get<InServiceDiscoveryConfiguration>();
            Configuration.Modules.SoaClient().Discovery = _appConfiguration["SoaClient:Discovery"];
            Configuration.Modules.SoaClient().ConsulServiceDiscovery = _appConfiguration.GetSection("SoaClient:ConsulServiceDiscovery").Get<ConsulServiceDiscoveryConfiguration>();

            var assemblies = new List<Assembly>();
            foreach (var assemblyName in Configuration.Modules.SoaClient().AssemblyNames)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
                assemblies.Add(assembly);
            }

            serviceTypes = assemblies.SelectMany(x => x.ExportedTypes).Where(x => x.GetMethods().Any(y => y.GetCustomAttribute<SoaServiceAttribute>() != null)).ToList();



        }


        public override void Initialize()
        {
            var thisAssembly = typeof(SoaClientModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            //ServiceProxy

            var serviceProxyGenerator = IocManager.Resolve<IServiceProxyGenerator>();
            var serviceProxyTypes = serviceProxyGenerator.GenerateProxyTypes(serviceTypes);

            var remoteServiceInvoker = IocManager.Resolve<IRemoteServiceCaller>();

            foreach (var serviceProxyType in serviceProxyTypes)
            {
                if (!IocManager.IsRegistered(serviceProxyType))
                {

                    var instance = serviceProxyType.GetTypeInfo().GetConstructors().First().Invoke(new object[]
                        {
                            remoteServiceInvoker
                        });

                    IocManager.IocContainer.Register(
                        Component.For(serviceProxyType).Instance(instance)
                    );
                }
            }

            var ass = serviceProxyGenerator.GetGeneratedServiceProxyAssembly();

            Configuration.Modules.AbpAspNetCore()
            .CreateControllersForAppServices(ass);

        }


        public override async void PostInitialize()
        {
            var clientServiceDiscovery = (ClientServiceDiscovery)IocManager.Resolve<IClientServiceDiscovery>();
            await clientServiceDiscovery?.UpdateRoutes();

            var config = IocManager.Resolve<ISoaClientConfiguration>();

            //PollingAddressSelector

            //Transfer
            var factory = IocManager.Resolve<ITransportClientFactory>();
            var serializer = IocManager.Resolve<ISerializer>();
            if (config.Transport == "DotNetty")
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(new MultithreadEventLoopGroup())
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        pipeline.AddLast(new LengthFieldPrepender(4));
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                        pipeline.AddLast(new ReadClientMessageChannelHandlerAdapter(serializer));
                        pipeline.AddLast(new ClientHandlerChannelHandlerAdapter(factory));
                    }));
                AttributeKey<IClientSender> clientSenderKey = AttributeKey<IClientSender>.ValueOf(typeof(DefaultTransportClientFactory), nameof(IClientSender));
                AttributeKey<IClientListener> clientListenerKey = AttributeKey<IClientListener>.ValueOf(typeof(DefaultTransportClientFactory), nameof(IClientListener));
                //AttributeKey<EndPoint> endPointKey = AttributeKey<EndPoint>.ValueOf(typeof(DefaultTransportClientFactory), nameof(EndPoint));
                AttributeKey<string> endPointKey = AttributeKey<string>.ValueOf(typeof(DefaultTransportClientFactory), "addresscode");
                factory.ClientCreatorDelegate += (SoaAddress address, ref ITransportClient client) =>
                {
                    //if (client == null && address.GetType().IsAssignableFrom(typeof(DotNettyAddress)))
                    if (client == null && address.ServerFlag == "DotNetty")
                    {
                        var ep = address.CreateEndPoint();
                        var channel = bootstrap.ConnectAsync(ep).Result;
                        var listener = new DotNettyClientListener();
                        channel.GetAttribute(clientListenerKey).Set(listener);
                        var sender = new DotNettyClientSender(channel, serializer);
                        channel.GetAttribute(clientSenderKey).Set(sender);
                        channel.GetAttribute(endPointKey).Set($"{address.ServerFlag}-{address.Code}");
                        client = new DefaultTransportClient(listener, sender);
                    }
                };


            }

            else if (config.Transport == "Http")
            {
                factory.ClientCreatorDelegate += (SoaAddress address, ref ITransportClient client) =>
                {
                    //if (client == null && address.GetType() == typeof(HttpAddress))
                    if (client == null && address.ServerFlag == "Http")
                    {
                        var listener = new HttpClientListener();
                        var sender = new HttpClientSender(address, listener);
                        client = new DefaultTransportClient(listener, sender);
                    }
                };

            }
            else
            {
                throw new Exception("不合法的Transport名称");
            }


            //Token

            var tokenGetter = IocManager.Resolve<IServiceTokenGetter>();
            tokenGetter.GetToken = () => config.Token;

            //InServerForDiscovery

            var clientDiscovery = IocManager.Resolve<IClientServiceDiscovery>();
            var remoteCaller = IocManager.Resolve<IRemoteServiceCaller>();

            if (config.Discovery == "Consul")
            {
                clientDiscovery.AddRoutesGetter(async () =>
                {

                    var serverAddress = $"http://{config.ConsulServiceDiscovery.Ip}:{config.ConsulServiceDiscovery.Port}";

                    var consul = new ConsulClient(config =>
                    {
                        config.Address = new Uri(serverAddress);
                    });
                    var queryResult = await consul.KV.Keys("SoaService");
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


            }
            else if (config.Discovery == "InServer")
            {
                var typeConverter = IocManager.Resolve<ITypeConvertProvider>();
                StringBuilder sb = new StringBuilder();

                var address = Configuration.Modules.SoaClient().Address;
                foreach (var addr in address)
                {
                    sb.AppendFormat(addr.Code + ",");
                    var service = new SoaServiceRoute
                    {
                        Address = new List<SoaAddress>
                        {
                            addr
                        },
                        ServiceDescriptor = new SoaServiceDesc { Id = "Soa.ServiceDiscovery.InServer.GetRoutesDescAsync" }
                    };
                    clientDiscovery.AddRoutesGetter(async () =>
                    {
                        var result = await remoteCaller.InvokeAsync(service, null, null);
                        if (result == null || result.HasError)
                        {
                            return null;
                        }

                        var routesDesc =
                            (List<SoaServiceRouteDesc>)typeConverter.Convert(result.Result,
                                typeof(List<SoaServiceRouteDesc>));
                        var routes = new List<SoaServiceRoute>();
                        foreach (var desc in routesDesc)
                        {
                            List<SoaAddress> addresses =
                                new List<SoaAddress>(desc.AddressDescriptors.ToArray().Count());
                            foreach (var addDesc in desc.AddressDescriptors)
                            {
                                var addrType = Type.GetType(addDesc.Type);
                                addresses.Add(serializer.Deserialize(addDesc.Value, addrType) as SoaAddress);
                            }

                            routes.Add(new SoaServiceRoute()
                            {
                                ServiceDescriptor = desc.ServiceDescriptor,
                                Address = addresses
                            });
                        }

                        return routes;
                    });
                }
                if (sb.Length > 0)
                {

                }
            }
            else
            {
                throw new Exception("不合法的Discovery名称");
            }












            var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //HealthCheck

            workerManager.Add(IocManager.Resolve<HealthCheckJobManager>());


            //ServiceDiscovery

            workerManager.Add(IocManager.Resolve<ServiceDiscoveryJobManager>());


            base.PostInitialize();


        }
    }
}
