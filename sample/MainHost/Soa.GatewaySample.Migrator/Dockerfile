#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/MainHost/Soa.GatewaySample.Migrator/Soa.GatewaySample.Migrator.csproj", "sample/MainHost/Soa.GatewaySample.Migrator/"]
COPY ["sample/MainHost/Soa.GatewaySample.Core/Soa.GatewaySample.Core.csproj", "sample/MainHost/Soa.GatewaySample.Core/"]
COPY ["sample/Services.Abstract/Soa.Sample.IService2/Soa.Sample.IService2.csproj", "sample/Services.Abstract/Soa.Sample.IService2/"]
COPY ["src/Soa/Soa.csproj", "src/Soa/"]
COPY ["sample/Services.Abstract/Soa.Sample.IAuthorizedService/Soa.Sample.IAuthorizedService.csproj", "sample/Services.Abstract/Soa.Sample.IAuthorizedService/"]
COPY ["sample/Services.Abstract/Soa.Sample.IService1/Soa.Sample.IService1.csproj", "sample/Services.Abstract/Soa.Sample.IService1/"]
COPY ["sample/MainHost/Soa.GatewaySample.EntityFrameworkCore/Soa.GatewaySample.EntityFrameworkCore.csproj", "sample/MainHost/Soa.GatewaySample.EntityFrameworkCore/"]
RUN dotnet restore "sample/MainHost/Soa.GatewaySample.Migrator/Soa.GatewaySample.Migrator.csproj"
COPY . .
WORKDIR "/src/sample/MainHost/Soa.GatewaySample.Migrator"
RUN dotnet build "Soa.GatewaySample.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Soa.GatewaySample.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Soa.GatewaySample.Migrator.dll","-q"]