using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Soa.GatewaySample.Authorization.Users;
using Soa.GatewaySample.Editions;

namespace Soa.GatewaySample.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
