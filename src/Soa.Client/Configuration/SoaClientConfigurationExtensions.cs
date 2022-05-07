using Abp.Configuration.Startup;

namespace Soa.Client.Configuration
{
    public static class SoaClientConfigurationExtensions
    {
        /// <summary>
        ///     Used to configure ABP SoaClient module.
        /// </summary>
        public static ISoaClientConfiguration SoaClient(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<ISoaClientConfiguration>();
        }
    }
}
