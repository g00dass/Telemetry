docker-compose -f ./DataLayer.IntegrationTests/docker-compose.yml up --build -d
dotnet build MongoMigrations/MongoMigrations.csproj
./MongoMigrations/bin/Debug/netcoreapp3.1/MongoMigrations.exe