using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Soa.Sample.IService2;

namespace Soa.Sample.Service2
{
    public class Service2Manager : DomainService, IService2Manager
    {
        private readonly IRepository<Entity2, long> _entity2Repository;

        public Service2Manager(
            IRepository<Entity2, long> entity2repository)
        {
            _entity2Repository = entity2repository;
        }

        public int GetNum(long mainId)
        {
            var entity2 = this._entity2Repository.FirstOrDefault(c => c.MainId == mainId);
            return entity2.Num;
        }
    }
}
