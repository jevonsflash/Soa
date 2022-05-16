using Abp.AutoMapper;
using Soa.GatewaySample.Authentication.External;

namespace Soa.GatewaySample.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
