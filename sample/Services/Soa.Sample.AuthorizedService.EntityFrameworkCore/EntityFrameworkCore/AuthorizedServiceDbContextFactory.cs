using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Soa.Sample.AuthorizedService.Configuration;
using Soa.Sample.AuthorizedService.Web;
using System.Runtime.InteropServices;

namespace Soa.AuthorizedService.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class AuthorizedServiceDbContextFactory : IDesignTimeDbContextFactory<AuthorizedServiceDbContext>
    {
        public AuthorizedServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuthorizedServiceDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(
                     RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                    "Default_Docker" : "Default"
                    )
            );

            return new AuthorizedServiceDbContext(builder.Options);
        }
    }
}