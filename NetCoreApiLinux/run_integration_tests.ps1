docker-compose -f ./DataLayer.IntegrationTests/docker-compose.yml up --build -d
dotnet test DataLayer.IntegrationTests
docker-compose -f ./DataLayer.IntegrationTests/docker-compose.yml down