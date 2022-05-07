using Abp.BackgroundJobs;
using Abp.Configuration.Startup;
using Abp.Hangfire.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soa.Configuration
{
    public static class HangfireConfigurationExtensions
    {
        public static void UseHangfire<T>(this IBackgroundJobConfiguration backgroundJobConfiguration, Action<IAbpHangfireConfiguration> configureAction) where T : class, IBackgroundJobManager
        {
            backgroundJobConfiguration.AbpConfiguration.ReplaceService<IBackgroundJobManager, T>();
            configureAction(backgroundJobConfiguration.AbpConfiguration.Modules.AbpHangfire());
        }
    }
}
