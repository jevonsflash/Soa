using Microsoft.EntityFrameworkCore;

namespace Soa.AuthorizedService.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<AuthorizedServiceDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for AuthorizedServiceDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}
