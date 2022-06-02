using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.GatewaySample.Configuration;
using Soa.GatewaySample.EntityFrameworkCore;
using Soa.GatewaySample.Migrator.DependencyInjection;
using System.Runtime.InteropServices;

namespace Soa.GatewaySample.Migrator
{
    [DependsOn(typeof(GatewaySampleEntityFrameworkModule))]
    public class GatewaySampleMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public GatewaySampleMigratorModule(GatewaySampleEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(GatewaySampleMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                "Default_Docker" : GatewaySampleConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GatewaySampleMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
