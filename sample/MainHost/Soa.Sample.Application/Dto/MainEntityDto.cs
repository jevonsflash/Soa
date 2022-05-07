using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Soa.Sample.MainService;

namespace Soa.Sample.Dto
{
    [AutoMapFrom(typeof(MainEntity))]
    [AutoMapTo(typeof(MainEntity))]
    public class MainEntityDto : EntityDto<long>

    {

        public string Name { get; set; }

        public string Type { get; set; }
        public int Num { get; set; }

    }
}
