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
        string connTemplate = configuration.GetConnectionString("DefaultConnection")!;
        if (string.IsNullOrWhiteSpace(connTemplate))
            throw new InvalidOperationException("No DefaultConnection in config");

        // pull every value from IConfiguration (which includes User Secrets)
        var replacements = new Dictionary<string, string>
        {
            ["$MYSQL_HOST"] = configuration["MYSQL_HOST"]!,
            ["$MYSQL_DB"] = configuration["MYSQL_DB"]!,
            ["$MYSQL_USER"] = configuration["MYSQL_USER"]!,
            ["$MYSQL_PASSWORD"] = configuration["MYSQL_PASSWORD"]!,
            ["$MYSQL_PORT"] = configuration["MYSQL_PORT"]!
        };
        foreach (var kv in replacements)
            connTemplate = connTemplate.Replace(kv.Key, kv.Value);

        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseMySQL(connTemplate));
        // Register your repositories
        // e.g., services.AddScoped<IYourRepository, YourRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}