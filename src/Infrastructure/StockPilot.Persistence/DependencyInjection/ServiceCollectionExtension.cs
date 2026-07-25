using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Persistence.Commands;
using StockPilot.Persistence.Connections;
using StockPilot.Persistence.Queries;

namespace StockPilot.Persistence.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("PostgreSQLConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'PostgreSQLConnection' not found.");
        }

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
        services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();

        return services;
    }
}
