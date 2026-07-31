using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Products.Commands.CreateProduct;
using StockPilot.Application.Features.Products.Commands.DeleteProduct;
using StockPilot.Application.Features.Products.Commands.UpdateProduct;
using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;
using StockPilot.Application.Features.ProductSuppliers.Commands.AssignSupplier;
using StockPilot.Application.Features.ProductSuppliers.Commands.RemoveSupplier;
using StockPilot.Application.Features.ProductSuppliers.Queries.GetProductSuppliers;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(
    CreateProductHandler createProductHandler,
    GetProductByIdHandler getProductByIdHandler,
    GetProductsHandler getProductsHandler,
    UpdateProductHandler updateProductHandler,
    DeleteProductHandler deleteProductHandler,
    AssignSupplierHandler assignSupplierHandler,
    RemoveSupplierHandler removeSupplierHandler,
    GetProductSuppliersHandler getProductSuppliersHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var productResponse =
                await createProductHandler.Handle(command, cancellationToken);

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
        var product = await getProductByIdHandler.Handle(new GetProductByIdQuery(id), cancellationToken);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = await getProductsHandler.Handle(query, cancellationToken);
            return Ok(data);
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        UpdateProductCommand updateCommand = command with { Id = id };
        try
        {
            var updatedProduct = await updateProductHandler.Handle(updateCommand, cancellationToken);
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        await deleteProductHandler.Handle(new DeleteProductCommand(id), cancellationToken);
        return Ok();
    }

    [HttpPost("{ProductId:guid}/suppliers/{SupplierId:guid}")]
    public async Task<IActionResult> AssignSupplier([FromRoute] AssignSupplierCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            await assignSupplierHandler.Handle(command, cancellationToken);
            return Ok();
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

    [HttpDelete("{ProductId:guid}/suppliers/{SupplierId:guid}")]
    public async Task<IActionResult> RemoveSupplier([FromRoute] RemoveSupplierCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            await removeSupplierHandler.Handle(command, cancellationToken);
            return Ok();
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

    [HttpGet("{ProductId:guid}/suppliers")]
    public async Task<IActionResult> GetSuppliers([FromRoute] GetProductSuppliersQuery query, CancellationToken cancellationToken)
    {
        return Ok(await getProductSuppliersHandler.Handle(query, cancellationToken));
    }
}
