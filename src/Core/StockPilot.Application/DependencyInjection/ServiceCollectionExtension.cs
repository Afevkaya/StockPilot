using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StockPilot.Application.Features.Categories.Commands.CreateCategory;
using StockPilot.Application.Features.Categories.Commands.DeleteCategory;
using StockPilot.Application.Features.Categories.Commands.UpdateCategory;
using StockPilot.Application.Features.Categories.Queries.GetCategories;
using StockPilot.Application.Features.Categories.Queries.GetCategory;
using StockPilot.Application.Features.Products.Commands.CreateProduct;
using StockPilot.Application.Features.Products.Commands.DeleteProduct;
using StockPilot.Application.Features.Products.Commands.UpdateProduct;
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
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<GetCategoriesHandler>();
        services.AddScoped<GetCategoryByIdHandler>();
        services.AddScoped<UpdateCategoryHandler>();
        services.AddScoped<DeleteCategoryHandler>();
        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        return services;
    }
}
