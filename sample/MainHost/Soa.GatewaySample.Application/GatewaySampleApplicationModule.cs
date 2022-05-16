using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.GatewaySample.Authorization;

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
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(GatewaySampleApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
