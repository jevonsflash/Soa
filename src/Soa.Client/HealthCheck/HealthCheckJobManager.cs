using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Soa.Client.Configuration;
using Soa.Client.Configuration.JobConfiguration;
using Soa.Hangfire;
using System.Linq;

namespace Soa.Client.HealthCheck
{
    public class HealthCheckJobManager : BackgroundWorkerBase, ISingletonDependency
    {
        private readonly IBackgroundJobFullFeatrueManager _backgroundJobManager;
        private readonly ISoaClientConfiguration soaClientConfiguration;

        public HealthCheckJobManager(
            IBackgroundJobFullFeatrueManager backgroundJobManager,
            ISoaClientConfiguration soaClientConfiguration
            )
        {
            _backgroundJobManager = backgroundJobManager;
            this.soaClientConfiguration = soaClientConfiguration;
        }

        public HealthCheckConfiguration Config => soaClientConfiguration.HealthCheck;
       
        public override void Start()
        {
            base.Start();

            StartHealthCheckJob();

        }
        private void StartHealthCheckJob()
        {
            var cron = Config.JobCron;
            var args2 = new HealthCheckJobArgs();
            args2.Timeout = Config.Timeout;
            StartMaintainJob<HealthCheckJob, HealthCheckJobArgs>(args2, cron);
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
            StopMaintainJob<HealthCheckJob>();
        }

        public override void WaitToStop()
        {
            base.WaitToStop();
        }
    }
}
