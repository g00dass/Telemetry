docker-compose up --build -d
dotnet build ../MongoMigrations/MongoMigrations.csproj
../MongoMigrations/bin/Debug/netcoreapp3.1/MongoMigrations
dotnet test ../DataLayer.IntegrationTests -v n
docker-compose down