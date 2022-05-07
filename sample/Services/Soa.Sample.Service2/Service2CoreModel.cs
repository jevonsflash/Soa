using Abp;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Soa.Sample.Service2
{
    [DependsOn(typeof(AbpKernelModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class Service2CoreModel : AbpModule
    {

        public override void Initialize()
        {
            var thisAssembly = typeof(Service2CoreModel).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

        }
    }
}
