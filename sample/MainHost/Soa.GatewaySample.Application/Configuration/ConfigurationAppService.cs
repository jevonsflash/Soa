using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Soa.GatewaySample.Configuration.Dto;

namespace Soa.GatewaySample.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : GatewaySampleAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
