FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY *.sln .
COPY NetCoreApiLinux/*.csproj ./NetCoreApiLinux/
RUN dotnet restore

COPY NetCoreApiLinux/. ./NetCoreApiLinux/
WORKDIR /app/NetCoreApiLinux
RUN dotnet publish -c Release -o /out --no-restore


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "NetCoreApiLinux.dll"]