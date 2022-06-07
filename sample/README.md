# Soa Sample

## 示例说明

1. SimpleRpcSample：一个简单的面向服务体系架构的示例程序。使用微服务Service1， Service2.
2. GatewaySample：一个通过网关转发请求到服务的示例程序，包含登录鉴权，使用微服务Service1， Service2，AuthorizedService
3. 运行项目前请先安装好SQL server服务，SQL server的安装请参考[官方文档](https://docs.microsoft.com/zh-cn/sql/database-engine/install-windows/install-sql-server)

## 示例帮助

### 本机部署

1. 运行 SimpleRpcSample 以及微服务Service1， Service2，需要运行`sampledb.sql`以生成初始数据

2. 运行 GatewaySample 需要运行`Soa.GatewaySample.Migrator`迁移程序
```
cd \Soa\sample\MainHost\Soa.GatewaySample.Migrator
dotnet run
``` 

3. 完成数据库配置后，启动微服务：  

    Soa.Sample.AuthorizedService.Host

    Soa.Sample.Service2.Host

    Soa.Sample.Service1.Host

4. 之后启动网关或者客户端：

    Soa.Sample.Web
    或
    Soa.GatewaySample.Web.Host

    可以使用解决方案属性页面“多个启动项目”来启动

    ![img](https://raw.githubusercontent.com/MatoApps/Soa/master/SOA/Screenshot_13.png)

### Docker容器部署

1. 运行微服务`Soa.Sample.AuthorizedService.Host`
```
docker run -p 8009:8009  --net="host" jevonsflash/soasampleauthorizedservicehost:latest
```
2. 运行`Soa.GatewaySample.Migrator`迁移程序
```
docker run jevonsflash/soagatewaysamplemigrator:latest
```
3. 之后启动网关
```
docker run -p 44311:44311 --net="host"  jevonsflash/soagatewaysamplewebhost:latest
```
若要更改数据库，请在docker run之后添加参数:
```
-e ConnectionStrings:Default_Docker="[你的数据库链接字符串]"
```
若要改变host地址，请在docker run之后添加参数
```
-e App:ServerRootAddress="http://0.0.0.0:44311/"
```
详细说明请见[Docker Hub](https://hub.docker.com/u/jevonsflash)

## 已知问题


## 作者信息

作者：林小

邮箱：jevonsflash@qq.com



## License

The MIT License (MIT)
