using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Soa.Sample.MainService;
using Soa.Sample.Service1;
using Soa.Sample.Service2;

namespace Soa.Sample.EntityFrameworkCore
{
    public class SampleDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {

        }
        public DbSet<MainEntity> MainEntity { get; set; }
        public DbSet<Entity1> Entity1 { get; set; }
        public DbSet<Entity2> Entity2 { get; set; }
    }
}
