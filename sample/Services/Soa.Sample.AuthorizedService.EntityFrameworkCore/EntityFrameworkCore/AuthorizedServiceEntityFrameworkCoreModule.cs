using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.Reflection.Extensions;
using AuthorizedService.EntityFrameworkCore.Helper;
using AuthorizedService.EntityFrameworkCore.Seed;
using Microsoft.EntityFrameworkCore;
using Soa.Sample.AuthorizedService;
using System.Configuration;
using System.Transactions;

namespace Soa.AuthorizedService.EntityFrameworkCore
{
    [DependsOn(
        typeof(AuthorizedServiceCoreModel),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AuthorizedServiceEntityFrameworkCoreModule : AbpModule
    {

        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {

            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<AuthorizedServiceDbContext>(options =>
                {

                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);

                });
            }

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AuthorizedServiceEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var connectionStringResolver = IocManager.Resolve<IConnectionStringResolver>();

            var connStrSection = connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host));

            using (var _unitOfWorkManager = IocManager.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = _unitOfWorkManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var dbContextResolver = IocManager.Resolve<IDbContextResolver>();

                    using (var dbContext = dbContextResolver.Resolve<AuthorizedServiceDbContext>(connStrSection, null))
                    {
                        dbContext.Database.Migrate();
                        RunMigrate(dbContext);
                        _unitOfWorkManager.Object.Current.SaveChanges();
                        uow.Complete();
                    }
                }
            }
            WithDbContextHelper.WithDbContext<AuthorizedServiceDbContext>(IocManager, RunMigrate);
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }

        public static void RunMigrate(AuthorizedServiceDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }

    }
}