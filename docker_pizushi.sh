#!/bin/bash

set -e

server_up() {
    echo "Server up..."
    docker pull novakvova/pizushi-asp-api:latest
    docker stop pizushi-asp_container || true
    docker rm pizushi-asp_container || true
    docker run -d --restart=always -v /volumes/pizushi-asp/images:/app/images --name pizushi-asp_container -p 4282:8080 novakvova/pizushi-asp-api
}

start_containers() {
    echo "Containers start..."
    docker run -d --restart=always -v /volumes/pizushi-asp/images:/app/images --name pizushi-asp_container -p 4282:8080 novakvova/pizushi-asp-api
}

stop_containers() {
    echo "Containers stop..."
    docker stop pizushi-asp_container || true
    docker rm pizushi-asp_container || true
}

restart_containers() {
    echo "Containers restart..."
    docker stop pizushi-asp_container || true
    docker rm pizushi-asp_container || true
    docker run -d --restart=always -v /volumes/pizushi-asp/images:/app/images --name pizushi-asp_container -p 4282:8080 novakvova/pizushi-asp-api
}

echo "Choose action:"
echo "1. Server up"
echo "2. Containers start"
echo "3. Containers stop"
echo "4. Containers restart"
read -p "Enter action number: " action

case $action in
    1)
        server_up
        ;;
    2)
        start_containers
        ;;
    3)
        stop_containers
        ;;
    4)
        restart_containers
        ;;
    *)
        echo "Invalid action number!"
        exit 1
        ;;
esac
