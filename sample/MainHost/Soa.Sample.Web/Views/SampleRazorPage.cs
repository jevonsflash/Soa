using Abp.AspNetCore.Mvc.Views;

namespace Soa.Sample.Web.Views
{
    public abstract class SampleRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected SampleRazorPage()
        {
            LocalizationSourceName = SampleConsts.LocalizationSourceName;
        }
    }
}
