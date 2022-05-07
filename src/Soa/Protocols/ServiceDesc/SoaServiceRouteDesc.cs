using System.Collections.Generic;

namespace Soa.Protocols.ServiceDesc
{
    public class SoaServiceRouteDesc
    {
        public IEnumerable<SoaAddressDesc> AddressDescriptors { get; set; }

        public SoaServiceDesc ServiceDescriptor { get; set; }
    }
}