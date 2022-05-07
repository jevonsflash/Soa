using System;
using System.Threading.Tasks;
using Abp.TestBase;
using Soa.Sample.EntityFrameworkCore;
using Soa.Sample.Tests.TestDatas;

namespace Soa.Sample.Tests
{
    public class SampleTestBase : AbpIntegratedTestBase<SampleTestModule>
    {
        public SampleTestBase()
        {
            UsingDbContext(context => new TestDataBuilder(context).Build());
        }

        protected virtual void UsingDbContext(Action<SampleDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<SampleDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected virtual T UsingDbContext<T>(Func<SampleDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<SampleDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<SampleDbContext, Task> action)
        {
            using (var context = LocalIocManager.Resolve<SampleDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<SampleDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<SampleDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
