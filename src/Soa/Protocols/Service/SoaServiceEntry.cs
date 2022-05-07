using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soa.Protocols.Communication;
using Soa.Protocols.ServiceDesc;

namespace Soa.Protocols.Service
{
    public class SoaServiceEntry
    {
        /// <summary>
        ///     invoke service func
        /// </summary>
        public Func<IDictionary<string, object>, SoaPayload, Task<object>> Func { get; set; }

        /// <summary>
        ///     description for the service
        /// </summary>
        public SoaServiceDesc Descriptor { get; set; }
    }
}