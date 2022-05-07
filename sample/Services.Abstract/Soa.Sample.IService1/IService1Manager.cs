using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soa.Protocols.Attributes;
using Soa.Protocols.Service;

namespace Soa.Sample.IService1
{
    [SoaServiceRoute("soa_api/service1")]
    public interface IService1Manager : ISoaService
    {
        [SoaService(CreatedBy = "linxiao", Comment = "get type by main id")]
        public string GetType(long mainId);

    }
}
