using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Persistence.Commands;
using StockPilot.Persistence.Connections;
using StockPilot.Persistence.Migrations;
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

        services.AddSingleton(_ => new DatabaseMigrator(connectionString));
        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
        services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
        services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
        services.AddScoped<IStockMovementCommandRepository, StockMovementCommandRepository>();
        services.AddScoped<IInventoryQueryRepository, InventoryQueryRepository>();
        services.AddScoped<ISupplierCommandRepository, SupplierCommandRepository>();
        services.AddScoped<ISupplierQueryRepository, SupplierQueryRepository>();
        services.AddScoped<IPurchaseCommandRepository, PurchaseCommandRepository>();
        services.AddScoped<IPurchaseQueryRepository, PurchaseQueryRepository>();
        services.AddScoped<ISaleCommandRepository, SaleCommandRepository>();
        services.AddScoped<ISaleQueryRepository, SaleQueryRepository>();

        return services;
    }
}
