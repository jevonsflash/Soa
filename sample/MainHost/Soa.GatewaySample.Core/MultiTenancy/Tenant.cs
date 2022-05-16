using Abp.MultiTenancy;
using Soa.GatewaySample.Authorization.Users;

namespace Soa.GatewaySample.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
