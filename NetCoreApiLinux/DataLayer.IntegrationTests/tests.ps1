docker-compose -f docker-compose-mongo.yml up --build -d
docker-compose -f docker-compose-tests.yml up --build
docker-compose -f docker-compose-mongo.yml down
docker-compose -f docker-compose-tests.yml down