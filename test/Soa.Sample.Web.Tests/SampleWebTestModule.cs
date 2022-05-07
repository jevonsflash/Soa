using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.Sample.Web.Startup;
namespace Soa.Sample.Web.Tests
{
    [DependsOn(
        typeof(SampleWebModule),
        typeof(AbpAspNetCoreTestBaseModule)
        )]
    public class SampleWebTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SampleWebTestModule).GetAssembly());
        }
    }
}