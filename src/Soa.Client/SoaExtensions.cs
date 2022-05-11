using Abp;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Providers;
using Abp.Modules;
using Castle.Facilities.Logging;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Soa.Logging.Console;
using System;

namespace Soa.Client
{
    public static class SoaExtensions
    {
        public static IServiceProvider AddSoa<TStartupModule>(this IServiceCollection services)
           where TStartupModule : AbpModule
        {
            //Configure MVC
            services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.Conventions.Add(new SoaServiceConvention(services));
            });
            var result = services.AddAbp<TStartupModule>(options =>
             {
                 options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                     f => f.UseConsoleLogging()
                 );

                 //options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                 //    f => f.UseAbpLog4Net().WithConfig(
                 //        _hostingEnvironment.IsDevelopment()
                 //            ? "log4net.config"
                 //            : "log4net.Production.config"
                 //        )
                 //);
             });

            //Add feature providers

            var abpBootstrapper = services.GetSingletonService<AbpBootstrapper>();
            var iocResolver = abpBootstrapper.IocManager;
            var partManager = services.GetSingletonServiceOrNull<ApplicationPartManager>();
            partManager?.FeatureProviders.Add(new SoaControllerFeatureProvider(iocResolver));

            return result;


        }



        public static void UseSoa(this IApplicationBuilder app)
        {
            app.UseAbp();
        }
    }
}
