using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Soa.Sample.Service2
{
    public class Entity2 : Entity<long>
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [Comment("名称")]
        public string Name { get; set; }

        [Comment("个数")]
        public int Num { get; set; }

        [Comment("外键Id")]
        public long MainId { get; set; }


    }
}
