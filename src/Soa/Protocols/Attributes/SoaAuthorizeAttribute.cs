using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soa.Protocols.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SoaAuthorizeAttribute : AbpAuthorizeAttribute
    {
        public SoaAuthorizeAttribute(params string[] permissions) : base(permissions)
        {

        }
    }
}
