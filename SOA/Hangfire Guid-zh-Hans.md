# 定时任务
### 网关（客户端） GatewaySample

GatewaySampleCoreModule.cs 文件

添加包
* Abp.Hangfire 
* Hangfire.SqlServer



引用命名空间

```
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Hangfire;
using Soa.Configuration;
using Soa.Hangfire;
```
添加AbpHangfireModule模块依赖
```
[DependsOn(typeof(AbpHangfireModule))]
public class GatewaySampleCoreModule : AbpModule
{
  //Your code.

  IocManager.Register<IBackgroundJobFullFeatrueManager, HangfireBackgroundJobFullFeatrueManager>(DependencyLifeStyle.Singleton);
}

```

配置UseHangfire

并将IBackgroundJobFullFeatrueManager添加至Ioc
```
public override void PostInitialize()
{

  //Your code.

  Configuration.BackgroundJobs.UseHangfire<HangfireBackgroundJobFullFeatrueManager>(configuration =>
  {

      (configuration as AbpHangfireConfiguration).GlobalConfiguration.UseSqlServerStorage(Configuration.DefaultNameOrConnectionString);
  });
  var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
  workerManager.Add(IocManager.Resolve<IBackgroundJobFullFeatrueManager>());

}
```