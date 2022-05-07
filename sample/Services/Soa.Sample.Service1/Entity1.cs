using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Soa.Sample.Service1
{
    public class Entity1 : Entity<long>
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [Comment("名称")]
        public string Name { get; set; }

        [Comment("类型")]
        public string Type { get; set; }

        [Comment("外键Id")]
        public long MainId { get; set; }



    }
}
