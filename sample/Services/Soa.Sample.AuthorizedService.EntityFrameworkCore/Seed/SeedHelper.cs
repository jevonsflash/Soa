using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using Soa.AuthorizedService.EntityFrameworkCore;

namespace AuthorizedService.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            Helper.WithDbContextHelper.WithDbContext<AuthorizedServiceDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(AuthorizedServiceDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            var builder = new InitialDbBuilder(context);
            builder.CreateMusic();
            builder.CreateMovie();
        }

    }
}
