# Soa Sample

## 示例说明

1. SimpleRpcSample：一个简单的面向服务体系架构的示例程序。使用微服务Service1， Service2.
2. GatewaySample：一个通过网关转发请求到服务的示例程序，包含登录鉴权，使用微服务Service1， Service2，AuthorizedService

## 示例帮助

1. 运行网关和微服务前请确保数据库脚本已经运行
2. 运行 SimpleRpcSample 以及微服务Service1， Service2，只需要跑sql脚本sampledb.sql
3. 运行 GatewaySample 需要运行Ef Update-Database命令 
~~之后运行 sql脚本gateway_sampledb.sql~~
4. ~~运行 AuthorizedService 需要运行Ef Update-Database命令 之后运行 sql脚本authorized_service_db.sql~~
    ![img](https://raw.githubusercontent.com/MatoApps/Soa/master/SOA/Screenshot_12.png)
5. 完成数据库配置后，启动微服务：  

    Soa.Sample.AuthorizedService.Host

    Soa.Sample.Service2.Host

    Soa.Sample.Service1.Host

    之后启动网关或者客户端：

    Soa.Sample.Web
    或
    Soa.GatewaySample.Web.Host

    可以使用解决方案属性页面“多个启动项目”来启动

    ![img](https://raw.githubusercontent.com/MatoApps/Soa/master/SOA/Screenshot_13.png)

## 已知问题


## 作者信息

作者：林小

邮箱：jevonsflash@qq.com



## License

The MIT License (MIT)
