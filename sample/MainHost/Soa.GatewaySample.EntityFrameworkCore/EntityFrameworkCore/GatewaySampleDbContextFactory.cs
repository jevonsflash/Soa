using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Soa.GatewaySample.Configuration;
using Soa.GatewaySample.Web;
using System.Runtime.InteropServices;

namespace Soa.GatewaySample.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class GatewaySampleDbContextFactory : IDesignTimeDbContextFactory<GatewaySampleDbContext>
    {
        public GatewaySampleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GatewaySampleDbContext>();
            
            /*
             You can provide an environmentName parameter to the AppConfigurations.Get method. 
             In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
             Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
             https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
             */
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            GatewaySampleDbContextConfigurer.Configure(builder, configuration.GetConnectionString(
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                "Default_Docker" : GatewaySampleConsts.ConnectionStringName

                ));

            return new GatewaySampleDbContext(builder.Options);
        }
    }
}
