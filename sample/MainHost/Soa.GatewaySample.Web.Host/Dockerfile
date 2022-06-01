#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 44311

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/MainHost/Soa.GatewaySample.Web.Host/Soa.GatewaySample.Web.Host.csproj", "sample/MainHost/Soa.GatewaySample.Web.Host/"]
COPY ["sample/MainHost/Soa.GatewaySample.Web.Core/Soa.GatewaySample.Web.Core.csproj", "sample/MainHost/Soa.GatewaySample.Web.Core/"]
COPY ["sample/MainHost/Soa.GatewaySample.Application/Soa.GatewaySample.Application.csproj", "sample/MainHost/Soa.GatewaySample.Application/"]
COPY ["sample/MainHost/Soa.GatewaySample.Core/Soa.GatewaySample.Core.csproj", "sample/MainHost/Soa.GatewaySample.Core/"]
COPY ["sample/Services.Abstract/Soa.Sample.IService2/Soa.Sample.IService2.csproj", "sample/Services.Abstract/Soa.Sample.IService2/"]
COPY ["src/Soa/Soa.csproj", "src/Soa/"]
COPY ["sample/Services.Abstract/Soa.Sample.IAuthorizedService/Soa.Sample.IAuthorizedService.csproj", "sample/Services.Abstract/Soa.Sample.IAuthorizedService/"]
COPY ["sample/Services.Abstract/Soa.Sample.IService1/Soa.Sample.IService1.csproj", "sample/Services.Abstract/Soa.Sample.IService1/"]
COPY ["sample/MainHost/Soa.GatewaySample.EntityFrameworkCore/Soa.GatewaySample.EntityFrameworkCore.csproj", "sample/MainHost/Soa.GatewaySample.EntityFrameworkCore/"]
COPY ["src/Soa.Client/Soa.Client.csproj", "src/Soa.Client/"]
RUN dotnet restore "sample/MainHost/Soa.GatewaySample.Web.Host/Soa.GatewaySample.Web.Host.csproj"
COPY . .
WORKDIR "/src/sample/MainHost/Soa.GatewaySample.Web.Host"
RUN dotnet build "Soa.GatewaySample.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Soa.GatewaySample.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Soa.GatewaySample.Web.Host.dll"]