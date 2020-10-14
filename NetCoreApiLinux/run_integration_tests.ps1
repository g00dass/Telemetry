./build_integration_tests.ps1
dotnet test DataLayer.IntegrationTests
docker-compose -f ./DataLayer.IntegrationTests/docker-compose.yml down