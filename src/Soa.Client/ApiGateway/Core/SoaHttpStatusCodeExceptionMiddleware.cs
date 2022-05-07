using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Soa.Client.ApiGateway.Core
{
    public class SoaHttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public SoaHttpStatusCodeExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SoaHttpStatusCodeException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = ex.ContentType;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
