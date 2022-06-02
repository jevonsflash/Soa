using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Soa.Sample.Configuration;
using Soa.Sample.EntityFrameworkCore;
using Soa.Server;

namespace Soa.Sample.Service1.Host
{
    [DependsOn(typeof(SampleEntityFrameworkCoreModule))]
    [DependsOn(typeof(Service1CoreModel))]
    [DependsOn(typeof(SoaServerModule))]
    public class Service1HostModel : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Service1HostModel(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(Service1HostModel).GetAssembly().GetDirectoryPathOrNull(), env.EnvironmentName
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                    RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                    "Default_Docker" : "Default"
                );
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service1HostModel).GetAssembly());
        }

    }
}
