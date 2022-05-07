using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Soa.Configuration;
using Soa.Hangfire;
using Soa.Sample.Configuration;
using Soa.Sample.Localization;
using Abp.Threading.BackgroundWorkers;

namespace Soa.Sample
{
    [DependsOn(typeof(AbpHangfireModule))]
    public class SampleCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            SampleLocalizationConfigurer.Configure(Configuration.Localization);
            
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = SampleConsts.DefaultPassPhrase;
       
            IocManager.Register<IBackgroundJobFullFeatrueManager, HangfireBackgroundJobFullFeatrueManager>(DependencyLifeStyle.Singleton);
            


        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SampleCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {


            Configuration.BackgroundJobs.UseHangfire<HangfireBackgroundJobFullFeatrueManager>(configuration =>
            {

                (configuration as AbpHangfireConfiguration).GlobalConfiguration.UseSqlServerStorage(Configuration.DefaultNameOrConnectionString);
            });
            var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workerManager.Add(IocManager.Resolve<IBackgroundJobFullFeatrueManager>());

        }

    }
}