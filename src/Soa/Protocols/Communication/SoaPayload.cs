using System.Collections.Generic;

namespace Soa.Protocols.Communication
{
    /// <summary>
    ///     store client meta info when using jwt to authorize the client
    /// </summary>
    public class SoaPayload
    {
        public IDictionary<string, object> Items { get; set; }
    }
}
