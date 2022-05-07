using Microsoft.AspNetCore.Http;

namespace Soa.Client.ApiGateway.Core
{
    public static class SoaHttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }

    //public static class StaticHttpContextExtensions
    //{
    //    public static void AddHttpContextAccessor(this IServiceCollection services)
    //    {
    //        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    //    }

    //    public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
    //    {
    //        var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
    //        SoaHttpContext.Configure(httpContextAccessor);
    //        return app;
    //    }
    //}
}
