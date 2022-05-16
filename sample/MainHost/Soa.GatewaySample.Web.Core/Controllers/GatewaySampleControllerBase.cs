using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Soa.GatewaySample.Controllers
{
    public abstract class GatewaySampleControllerBase: AbpController
    {
        protected GatewaySampleControllerBase()
        {
            LocalizationSourceName = GatewaySampleConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
