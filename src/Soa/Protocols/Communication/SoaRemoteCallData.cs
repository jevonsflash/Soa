using System.Collections.Generic;
using Soa.Protocols.ServiceDesc;

namespace Soa.Protocols.Communication
{
    /// <summary>
    ///     like json-RPC to specify info to invoke remote server method(service)
    /// </summary>
    public class SoaRemoteCallData
    {
        public string ServiceId { get; set; }

        public SoaPayload Payload { get; set; }

        public string Token { get; set; }
        public SoaServiceDesc Descriptor { get; set; }

        public IDictionary<string, object> Parameters { get; set; }
    }

}