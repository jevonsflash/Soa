using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Soa.Client.ApiGateway.Core
{
    public class SoaQueryStringModelBinder : IModelBinder
    {
        private readonly IModelBinder _modelBinder;
        public SoaQueryStringModelBinder(IModelBinder modelBinder)
        {
            _modelBinder = modelBinder;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var query = bindingContext.ActionContext.HttpContext.Request.QueryString.Value;
            var queryString = new SoaQueryString(query);
            bindingContext.ModelState.SetModelValue("query", queryString, null);
            bindingContext.Result = ModelBindingResult.Success(queryString);
            return Task.CompletedTask;
        }
    }
}
