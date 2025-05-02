using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.Repositories;
using eCommerce.DataAccessLayer.RepositoryContracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.ProductServices.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add your infrastructure services here
        // e.g., services.AddDbContext<YourDbContext>();
        string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;

        string connectionString = connectionStringTemplate
            .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST")!)
            .Replace("$MYSQL_DB", Environment.GetEnvironmentVariable("MYSQL_DB")!)
            .Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER")!)
            .Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT")!)
            .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD")!);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });
        // Register your repositories
        // e.g., services.AddScoped<IYourRepository, YourRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}