using System.Collections.Generic;
using Abp.BackgroundJobs;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;

namespace Soa.Hangfire
{
    public interface IBackgroundJobFullFeatrueManager : IBackgroundJobManager
    {
        public void AddRecurring<TJob, TArgs>(TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            string cron = null) where TJob : IBackgroundJobBase<TArgs>;

        public List<RecurringJobDto> GetRecurringJob();
        public void DeleteRecurringJob(string id);
        public void AppedToExistJob<TJob, TArgs>(string id, TArgs args) where TJob : IBackgroundJobBase<TArgs>;

        public Dictionary<string, ScheduledJobDto> GetScheduledJob();
        public Dictionary<string, DeletedJobDto> GetDeletedJob();
        public Dictionary<string, EnqueuedJobDto> GetEnqueuedJob();
        public Dictionary<string, FailedJobDto> GetFailedJob();
        public Dictionary<string, SucceededJobDto> GetSucceededJob();
        public Dictionary<string, ProcessingJobDto> GetProcessingJob();

    }
}