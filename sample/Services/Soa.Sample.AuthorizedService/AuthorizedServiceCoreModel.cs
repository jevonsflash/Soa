using Abp;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.Sample.IAuthorizedService.Localization;
using System.Reflection;

namespace Soa.Sample.AuthorizedService
{
    [DependsOn(typeof(AbpKernelModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]

    public class AuthorizedServiceCoreModel : AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            AuthorizedServiceLocalizationConfigurer.Configure(Configuration.Localization);

        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AuthorizedServiceCoreModel).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
               cfg => cfg.AddMaps(thisAssembly)
            );
        }

        public override Assembly[] GetAdditionalAssemblies()
        {
            Assembly[] result = new[]
            {
                Assembly.Load("Soa.Sample.IAuthorizedService")
            };
            return result;
        }
    }
}
