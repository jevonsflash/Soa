using System.Threading.Tasks;
using Soa.GatewaySample.Configuration.Dto;

namespace Soa.GatewaySample.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
