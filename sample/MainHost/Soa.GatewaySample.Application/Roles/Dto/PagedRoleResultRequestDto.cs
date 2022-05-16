using Abp.Application.Services.Dto;

namespace Soa.GatewaySample.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

