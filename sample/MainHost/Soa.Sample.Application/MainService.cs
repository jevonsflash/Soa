using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Soa.Sample.IService1;
using Soa.Sample.IService2;
using Soa.Client.Proxy;
using Soa.Sample.Dto;

//using Soa.Sample.Service1;
//using Soa.Sample.Service2;

namespace Soa.Sample.MainService.Application.Main
{

    //[AbpAuthorize(PermissionNames.Pages_Main)]
    public class MainService : AsyncCrudAppService<MainEntity, MainEntityDto, long>
    {
        private IService1Manager _service1Manager;
        private IService2Manager _service2Manager;

        //若是使用方式1， 在注入仓储对象的时候，势必直接引用了这两个服务的领域层，造成了耦合
        //private readonly IRepository<Entity1, long> _entity1Repository;
        //private readonly IRepository<Entity2, long> _entity2Repository;

        //若是使用方式2， 在注入领域服务（Manager）时，也势必直接引用了这两个服务的领域层，造成了项目之间的耦合
        //private readonly Service2Manager _service2Manager;
        //private readonly Service1Manager _service1Manager;

        public MainService(
            IRepository<MainEntity, long> repository
            , IServiceProxy serviceProxy
            //Service2Manager service2Manager,
            //Service1Manager service1Manager

            //IRepository<Entity1, long> entity1repository,
            //IRepository<Entity2, long> entity2repository
        ) : base(repository)
        {
            //_service2Manager = service2Manager;
            //_service1Manager = service1Manager;
            //_entity1Repository = entity1repository;
            //_entity2Repository = entity2repository;
            _service1Manager = serviceProxy.GetService<IService1Manager>();
            _service2Manager = serviceProxy.GetService<IService2Manager>();
        }

        /// <summary>
        /// 方式1: 模拟传统方式在应用层获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public MainEntityDto GetExtends(EntityDto<long> input)
        //{
        //    //1. 使用用两个服务的仓储对象， 分别查询到对应的数据
        //    var entity1 = this._entity1Repository.FirstOrDefault(c => c.MainId == input.Id);
        //    var entity2 = this._entity2Repository.FirstOrDefault(c => c.MainId == input.Id);

        //    //2. 再从MainService的仓储对象中查询数据，并拼装成Dto返回
        //    var result = MapToEntityDto(Repository.FirstOrDefault(c => c.Id == input.Id));
        //    result.Type = entity1.Type;
        //    result.Num = entity2.Num;
        //    return result;
        //}


        /// <summary>
        /// 方式2: 模拟传统方式在领域层获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public MainEntityDto GetExtends(EntityDto<long> input)
        //{
        //    //1. 调用两个领域服务（Manager）中的方法获取返回的数据
        //    var type = this._service1Manager.GetType(input.Id);
        //    var num = this._service2Manager.GetNum(input.Id);

        //    //2. 再从MainService的仓储对象中查询数据，并拼装成Dto返回
        //    var result = MapToEntityDto(Repository.FirstOrDefault(c => c.Id == input.Id));
        //    result.Type = type;
        //    result.Num = num;
        //    return result;
        //}

        /// <summary>
        /// 微服务方式：通过调用领域层代理对象获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MainEntityDto GetExtends(EntityDto<long> input)
        {
            //1. 调用两个代理服务中的方法获取返回的数据
            var type = this._service1Manager.GetType(input.Id);
            var num = this._service2Manager.GetNum(input.Id);

            //2. 再从MainService的仓储对象中查询数据，并拼装成Dto返回
            var result = MapToEntityDto(Repository.FirstOrDefault(c => c.Id == input.Id));
            result.Type = type;
            result.Num = num;
            return result;
        }
    }
}
