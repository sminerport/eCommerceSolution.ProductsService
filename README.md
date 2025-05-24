# Products Microservice

This repository contains the **Products** service for the eCommerce solution. The service exposes a REST API built with **ASP.NET Core** and provides CRUD operations for products.

## Projects

- **ProductsMicroService.API** – ASP.NET Core Web API project hosting HTTP endpoints.
- **BusinessLogicLayer** – Contains DTOs, service logic and RabbitMQ publishing logic.
- **DataAccessLayer** – Entity Framework Core context, entities and repository implementations.
- **ProductsUnitTests** – Unit tests for the business logic layer.
- **k8s** – Kubernetes manifests for deploying the service.

The solution targets **.NET 9.0** and uses **MySQL** as the database and **RabbitMQ** for event messages.

## Building and Running

The service can be run locally using the `dotnet` CLI or via Docker.

```bash
# Restore and build the solution
 dotnet build

# Run the API from the service project
 dotnet run --project ProductsMicroService.API
```

Running the service requires the following environment variables:

- `MYSQL_HOST`
- `MYSQL_DB`
- `MYSQL_USER`
- `MYSQL_PASSWORD`
- `MYSQL_PORT`
- `RABBITMQ_HOST`
- `RABBITMQ_USERNAME`
- `RABBITMQ_PASSWORD`
- `RABBITMQ_PORT`
- `RABBITMQ_PRODUCTS_EXCHANGE`

Alternatively, build and run using Docker:

```bash
 docker build -t products-microservice -f ProductsMicroService.API/Dockerfile .
 docker run -p 7070:7070 --env-file <env-file> products-microservice
```

Kubernetes manifests are provided under the `k8s/` directory for deployment to different environments.

## Testing

Unit tests are located in the `ProductsUnitTests` project:

```bash
 dotnet test
```

## CI/CD

The repository includes an `azure-pipelines.yml` file that builds the Docker image, runs tests and deploys the service to Kubernetes clusters for various environments.
