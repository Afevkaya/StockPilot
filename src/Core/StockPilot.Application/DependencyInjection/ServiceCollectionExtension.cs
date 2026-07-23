using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StockPilot.Application.Features.Products.Commands.CreateProduct;
using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;

namespace StockPilot.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<GetProductsHandler>();
        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        return services;
    }
}
