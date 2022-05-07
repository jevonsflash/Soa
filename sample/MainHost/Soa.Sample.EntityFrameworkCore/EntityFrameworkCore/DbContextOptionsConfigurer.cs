using Microsoft.EntityFrameworkCore;

namespace Soa.Sample.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<SampleDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for SampleDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}
