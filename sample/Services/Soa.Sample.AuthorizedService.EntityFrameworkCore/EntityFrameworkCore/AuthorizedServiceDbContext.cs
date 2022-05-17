using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Soa.Sample.AuthorizedService.Models;

namespace Soa.AuthorizedService.EntityFrameworkCore
{
    public class AuthorizedServiceDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public AuthorizedServiceDbContext(DbContextOptions<AuthorizedServiceDbContext> options)
            : base(options)
        {

        }
        public DbSet<Music> Music { get; set; }
        public DbSet<Movie> Movie { get; set; }
    }
}
