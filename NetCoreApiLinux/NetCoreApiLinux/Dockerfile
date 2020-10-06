FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY *.sln .
COPY docker-compose.dcproj .
COPY NetCoreApiLinux/*.csproj ./NetCoreApiLinux/
COPY DataLayer/*.csproj ./DataLayer/
RUN dotnet restore NetCoreApiLinux.sln

COPY NetCoreApiLinux/. ./NetCoreApiLinux/
COPY DataLayer/. ./DataLayer/
WORKDIR /app/NetCoreApiLinux
RUN dotnet publish "NetCoreApiLinux.csproj" -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "NetCoreApiLinux.dll"]
