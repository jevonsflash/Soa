using System;
using System.Threading.Tasks;
using Abp;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Soa.Client.Discovery;

namespace Soa.Client.HealthCheck
{
    public class HealthCheckJob : IAsyncBackgroundJob<HealthCheckJobArgs>, ITransientDependency
    {

        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IHealthCheck clientHealthCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDistributionJob"/> class.
        /// </summary>
        public HealthCheckJob(
            IUnitOfWorkManager unitOfWorkManager,
            IHealthCheck clientHealthCheck)
        {

            this.unitOfWorkManager = unitOfWorkManager;
            this.clientHealthCheck = clientHealthCheck;
        }

        public async Task ExecuteAsync(HealthCheckJobArgs args)
        {
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
             {

                 await clientHealthCheck?.CheckHealth(args.Timeout);

             });

        }


    }

}