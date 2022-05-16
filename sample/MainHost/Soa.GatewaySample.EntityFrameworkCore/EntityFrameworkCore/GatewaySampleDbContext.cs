using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Soa.GatewaySample.Authorization.Roles;
using Soa.GatewaySample.Authorization.Users;
using Soa.GatewaySample.MultiTenancy;

namespace Soa.GatewaySample.EntityFrameworkCore
{
    public class GatewaySampleDbContext : AbpZeroDbContext<Tenant, Role, User, GatewaySampleDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public GatewaySampleDbContext(DbContextOptions<GatewaySampleDbContext> options)
            : base(options)
        {
        }
    }
}
