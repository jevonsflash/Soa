using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Soa.Client.ApiGateway.Core;

namespace Soa.Client.ApiGateway
{
    public static class StartupExtension
    {
        public static IApplicationBuilder UseSoa(this IApplicationBuilder app)
        {
            Console.WriteLine();
            app.UseMiddleware<SoaHttpStatusCodeExceptionMiddleware>();
            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            SoaHttpContext.Configure(httpContextAccessor);
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}");
                routes.MapControllerRoute(
                    name: "defaultApi",
                    pattern: "api/{controller}/{action}");
                routes.MapControllerRoute(
                    "SoaPath",
                    "{*path}",
                    new { controller = "SoaServices", action = "SoaPath" });
            });

            return app;
        }
        public static IServiceCollection AddSoa(this IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers(o =>
            {
                o.ModelBinderProviders.Insert(0, new SoaQueryStringModelBinderProvider());
                o.ModelBinderProviders.Insert(1, new SoaModelBinderProvider());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            return services;
        }

    }
}
