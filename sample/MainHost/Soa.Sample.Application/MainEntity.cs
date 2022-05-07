using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Soa.Sample.MainService
{
    public class MainEntity : Entity<long>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [Comment("名称")]
        public string Name { get; set; }
    }
}
