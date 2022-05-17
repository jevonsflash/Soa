using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Soa.Sample.AuthorizedService.Models;
using Soa.Sample.IAuthorizedService;

namespace Soa.Sample.AuthorizedService
{
    public class AuthorizedServiceManager : DomainService, IAuthorizedServiceManager
    {
        private readonly IRepository<Movie, long> _movieRepository;
        private readonly IRepository<Music, long> _musicRepository;

        public AuthorizedServiceManager(
            IRepository<Movie, long> movierepository,
            IRepository<Music, long> musicRepository)
        {
            _movieRepository = movierepository;
            _musicRepository=musicRepository;
        }

        public IAuthorizedService.POCOs.Movie GetMovie(long id)
        {
            var movie = this._movieRepository.FirstOrDefault(c => c.Id == id);
            return ObjectMapper.Map<IAuthorizedService.POCOs.Movie>(movie);
        }

        public IAuthorizedService.POCOs.Music GetMusic(long id)
        {
            var movie = this._musicRepository.FirstOrDefault(c => c.Id == id);
            return ObjectMapper.Map<IAuthorizedService.POCOs.Music>(movie);
        }

    }
}
