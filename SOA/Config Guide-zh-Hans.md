# 配置说明
## 网关（客户端）
SoaClient

字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Address    |    数组          | 服务地址列表
Transport    |    string          | 传输方式(Http或DotNetty)
AssemblyNames    |    string          | 服务依赖的程序集名称，以逗号分隔
HealthCheck    |    块内容          | 健康监测配置
Discovery    |    string          | 服务发现方式(InServer或者Consul)
InServiceDiscovery    |    块内容          | 本机服务发现配置
ConsulServiceDiscovery    |    块内容          | Consul服务发现配置

Address

字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Ip    |    string          | Ip地址
Port    |    string          | 端口号

HealthCheck

字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Enable    |    bool          | 是否启用
JobCron    |    string          | 定时任务Cron表达式
Timeout    |    string          | 监测超时

InServiceDiscovery
字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Enable    |    bool          | 是否启用
JobCron    |    string          | 定时任务Cron表达式


ConsulServiceDiscovery
字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Ip    |    string          | Consul服务Ip地址
Port    |    string          | Consul服务端口号

示例
```
"SoaClient": {
    "Address": [
      {
        "Ip": "127.0.0.1",
        "Port": "8007"
      },
      {
        "Ip": "127.0.0.1",
        "Port": "8008"
      },
      {
        "Ip": "127.0.0.1",
        "Port": "8009"
      }
    ],
    "Transport": "Http",
    "AssemblyNames": "Soa.Sample.IService1,Soa.Sample.IService2,Soa.Sample.IAuthorizedService",
    "HealthCheck": {
      "Enable": true,
      "JobCron": "0/5 * * * * ? ",
      "Timeout": 3000
    },
    "Discovery": "InServer",

    "InServiceDiscovery": {
      "Enable": true,
      "JobCron": "0 * * * * ? "
    },
    "ConsulServiceDiscovery": {
      "Ip": "127.0.0.1",
      "Port": "8500"
    }

  }

```


## 微服务（服务端）

SoaServer

字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Name    |    string          | 名称
Ip    |    string          | Ip地址
Port    |    string          | 端口号
Transport    |    string          | 传输方式(Http或DotNetty)
AssemblyNames    |    string          | 服务依赖的程序集名称，以逗号分隔
Discovery    |    string          | 服务发现方式(InServer或者Consul)
ConsulServiceDiscovery    |    块内容          | Consul服务发现配置


ConsulServiceDiscovery
字段 | 值类型 | 含义 
:----------: | :-----------: | :-----------
Ip    |    string          | Consul服务Ip地址
Port    |    string          | Consul服务端口号

示例
```
{
  "ConnectionStrings": {
    "Default": "Server=localhost; Database=AuthorizedServiceDb; Trusted_Connection=True;"

  },
  "SoaServer": {
    "Name": "AuthorizedService",
    "Ip": "127.0.0.1",
    "Port": "8009",
    "Transport": "Http",
    "AssemblyNames": "Soa.Sample.IAuthorizedService,Soa.Sample.AuthorizedService",
    "Discovery": "InServer",
    "ConsulServiceDiscovery": {
      "Ip": "127.0.0.1",
      "Port": "8500"
    }
  }
}

```