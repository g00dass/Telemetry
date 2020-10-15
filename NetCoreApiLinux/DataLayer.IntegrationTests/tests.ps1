docker-compose up --build -d
docker-compose exec tests ./MongoMigrations/bin/Debug/netcoreapp3.1/MongoMigrations
docker-compose exec tests dotnet test DataLayer.IntegrationTests
docker-compose down