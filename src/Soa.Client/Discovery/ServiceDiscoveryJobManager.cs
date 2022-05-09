using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soa.Client.Configuration;
using Soa.Client.Configuration.JobConfiguration;
using Soa.Hangfire;

namespace Soa.Client.Discovery
{
    public class ServiceDiscoveryJobManager : BackgroundWorkerBase, ISingletonDependency
    {
        private readonly IBackgroundJobFullFeatrueManager _backgroundJobManager;
        private readonly ISoaClientConfiguration soaClientConfiguration;

        public ServiceDiscoveryJobManager(
            IBackgroundJobFullFeatrueManager backgroundJobManager,
            ISoaClientConfiguration soaClientConfiguration

            )
        {
            _backgroundJobManager = backgroundJobManager;
            this.soaClientConfiguration = soaClientConfiguration;
        }

        public InServiceDiscoveryConfiguration Config => soaClientConfiguration.ServiceDiscovery;


        public override void Start()
        {
            base.Start();

            StartServiceDiscoveryJob();

        }
        private void StartServiceDiscoveryJob()
        {
            var cron = Config.JobCron;
            var args2 = new ServiceDiscoveryJobArgs();
            StartMaintainJob<ServiceDiscoveryJob, ServiceDiscoveryJobArgs>(args2, cron);
        }



        private void StartMaintainJob<TMaintainJob, TMaintainJobArgs>(TMaintainJobArgs args, string cron) where TMaintainJob : IBackgroundJobBase<TMaintainJobArgs>
        {

            var recurringJobs = _backgroundJobManager.GetRecurringJob();
            var currentRefundMaintainJobManager = recurringJobs.FirstOrDefault(c => c.Job != null && c.Job.Type == typeof(TMaintainJob));

            if (currentRefundMaintainJobManager != null)
            {
                _backgroundJobManager.DeleteRecurringJob(currentRefundMaintainJobManager.Id);

            }

            if (Config.Enable)
            {
                _backgroundJobManager.AddRecurring<TMaintainJob, TMaintainJobArgs>(args, cron: cron);

            }


        }

        private void StopMaintainJob<TMaintainJob>()
        {

            var recurringJobs = _backgroundJobManager.GetRecurringJob();
            var currentRefundMaintainJobManager = recurringJobs.FirstOrDefault(c => c.Job != null && c.Job.Type == typeof(TMaintainJob));
            if (currentRefundMaintainJobManager != null)
            {
                _backgroundJobManager.DeleteRecurringJob(currentRefundMaintainJobManager.Id);
            }
        }
        public override void Stop()
        {
            base.Stop();
            StopMaintainJob<ServiceDiscoveryJob>();
        }

        public override void WaitToStop()
        {
            base.WaitToStop();
        }
    }
}
