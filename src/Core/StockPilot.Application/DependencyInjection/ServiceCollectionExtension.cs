using Microsoft.Extensions.DependencyInjection;
using StockPilot.Application.Features.Products.Commands.CreateProduct;
using StockPilot.Application.Features.Products.Queries.GetProductById;

namespace StockPilot.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        return services;
    }
}
