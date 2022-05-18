using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Threading.BackgroundWorkers;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Hangfire;
using Soa.Configuration;
using Soa.GatewaySample.Authorization.Roles;
using Soa.GatewaySample.Authorization.Users;
using Soa.GatewaySample.Configuration;
using Soa.GatewaySample.Localization;
using Soa.GatewaySample.MultiTenancy;
using Soa.GatewaySample.Timing;
using Soa.Hangfire;
using Soa.Sample.IAuthorizedService.Localization;

namespace Soa.GatewaySample
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    [DependsOn(typeof(AbpHangfireModule))]
    public class GatewaySampleCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            GatewaySampleLocalizationConfigurer.Configure(Configuration.Localization);
            AuthorizedServiceLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = GatewaySampleConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = GatewaySampleConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = GatewaySampleConsts.DefaultPassPhrase;
       
            IocManager.Register<IBackgroundJobFullFeatrueManager, HangfireBackgroundJobFullFeatrueManager>(DependencyLifeStyle.Singleton);

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GatewaySampleCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
            Configuration.BackgroundJobs.UseHangfire<HangfireBackgroundJobFullFeatrueManager>(configuration =>
            {

                (configuration as AbpHangfireConfiguration).GlobalConfiguration.UseSqlServerStorage(Configuration.DefaultNameOrConnectionString);
            });
            var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workerManager.Add(IocManager.Resolve<IBackgroundJobFullFeatrueManager>());

        }
    }
}
