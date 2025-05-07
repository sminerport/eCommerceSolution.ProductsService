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

        if (string.IsNullOrEmpty(connectionStringTemplate))
        {
            throw new InvalidOperationException("No DefaultConnection found in appsettings!");
        }

        var mysqlHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
        if (string.IsNullOrEmpty(mysqlHost))
        {
            throw new InvalidOperationException("MYSQL_HOST env variable is missing!");
        }

        var mysqlDb = Environment.GetEnvironmentVariable("MYSQL_DB");
        if (string.IsNullOrEmpty(mysqlDb))
        {
            throw new InvalidOperationException("MYSQL_DB env variable is missing!");
        }

        var mysqlUser = Environment.GetEnvironmentVariable("MYSQL_USER");
        if (string.IsNullOrEmpty(mysqlUser))
        {
            throw new InvalidOperationException("MYSQL_USER env variable is missing!");
        }

        var mysqlPort = Environment.GetEnvironmentVariable("MYSQL_PORT");
        if (string.IsNullOrEmpty(mysqlPort))
        {
            throw new InvalidOperationException("MYSQL_PORT env variable is missing!");
        }
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