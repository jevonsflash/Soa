using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.GatewaySample.Authorization;
using Soa.Sample.IAuthorizedService.Authorization;
using System.Reflection;

namespace Soa.GatewaySample
{
    [DependsOn(
        typeof(GatewaySampleCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class GatewaySampleApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<GatewaySampleAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<AuthorizedServiceAuthorizationProvider>();

        }

        public override void Initialize()
        {
            var thisAssembly = typeof(GatewaySampleApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            var ass = Assembly.Load("Soa.Sample.IAuthorizedService");
            IocManager.RegisterAssemblyByConvention(ass);

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
