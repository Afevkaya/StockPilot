using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Products.Commands.CreateProduct;
using StockPilot.Application.Features.Products.Commands.UpdateProduct;
using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(
    CreateProductHandler createProductHandler,
    GetProductByIdHandler getProductByIdHandler,
    GetProductsHandler getProductsHandler,
    UpdateProductHandler updateProductHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var productResponse =
                await createProductHandler.HandleAsync(command, cancellationToken);

            return Ok(productResponse);
        }
        catch (ValidationException exception)
        {
            var errors = exception.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var product = await getProductByIdHandler.HandleAsync(new GetProductByIdQuery(id), cancellationToken);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        var products = await getProductsHandler.HandleAsync(cancellationToken);
        return Ok(products);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        UpdateProductCommand updateCommand = command with { Id = id };
        try
        {
            var updatedProduct = await updateProductHandler.HandleAsync(updateCommand, cancellationToken);
            return Ok(updatedProduct);
        }
        catch (ValidationException exception)
        {
            var errors = exception.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors));
        }

    }
}
