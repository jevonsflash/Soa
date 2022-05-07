using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Soa.Sample.EntityFrameworkCore
{
    [DependsOn(
        typeof(SampleCoreModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class SampleEntityFrameworkCoreModule : AbpModule
    {

        public override void PreInitialize()
        {

            Configuration.Modules.AbpEfCore().AddDbContext<SampleDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SampleEntityFrameworkCoreModule).GetAssembly());
        }


    }
}