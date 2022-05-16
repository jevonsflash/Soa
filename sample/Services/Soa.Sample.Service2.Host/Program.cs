using System;
using System.IO;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Soa.Server;

namespace Soa.Sample.Service2.Host
{
    class Program
    {
        private static WebApplicationBuilder builder;
        static void Main(string[] args)
        {
            Console.WriteLine("Service2 Host Started!");
            var ioc = new IocManager();
            builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSoaServer<Service2HostModel>(new SoaServerOptions()
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
