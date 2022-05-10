# Soa
## 介绍

一个轻量级的微服务库，基于.Net 6 + Abp框架 可快速地将现有项目改造成为面向服务体系结构，实现模块间松耦合。


## 感谢

RabbitTeam 的项目 [RabbitCloud](https://github.com/RabbitTeam/RabbitCloud)

grissomlau 的项目 [jimu](https://github.com/grissomlau/jimu)

部分模块以及算法代码参考自以上项目

## 特点：

* 支持DotNetty和Http两种模式的RPC
* 支持自动路由发现注册与微服务健康监测
* 支持模块以及模块的依赖关系
* 支持简单对象(POCO)作为参数或返回类型
* 支持多语言/本地化

## 内容：

* 基于Roslyn的动态客户代理类(Proxy模块)
* POCO对象传输编解码(TypeConverter模块)
* 基于DotNetty或者HTTP的RPC(Transport模块)
* 路由服务发现(ServiceDiscovery模块)
* 健康监测(HealthCheck模块)
* 基于swagger的Api文档生成
* 基于Attribute注解的路由配置
* 基于Json配置文件的系统配置(Abp实现)
* 基于Hangfire的计划任务
* 基于Castle Windsor的Ioc(Abp实现)
* 基于Log4Net的日志(Abp实现)
* 基于EF，并实现模型的Repository仓储模式(Abp实现)

## 快速开始


Program.cs

```
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSoa<YourServiceHostModel>();
var webapp = builder.Build();
webapp.UseSoa();
webapp.Run();
```

YourServiceHostModel.cs:

```
[DependsOn(typeof(SoaServerModule))]
public class Service1HostModel : AbpModule
{
    //Your code
}
```
完整示例请参考[Sample](https://github.com/MatoApps/Soa/tree/master/sample)
,配置说明式请阅读系列博客

系列博客

1. [使用Soa库+Abp搭建微服务项目框架（一）：Abp与DDD相关知识回顾](https://blog.csdn.net/jevonsflash/article/details/120830747)
2. [使用Soa库+Abp搭建微服务项目框架（二）：面向服务体系的介绍](https://blog.csdn.net/jevonsflash/article/details/120841700)
3. [使用Soa库+Abp搭建微服务项目框架（三）：项目改造](https://blog.csdn.net/jevonsflash/article/details/120839802)
4. [使用Soa库+Abp搭建微服务项目框架（四）：动态代理和RPC](https://blog.csdn.net/jevonsflash/article/details/120850141)
5. [使用Soa库+Abp搭建微服务项目框架（五）：服务发现和健康监测](https://blog.csdn.net/jevonsflash/article/details/124668465)