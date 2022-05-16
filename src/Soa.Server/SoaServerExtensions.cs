using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.IO;
using Abp.Modules;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Soa.Logging.Console;
using System;
using Abp.PlugIns;
using System.IO;

namespace Soa.Server
{
    public static class SoaServerExtensions
    {
        public static IServiceProvider AddSoaServer<TStartupModule>(this IServiceCollection services)
           where TStartupModule : AbpModule
        {
            return AddSoaServer<TStartupModule>(services, null);
        }
        public static IServiceProvider AddSoaServer<TStartupModule>(this IServiceCollection services, SoaServerOptions options)
           where TStartupModule : AbpModule
        {

            var currentUseLogger = options?.LoggerProvider;
            var isdev = options.IsDevelopment;
            var plugInsPath = options?.PlugInsPath;

            return services.AddAbp<TStartupModule>(options =>
            {
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                   f =>
                   {
                       if (currentUseLogger.ToUpper() == "CONSOLELOGGER")
                       {
                           GetConsoleLoggingFacility(f);
                       }
                       else if (currentUseLogger.ToUpper() == "LOG4NET")
                       {
                           GetLog4NetFacility(f, isdev);
                       }
                   }
               );

                if (!string.IsNullOrEmpty(plugInsPath))
                {
                    DirectoryHelper.CreateIfNotExists(plugInsPath);

                    options.PlugInSources.AddFolder(plugInsPath, SearchOption.AllDirectories);
                }



            });

        }

        private static LoggingFacility GetConsoleLoggingFacility(LoggingFacility f)
        {
            return f.UseConsoleLogging();
        }

        private static LoggingFacility GetLog4NetFacility(LoggingFacility f, bool isDevelopment)
        {

            return f.UseAbpLog4Net().WithConfig(isDevelopment
                                           ? "log4net.config"
                                           : "log4net.Production.config"
                                       );
        }

        public static void UseSoaServer(this IApplicationBuilder app, Action<AbpApplicationBuilderOptions> optionsAction = null)
        {
            app.UseAbp(optionsAction);
        }
    }
}
