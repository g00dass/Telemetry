FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY DataLayer/*.csproj ./DataLayer/
COPY DataLayer.Tests/*.csproj ./DataLayer.Tests/
COPY DataLayer.IntegrationTests/*.csproj ./DataLayer.IntegrationTests/
COPY MongoMigrations/*.csproj ./MongoMigrations/
RUN dotnet restore DataLayer/*.csproj

COPY DataLayer/. ./DataLayer/
COPY DataLayer.Tests/. ./DataLayer.Tests/
COPY DataLayer.IntegrationTests/. ./DataLayer.IntegrationTests/
COPY MongoMigrations/. ./MongoMigrations/
RUN dotnet build MongoMigrations/MongoMigrations.csproj
