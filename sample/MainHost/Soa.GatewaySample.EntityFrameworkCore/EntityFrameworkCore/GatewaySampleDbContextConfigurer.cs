using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Soa.GatewaySample.EntityFrameworkCore
{
    public static class GatewaySampleDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<GatewaySampleDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<GatewaySampleDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
