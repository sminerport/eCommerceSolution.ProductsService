# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7070
EXPOSE 7071

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductsMicroService.API/ProductsMicroService.API.csproj", "ProductsMicroService.API/"]
RUN dotnet restore "./ProductsMicroService.API/ProductsMicroService.API.csproj"
COPY . .
WORKDIR "/src/ProductsMicroService.API"
RUN dotnet build "./ProductsMicroService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductsMicroService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV MYSQL_HOST=localhost
ENV MYSQL_DB=ecommerceproductsdatabase
ENV MYSQL_USER=root
ENV MYSQL_PASSWORD=admin
ENV MYSQL_PORT=3306
ENV RABBITMQ_HOST=rabbitmq
ENV RABBITMQ_USERNAME=guest
ENV RABBITMQ_PASSWORD=guest
ENV RABBITMQ_PORT=5672
ENV RABBITMQ_PRODUCTS_EXCHANGE=products.exchange
ENTRYPOINT ["dotnet", "ProductsMicroService.API.dll"]