using System;
using System.IO;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Soa.Server;
using Soa.Server.Transport.Http;

namespace Soa.Sample.Service1.Host
{
    class Program
    {
        private static WebApplicationBuilder builder;

        static void Main(string[] args)
        {
            Console.WriteLine("Service1 Host Started!");
            var ioc = new IocManager();
            builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSoaServer<Service1HostModel>(new SoaServerOptions()
            {
                IsDevelopment = builder.Environment.IsDevelopment(),
                LoggerProvider = "CONSOLELOGGER",
                PlugInsPath = Path.Combine(builder.Environment.ContentRootPath, "PlugIns")

            });
            var webapp = builder.Build();
            webapp.UseSoaServer();
            webapp.Run();
        }
    }
}
