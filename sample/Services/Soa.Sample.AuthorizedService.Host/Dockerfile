#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8009

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/Services/Soa.Sample.AuthorizedService.Host/Soa.Sample.AuthorizedService.Host.csproj", "sample/Services/Soa.Sample.AuthorizedService.Host/"]
COPY ["src/Soa.Server/Soa.Server.csproj", "src/Soa.Server/"]
COPY ["src/Soa/Soa.csproj", "src/Soa/"]
COPY ["sample/Services/Soa.Sample.AuthorizedService/Soa.Sample.AuthorizedService.csproj", "sample/Services/Soa.Sample.AuthorizedService/"]
COPY ["sample/Services.Abstract/Soa.Sample.IAuthorizedService/Soa.Sample.IAuthorizedService.csproj", "sample/Services.Abstract/Soa.Sample.IAuthorizedService/"]
COPY ["sample/Services/Soa.Sample.AuthorizedService.EntityFrameworkCore/Soa.Sample.AuthorizedService.EntityFrameworkCore.csproj", "sample/Services/Soa.Sample.AuthorizedService.EntityFrameworkCore/"]
RUN dotnet restore "sample/Services/Soa.Sample.AuthorizedService.Host/Soa.Sample.AuthorizedService.Host.csproj"
COPY . .
WORKDIR "/src/sample/Services/Soa.Sample.AuthorizedService.Host"
RUN dotnet build "Soa.Sample.AuthorizedService.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Soa.Sample.AuthorizedService.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Soa.Sample.AuthorizedService.Host.dll"]