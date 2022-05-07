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
using Castle.MicroKernel.Registration;
using Soa.Server.TransportServer;
using Soa.Server.Transport.DotNetty;
using Soa.Protocols.Service;
using Microsoft.Extensions.Configuration;
using Soa.Common;
using Microsoft.AspNetCore.Hosting;
using Soa.Server.Configuration;
using Abp.Logging;
using Soa.Server.Transport.Http;
using System.Runtime.Loader;
using Soa.Server.ServiceContainer;
using System.Threading;
using Soa.Server.Discovery;
using Microsoft.Extensions.Hosting;
using Soa.ServiceId;
using Soa.Protocols.Service;

namespace Soa.Server
{
    [DependsOn(typeof(SoaModule))]
    public class SoaServerModule : AbpModule
    {
        private IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment env;
        private List<Type> serviceTypes;

        public SoaServerModule(IWebHostEnvironment env)
        {
            this.env = env;
        }


        public override void PreInitialize()
        {

            _appConfiguration = AppConfigurations.Get(
typeof(SoaServerModule).GetAssembly().GetDirectoryPathOrNull(), env.EnvironmentName, env.IsDevelopment());
            IocManager.Register<ISoaServerConfiguration, SoaServerConfiguration>();
            Configuration.Modules.SoaServer().Ip = _appConfiguration["SoaServer:Ip"];
            Configuration.Modules.SoaServer().Port = int.Parse(_appConfiguration["SoaServer:Port"]);
            Configuration.Modules.SoaServer().Transport = _appConfiguration["SoaServer:Transport"];
            Configuration.Modules.SoaServer().AssemblyNames = _appConfiguration["SoaServer:AssemblyNames"].Split(',') ;

          
        }


        public override void Initialize()
        {
            var assemblies = new List<Assembly>();
            foreach (var assemblyName in Configuration.Modules.SoaServer().AssemblyNames)
            {
                var name = assemblyName;
                if (name.EndsWith(".dll")) name = name.Substring(0, name.Length - 4);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(name));
                assemblies.Add(assembly);
            }

            serviceTypes = assemblies.SelectMany(x => x.ExportedTypes)
           .Where(x => typeof(ISoaService).GetTypeInfo().IsAssignableFrom(x)).ToList();


            var config = IocManager.Resolve<ISoaServerConfiguration>();

            var ip = config.Ip;
            var port = config.Port;

            var thisAssembly = typeof(SoaServerModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);


            IocManager.Register<IServiceEntryContainer, ServiceEntryContainer>();
            if (!IocManager.IsRegistered<IServiceIdGenerator>())
                IocManager.Register<IServiceIdGenerator, ServiceIdGenerator>();
            // register module
            assemblies.ForEach(x => { IocManager.RegisterAssemblyByConvention(x); });


            if (config.Transport == "DotNetty")
            {
                IocManager.IocContainer.Register(Component.For<IServer, DotNettyServer>()
.ImplementedBy<DotNettyServer>()
.DependsOn(Dependency.OnValue("address", new DotNettyAddress(ip, port)))
.LifestyleSingleton());


            }
            else if (config.Transport == "Http")
            {
                IocManager.IocContainer.Register(Component.For<IServer, HttpServer>()
  .ImplementedBy<HttpServer>()
  .DependsOn(
      Dependency.OnValue("ip", ip),
      Dependency.OnValue("port", port))

  .LifestyleSingleton());
            }
            else
            {
                throw new Exception("不合法的Transport名称");
            }

        }

        public async override void PostInitialize()
        {

            var serviceEntryContainer = IocManager.Resolve<IServiceEntryContainer>();
            serviceEntryContainer.AddServices(serviceTypes.ToArray());
            while (!IocManager.IsRegistered(typeof(IServer)))
            {
                default(SpinWait).SpinOnce();
            }
            LogHelper.Logger.Info($"[config]use dotnetty for transfer");
            var server = IocManager.Resolve<IServer>();
            var routes = server.GetServiceRoutes();
            // register the method of GetRoutesAsync as microservice so that the client can call it from remote
            serviceEntryContainer.AddServices(new[] { typeof(InServerServiceDiscovery) });
            var discovery = IocManager.Resolve<IServiceDiscovery>();
            await discovery.ClearAsync();
            await discovery.SetRoutesAsync(routes);
            await server.StartAsync();
            base.PostInitialize();
        }




    }
}
