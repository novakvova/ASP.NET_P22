#!/bin/bash
set -e  # зупиняє скрипт при помилці

cd my-react-ts
docker build -t pizushi-client .
docker login
docker tag pizushi-client:latest novakvova/pizushi-client:latest
docker push novakvova/pizushi-client:latest
echo "Done ---client---!"

cd ..

cd WebApiPizushi
docker build -t pizushi-api .
docker tag pizushi-api:latest novakvova/pizushi-api:latest
docker push novakvova/pizushi-api:latest
echo "Done ---api---!"

read -p "Press any key to exit..."
