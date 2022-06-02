using Soa.Sample.Configuration;
using Soa.Sample.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace Soa.Sample.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class SampleDbContextFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        public SampleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SampleDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(
                    RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                    "Default_Docker" : SampleConsts.ConnectionStringName
                    )
            );

            return new SampleDbContext(builder.Options);
        }
    }
}