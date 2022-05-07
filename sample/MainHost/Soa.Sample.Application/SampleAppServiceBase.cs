using Abp.Application.Services;

namespace Soa.Sample
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class SampleAppServiceBase : ApplicationService
    {
        protected SampleAppServiceBase()
        {
            LocalizationSourceName = SampleConsts.LocalizationSourceName;
        }
    }
}