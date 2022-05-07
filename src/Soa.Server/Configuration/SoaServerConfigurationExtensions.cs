using Abp.Configuration.Startup;

namespace Soa.Server.Configuration
{
    public static class SoaServerConfigurationExtensions
    {
        /// <summary>
        ///     Used to configure ABP SoaServer module.
        /// </summary>
        public static ISoaServerConfiguration SoaServer(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<ISoaServerConfiguration>();
        }
    }
}
