using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Soa.GatewaySample.Configuration;

namespace Soa.GatewaySample.Web.Host.Startup
{
    [DependsOn(
       typeof(GatewaySampleWebCoreModule))]
    public class GatewaySampleWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public GatewaySampleWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GatewaySampleWebHostModule).GetAssembly());
        }
    }
}
