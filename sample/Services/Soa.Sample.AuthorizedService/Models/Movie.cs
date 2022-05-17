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
    [AutoMapFrom(typeof(IAuthorizedService.POCOs.Movie))]
    [AutoMapTo(typeof(IAuthorizedService.POCOs.Movie))]
    public class Movie : Entity<long>, IMovie
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}
