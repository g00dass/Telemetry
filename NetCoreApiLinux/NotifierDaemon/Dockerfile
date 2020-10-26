FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY DataLayer/*.csproj ./DataLayer/
COPY NotifierDaemon/*.csproj ./NotifierDaemon/

RUN dotnet restore DataLayer
RUN dotnet restore NotifierDaemon

COPY DataLayer/. ./DataLayer/
COPY NotifierDaemon/. ./NotifierDaemon/

WORKDIR /app/NotifierDaemon
RUN dotnet publish "NotifierDaemon.csproj" -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "NotifierDaemon.dll"]
