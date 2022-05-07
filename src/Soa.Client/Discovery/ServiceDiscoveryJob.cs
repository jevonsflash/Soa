using System;
using System.Threading.Tasks;
using Abp;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;

namespace Soa.Client.Discovery
{
    public class ServiceDiscoveryJob : IAsyncBackgroundJob<ServiceDiscoveryJobArgs>, ITransientDependency
    {

        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IClientServiceDiscovery clientServiceDiscovery;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDistributionJob"/> class.
        /// </summary>
        public ServiceDiscoveryJob(IUnitOfWorkManager unitOfWorkManager, IClientServiceDiscovery clientServiceDiscovery)
        {

            this.unitOfWorkManager = unitOfWorkManager;
            this.clientServiceDiscovery = clientServiceDiscovery;
        }

        public async Task ExecuteAsync(ServiceDiscoveryJobArgs args)
        {
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
             {
                 clientServiceDiscovery?.UpdateRoutes();



             });

        }
    }

}