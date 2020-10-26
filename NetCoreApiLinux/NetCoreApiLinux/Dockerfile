FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY *.sln .
COPY docker-compose.dcproj .
COPY NetCoreApiLinux/*.csproj ./NetCoreApiLinux/
COPY DataLayer/*.csproj ./DataLayer/
COPY DataLayer.Tests/*.csproj ./DataLayer.Tests/
COPY DataLayer.IntegrationTests/*.csproj ./DataLayer.IntegrationTests/
COPY Sandbox/*.csproj ./Sandbox/
COPY Telemetry.Client/*.csproj ./Telemetry.Client/
COPY Telemetry.Client.Test/*.csproj ./Telemetry.Client.Test/
COPY MongoMigrations/*.csproj ./MongoMigrations/
COPY NotifierDaemon/*.csproj ./NotifierDaemon/
RUN dotnet restore NetCoreApiLinux.sln

COPY NetCoreApiLinux/. ./NetCoreApiLinux/
COPY DataLayer/. ./DataLayer/
COPY DataLayer.Tests/. ./DataLayer.Tests/
COPY DataLayer.IntegrationTests/. ./DataLayer.IntegrationTests/
COPY Sandbox/. ./Sandbox/
COPY Telemetry.Client/. ./Telemetry.Clients/
COPY Telemetry.Client.Test/. ./Telemetry.Client.Test/
COPY MongoMigrations/. ./MongoMigrations/
COPY NotifierDaemon/. ./NotifierDaemon/
WORKDIR /app/NetCoreApiLinux
RUN dotnet publish "NetCoreApiLinux.csproj" -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "NetCoreApiLinux.dll"]
