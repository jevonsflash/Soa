using Abp.Application.Services;
using Soa.GatewaySample.MultiTenancy.Dto;

namespace Soa.GatewaySample.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

