using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Soa.Protocols.Attributes;
using Soa.Protocols.Service;
using Soa.Sample.IAuthorizedService.Authorization;
using Soa.Sample.IAuthorizedService.POCOs;

namespace Soa.Sample.IAuthorizedService
{
    [SoaServiceRoute("soa_api/authorized_service")]
    [SoaAuthorize(PermissionNames.Pages_AuthorizedService)]
    public interface IAuthorizedServiceManager : ISoaService
    {
        [SoaAuthorize(PermissionNames.Pages_Movie)]
        [SoaService(CreatedBy = "linxiao", Comment = "get bar")]
        public Movie GetMovie(long id);

        [SoaAuthorize(PermissionNames.Pages_Music)]
        [SoaService(CreatedBy = "linxiao", Comment = "get bar")]
        public Music GetMusic(long id);
   
    }


}
