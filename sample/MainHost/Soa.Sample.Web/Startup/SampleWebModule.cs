using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.Sample.Configuration;
using Soa.Sample.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Abp.Configuration.Startup;

namespace Soa.Sample.Web.Startup
{
    [DependsOn(
        typeof(SampleApplicationModule), 
        typeof(SampleEntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    [DependsOn(typeof(SoaClientModule))]

    public class SampleWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public SampleWebModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(SampleConsts.ConnectionStringName);

            Configuration.Navigation.Providers.Add<SampleNavigationProvider>();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(SampleApplicationModule).GetAssembly()
                );



            Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = true;

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SampleWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(SampleWebModule).Assembly);
        }
    }
}
