using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Soa.AuthorizedService.EntityFrameworkCore;
using Soa.Sample.AuthorizedService.Configuration;
using Soa.Server;

namespace Soa.Sample.AuthorizedService.Host
{
    [DependsOn(typeof(AuthorizedServiceEntityFrameworkCoreModule))]
    [DependsOn(typeof(AuthorizedServiceCoreModel))]
    [DependsOn(typeof(SoaServerModule))]
    public class AuthorizedServiceHostModel : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AuthorizedServiceHostModel(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(AuthorizedServiceHostModel).GetAssembly().GetDirectoryPathOrNull(), env.EnvironmentName
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString("Default");
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AuthorizedServiceHostModel).GetAssembly());
        }

    }
}
