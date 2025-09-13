@echo off

echo Changing directory client...
cd "my-react-ts"

echo Building Docker image client...
docker build -t pizushi-client .

echo Docker login...
docker login

echo Tagging Docker image client...
docker tag pizushi-client:latest novakvova/apizushi-client:latest

echo Pushing Docker image client to repository...
docker push novakvova/pizushi-client:latest

echo Done ---client---!

echo Changing directory api...
cd ".."
cd "WebApiPizushi"

echo Building Docker image api...
docker build -t pizushi-api . 

echo Tagging Docker image api...
docker tag pizushi-api:latest novakvova/pizushi-api:latest

echo Pushing Docker image api to repository...
docker push novakvova/pizushi-api:latest

echo Done ---api---!
pause

