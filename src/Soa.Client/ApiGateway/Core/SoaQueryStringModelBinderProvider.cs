using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Soa.Client.ApiGateway.Core
{
    public class SoaQueryStringModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder _modelBinder = new SoaQueryStringModelBinder(new SimpleTypeModelBinder(typeof(SoaQueryString),null));
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(SoaQueryString) ? _modelBinder : null;
        }
    }
}
