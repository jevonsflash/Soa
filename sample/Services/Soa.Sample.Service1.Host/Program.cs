using System;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
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
            builder.Services.AddSoa<Service1HostModel>();
            var webapp = builder.Build();
            webapp.UseSoa();
            webapp.Run();
        }
    }
}
