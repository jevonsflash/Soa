using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.Sample.AuthorizedService;

namespace Soa.AuthorizedService.EntityFrameworkCore
{
    [DependsOn(
        typeof(AuthorizedServiceCoreModel),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AuthorizedServiceEntityFrameworkCoreModule : AbpModule
    {

        public override void PreInitialize()
        {

            Configuration.Modules.AbpEfCore().AddDbContext<AuthorizedServiceDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AuthorizedServiceEntityFrameworkCoreModule).GetAssembly());
        }


    }
}