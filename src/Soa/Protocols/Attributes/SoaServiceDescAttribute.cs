using System;
using Soa.Protocols.ServiceDesc;

namespace Soa.Protocols.Attributes
{
    public abstract class SoaServiceDescAttribute : Attribute
    {
        public abstract void Apply(SoaServiceDesc descriptor);
    }
}