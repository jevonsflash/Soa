using Abp.AspNetCore;
using Abp.Modules;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Soa.Logging.Console;
using System;

namespace Soa.Server
{
    public static class SoaExtensions
    {
        public static IServiceProvider AddSoa<TStartupModule>(this IServiceCollection services)
           where TStartupModule : AbpModule
        {
            return services.AddAbp<TStartupModule>(options =>
            {
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseConsoleLogging()
                );
            });

        }

        public static void UseSoa(this IApplicationBuilder app)
        {
            app.UseAbp();
        }
    }
}
