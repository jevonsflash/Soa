using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Soa.Sample
{
    [DependsOn(
        typeof(SampleCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class SampleApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SampleApplicationModule).GetAssembly());
        }
    }
}