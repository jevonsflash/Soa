using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.ObjectMapping;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using HangfireRecurringJob = Hangfire.RecurringJob;
using HangfireBackgrounJob = Hangfire.BackgroundJob;

namespace Soa.Hangfire
{
    public class HangfireBackgroundJobFullFeatrueManager : HangfireBackgroundJobManager, IBackgroundJobFullFeatrueManager
    {

        public HangfireBackgroundJobFullFeatrueManager(
            IBackgroundJobConfiguration backgroundJobConfiguration,
            IAbpHangfireConfiguration hangfireConfiguration) : base(backgroundJobConfiguration, hangfireConfiguration)
        {
        }
        public IStorageConnection CurrentConnection => JobStorage.Current.GetConnection();

        public virtual void AddRecurring<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
         string cron = null) where TJob : IBackgroundJobBase<TArgs>
        {
            var id = Guid.NewGuid().ToString("N");
            var name = $"{typeof(TJob).Name}_{id}";

            if (string.IsNullOrEmpty(cron))
            {
                cron = Cron.Daily();
            }
            if (typeof(IBackgroundJob<TArgs>).IsAssignableFrom(typeof(TArgs)))
            {
                HangfireRecurringJob.AddOrUpdate<TJob>(name, job => ((IBackgroundJob<TArgs>)job).Execute(args), cron, TimeZoneInfo.Local);
            }
            else
            {
                HangfireRecurringJob.AddOrUpdate<TJob>(name, job => ((IAsyncBackgroundJob<TArgs>)job).ExecuteAsync(args), cron, TimeZoneInfo.Local);
            }

        }

        public virtual void DeleteRecurringJob(string id)
        {
            HangfireRecurringJob.RemoveIfExists(id);
        }

        public virtual void AppedToExistJob<TJob, TArgs>(string id, TArgs args) where TJob : IBackgroundJobBase<TArgs>
        {
            HangfireBackgrounJob.ContinueJobWith<TJob>(
                id,
                job => ((IBackgroundJob<TArgs>)job).Execute(args));
        }

        public virtual List<RecurringJobDto> GetRecurringJob()
        {
            var resultDtos = CurrentConnection.GetRecurringJobs();
            return resultDtos;
        }

        public virtual Dictionary<string, ScheduledJobDto> GetScheduledJob()
        {
            var mo = JobStorage.Current.GetMonitoringApi();
            Dictionary<string, ScheduledJobDto> result = null;

            var sj = mo.ScheduledJobs(0, (int)mo.ScheduledCount());
            result = new Dictionary<string, ScheduledJobDto>(sj.Select(c => new KeyValuePair<string, ScheduledJobDto>(c.Key, c.Value)));
            return result;
        }

        public virtual Dictionary<string, DeletedJobDto> GetDeletedJob()
        {
            var mo = JobStorage.Current.GetMonitoringApi();
            Dictionary<string, DeletedJobDto> result = null;

            var sj = mo.DeletedJobs(0, (int)mo.DeletedListCount());
            result = new Dictionary<string, DeletedJobDto>(sj.Select(c => new KeyValuePair<string, DeletedJobDto>(c.Key, c.Value)));
            return result;
        }

        public virtual Dictionary<string, EnqueuedJobDto> GetEnqueuedJob()
        {
            var mo = JobStorage.Current.GetMonitoringApi();
            Dictionary<string, EnqueuedJobDto> result = null;

            var sj = mo.EnqueuedJobs("default", 0, (int)mo.EnqueuedCount("default"));
            result = new Dictionary<string, EnqueuedJobDto>(sj.Select(c => new KeyValuePair<string, EnqueuedJobDto>(c.Key, c.Value)));
            return result;
        }

        public virtual Dictionary<string, FailedJobDto> GetFailedJob()
        {
            var mo = JobStorage.Current.GetMonitoringApi();
            Dictionary<string, FailedJobDto> result = null;

            var sj = mo.FailedJobs(0, (int)mo.FailedCount());
            result = new Dictionary<string, FailedJobDto>(sj.Select(c => new KeyValuePair<string, FailedJobDto>(c.Key, c.Value)));
            return result;
        }
        public virtual Dictionary<string, SucceededJobDto> GetSucceededJob()
        {
            var mo = JobStorage.Current.GetMonitoringApi();
            Dictionary<string, SucceededJobDto> result = null;

            var sj = mo.SucceededJobs(0, (int)mo.SucceededListCount());
            result = new Dictionary<string, SucceededJobDto>(sj.Select(c => new KeyValuePair<string, SucceededJobDto>(c.Key, c.Value)));
            return result;
        }
        public virtual Dictionary<string, ProcessingJobDto> GetProcessingJob()
        {
            var mo = JobStorage.Current.GetMonitoringApi();
            Dictionary<string, ProcessingJobDto> result = null;

            var sj = mo.ProcessingJobs(0, (int)mo.ProcessingCount());
            result = new Dictionary<string, ProcessingJobDto>(sj.Select(c => new KeyValuePair<string, ProcessingJobDto>(c.Key, c.Value)));
            return result;
        }


    }
}
