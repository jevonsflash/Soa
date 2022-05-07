using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Soa.Sample.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soa.Server;
using Soa.Sample.EntityFrameworkCore;

namespace Soa.Sample.Service2.Host
{
    [DependsOn(typeof(SampleEntityFrameworkCoreModule))]
    [DependsOn(typeof(Service2CoreModel))]
    [DependsOn(typeof(SoaServerModule))]
    public class Service2HostModel : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Service2HostModel()
        {
            _appConfiguration = AppConfigurations.Get(
                typeof(Service2HostModel).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            //配置数据库
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                "Default"
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
           
        }

        public override void Initialize()
        {
            //注册当前程序集到Ioc容器
            IocManager.RegisterAssemblyByConvention(typeof(Service2HostModel).GetAssembly());         
        }
    }
}
