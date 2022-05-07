using Abp.AspNetCore.Mvc.Controllers;

namespace Soa.Sample.Web.Controllers
{
    public abstract class SampleControllerBase: AbpController
    {
        protected SampleControllerBase()
        {
            LocalizationSourceName = SampleConsts.LocalizationSourceName;
        }
    }
}