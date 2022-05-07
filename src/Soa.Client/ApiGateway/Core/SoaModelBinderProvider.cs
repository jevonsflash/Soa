using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Soa.Client.ApiGateway.Core
{
    public class SoaModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder _modelBinder = new SoaModelBinder(new SimpleTypeModelBinder(typeof(SoaModel),null));
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(SoaModel) ? _modelBinder : null;
        }
    }
}
