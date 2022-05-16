using System.Threading.Tasks;
using Abp.Application.Services;
using Soa.GatewaySample.Authorization.Accounts.Dto;

namespace Soa.GatewaySample.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
