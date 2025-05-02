using eCommerce.BusinessLogicLayer.Mappers;
using eCommerce.BusinessLogicLayer.RabbitMQ;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.BusinessLogicLayer.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.ProductServices.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();
        services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestToProductMappingProfile>();
        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
        services.AddHostedService<RabbitMQInitializer>();
        return services;
    }
}