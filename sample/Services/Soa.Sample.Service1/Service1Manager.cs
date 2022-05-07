using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Soa.Sample.IService1;

namespace Soa.Sample.Service1
{
    public class Service1Manager : DomainService , IService1Manager
    {
        private readonly IRepository<Entity1, long> _entity1Repository;

        public Service1Manager(
            IRepository<Entity1, long> entity1repository)
        {
            _entity1Repository = entity1repository;
        }

        public string GetType(long mainId)
        {
            var entity1 = this._entity1Repository.FirstOrDefault(c => c.MainId == mainId);
            return entity1.Type;
        }
    }
}
