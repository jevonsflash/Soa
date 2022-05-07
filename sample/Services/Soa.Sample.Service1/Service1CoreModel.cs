using Abp;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System.Reflection;

namespace Soa.Sample.Service1
{
    [DependsOn(typeof(AbpKernelModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]

    public class Service1CoreModel : AbpModule
    {

        public override void Initialize()
        {
            var thisAssembly = typeof(Service1CoreModel).GetAssembly();

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
                Assembly.Load("Soa.Sample.IService1")
            };
            return result;
        }
    }
}
