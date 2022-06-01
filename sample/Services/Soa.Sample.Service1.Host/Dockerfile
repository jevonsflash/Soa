#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 8007


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/Services/Soa.Sample.Service1.Host/Soa.Sample.Service1.Host.csproj", "sample/Services/Soa.Sample.Service1.Host/"]
COPY ["sample/MainHost/Soa.Sample.EntityFrameworkCore/Soa.Sample.EntityFrameworkCore.csproj", "sample/MainHost/Soa.Sample.EntityFrameworkCore/"]
COPY ["sample/MainHost/Soa.Sample.Application/Soa.Sample.Application.csproj", "sample/MainHost/Soa.Sample.Application/"]
COPY ["sample/Services.Abstract/Soa.Sample.IService2/Soa.Sample.IService2.csproj", "sample/Services.Abstract/Soa.Sample.IService2/"]
COPY ["src/Soa/Soa.csproj", "src/Soa/"]
COPY ["src/Soa.Client/Soa.Client.csproj", "src/Soa.Client/"]
COPY ["sample/MainHost/Soa.Sample.Core/Soa.Sample.Core.csproj", "sample/MainHost/Soa.Sample.Core/"]
COPY ["sample/Services.Abstract/Soa.Sample.IService1/Soa.Sample.IService1.csproj", "sample/Services.Abstract/Soa.Sample.IService1/"]
COPY ["sample/Services/Soa.Sample.Service1/Soa.Sample.Service1.csproj", "sample/Services/Soa.Sample.Service1/"]
COPY ["sample/Services/Soa.Sample.Service2/Soa.Sample.Service2.csproj", "sample/Services/Soa.Sample.Service2/"]
COPY ["src/Soa.Server/Soa.Server.csproj", "src/Soa.Server/"]
RUN dotnet restore "sample/Services/Soa.Sample.Service1.Host/Soa.Sample.Service1.Host.csproj"
COPY . .
WORKDIR "/src/sample/Services/Soa.Sample.Service1.Host"
RUN dotnet build "Soa.Sample.Service1.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Soa.Sample.Service1.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Soa.Sample.Service1.Host.dll"]