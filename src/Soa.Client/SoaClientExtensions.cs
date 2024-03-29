﻿using Abp;
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
using Abp.Castle.Logging.Log4Net;
using Abp.IO;
using Abp.PlugIns;
using System.IO;

namespace Soa.Client
{
    public static class SoaClientExtensions
    {
        public static IServiceProvider AddSoaClient<TStartupModule>(this IServiceCollection services)
           where TStartupModule : AbpModule
        {
            return AddSoaClient<TStartupModule>(services, null);

        }

        public static IServiceProvider AddSoaClient<TStartupModule>(this IServiceCollection services, SoaClientOptions options, bool isWithoutCreatingServiceProvider = false)
   where TStartupModule : AbpModule
        {
            var currentUseLogger = options?.LoggerProvider;
            var isdev = options.IsDevelopment;
            var plugInsPath = options?.PlugInsPath;

            //Configure MVC
            services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.Conventions.Add(new SoaServiceConvention(services));
            });
            Action<AbpBootstrapperOptions> optionsAction = options =>
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
            };
            IServiceProvider result = null;
            if (!isWithoutCreatingServiceProvider)
            {
                result = services.AddAbp<TStartupModule>(optionsAction);
            }
            else
            {
                services.AddAbpWithoutCreatingServiceProvider<TStartupModule>(optionsAction);

            }


            //Add feature providers

            var abpBootstrapper = services.GetSingletonService<AbpBootstrapper>();
            var iocResolver = abpBootstrapper.IocManager;
            var partManager = services.GetSingletonServiceOrNull<ApplicationPartManager>();
            partManager?.FeatureProviders.Add(new SoaControllerFeatureProvider(iocResolver));

            return result;


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

        public static void UseSoaClient(this IApplicationBuilder app, Action<AbpApplicationBuilderOptions> optionsAction = null)
        {
            app.UseAbp(optionsAction);
        }
    }
}
