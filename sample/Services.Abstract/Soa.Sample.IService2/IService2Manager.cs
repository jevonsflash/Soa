using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soa.Protocols.Attributes;
using Soa.Protocols.Service;

namespace Soa.Sample.IService2
{
    [SoaServiceRoute("soa_api/service2")]
    public interface IService2Manager : ISoaService
    {
        [SoaService(CreatedBy = "linxiao", Comment = "get num by main id", EnableAuthorization = true)]
        public int GetNum(long mainId);

    }
}
