using Microsoft.Extensions.DependencyInjection;
using StockPilot.Application.Features.Products.CreateProduct;

namespace StockPilot.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<CreateProductHandler>();
        return services;
    }
}
