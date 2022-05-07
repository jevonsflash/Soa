using System;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
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
            builder.Services.AddSoa<Service2HostModel>();
            var webapp = builder.Build();
            webapp.UseSoa();
            webapp.Run();
        }
    }
}
