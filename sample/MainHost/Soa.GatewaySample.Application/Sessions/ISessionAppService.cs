using System.Threading.Tasks;
using Abp.Application.Services;
using Soa.GatewaySample.Sessions.Dto;

namespace Soa.GatewaySample.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
