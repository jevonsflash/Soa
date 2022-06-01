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

namespace AuthorizedService.EntityFrameworkCore.Helper
{
    public class WithDbContextHelper
    {
        public static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
    where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>();

                    contextAction(context);

                    uow.Complete();
                }
            }
        }

    }
}
