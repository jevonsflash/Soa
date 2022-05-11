using Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Controllers;
using Soa.Protocols.Service;
using System;
using System.Reflection;

namespace Soa.Client
{
    public class SoaControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IIocResolver _iocResolver;

        public SoaControllerFeatureProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            Type type = typeInfo.AsType();
            if (!typeof(ISoaService).IsAssignableFrom(type))
            {
                return false;
            }
            return true;
   
        }
    }
}
