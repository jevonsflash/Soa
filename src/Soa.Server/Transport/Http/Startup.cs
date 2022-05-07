using System;
using System.Collections.Generic;
using Abp.Dependency;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soa.Serializer;
using Soa.Server.ServiceContainer;
using Soa.Server.TransportServer;
using Soa.TypeConverter;

namespace Soa.Server.Transport.Http
{
    public class Startup : IStartup
    {
        readonly Stack<Func<RequestDel, RequestDel>> _middlewares;
        private readonly IServiceEntryContainer _serviceEntryContainer;
        private readonly ISerializer _serializer;
        private readonly ITypeConvertProvider _typeConvert;
        public Startup(IConfiguration configuration, Stack<Func<RequestDel, RequestDel>> middlewares, IServiceEntryContainer serviceEntryContainer,  ISerializer serializer, ITypeConvertProvider typeConvert)
        {
            Configuration = configuration;
            _middlewares = middlewares;
            _serviceEntryContainer = serviceEntryContainer;
            _serializer = serializer;
            _typeConvert = typeConvert;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.EnableEndpointRouting = false; });
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.UseMiddleware<HttpMiddleware>(_middlewares, _serviceEntryContainer, _serializer, _typeConvert);
        }

    }
}
