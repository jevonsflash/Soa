using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soa.Protocols.Service;
using Soa.Protocols.Service;
using Soa.Serializer;
using Soa.Server.ServiceContainer;
using Soa.Server.TransportServer;
using Soa.TypeConverter;

namespace Soa.Server.Transport.Http
{
    public class HttpServer : IServer
    {
        private readonly List<SoaServiceRoute> _serviceRoutes = new List<SoaServiceRoute>();
        private readonly IServiceEntryContainer _serviceEntryContainer;
        private readonly string _ip;
        private readonly int _port;
        private readonly ISerializer _serializer;
        private readonly Stack<Func<RequestDel, RequestDel>> _middlewares;
        private readonly ITypeConvertProvider _typeConvert;
        public HttpServer(string ip, int port,  IServiceEntryContainer serviceEntryContainer, ISerializer serializer, ITypeConvertProvider typeConvert)
        {
            _serviceEntryContainer = serviceEntryContainer;
            _ip = ip;
            _port = port;
            _serializer = serializer;
            _middlewares = new Stack<Func<RequestDel, RequestDel>>();
            _typeConvert = typeConvert;
        }
        public List<SoaServiceRoute> GetServiceRoutes()
        {
            if (!_serviceRoutes.Any())
            {
                var serviceEntries = _serviceEntryContainer.GetServiceEntry();
                serviceEntries.ForEach(entry =>
                {
                    var serviceRoute = new SoaServiceRoute
                    {
                        Address = new List<SoaAddress> {
                            new HttpAddress(_ip, _port){IsHealth = true}
                            },
                        ServiceDescriptor = entry.Descriptor
                    };
                    _serviceRoutes.Add(serviceRoute);
                });
            }

            return _serviceRoutes;
        }

        public Task StartAsync()
        {
            LogHelper.Logger.Info($"start server: {_ip}:{_port}\r\n");

            var builder = new WebHostBuilder()
              .UseKestrel()
              .UseIISIntegration()
              .UseSetting("detailedErrors", "true")
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseUrls($"http://{_ip}:{_port}")
              .ConfigureServices(services =>
              {
                  services.AddSingleton<IStartup>(new Startup(new ConfigurationBuilder().Build(), _middlewares, _serviceEntryContainer, _serializer, _typeConvert));
              })
              .UseSetting(WebHostDefaults.ApplicationKey, typeof(Startup).GetTypeInfo().Assembly.FullName);

            var host = builder.Build();
            host.Run();
            var endpoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
            LogHelper.Logger.Info($"server start successfuly, address is： {endpoint}\r\n");
            return Task.CompletedTask;
        }

        public IServer Use(Func<RequestDel, RequestDel> middleware)
        {
            _middlewares.Push(middleware);
            return this;
        }
    }
}
