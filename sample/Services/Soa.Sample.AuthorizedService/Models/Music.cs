using Abp.AutoMapper;
using Abp.Domain.Entities;
using Soa.Sample.IAuthorizedService.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soa.Sample.AuthorizedService.Models
{
    [AutoMapFrom(typeof(IAuthorizedService.POCOs.Music))]
    [AutoMapTo(typeof(IAuthorizedService.POCOs.Music))]
    public class Music : Entity<long>, IMusic
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
