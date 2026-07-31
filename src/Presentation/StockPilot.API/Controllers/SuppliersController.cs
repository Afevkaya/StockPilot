using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.ProductSuppliers.Queries.GetSupplierProducts;
using StockPilot.Application.Features.Suppliers.Commands.CreateSupplier;
using StockPilot.Application.Features.Suppliers.Commands.DeleteSupplier;
using StockPilot.Application.Features.Suppliers.Commands.UpdateSupplier;
using StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;
using StockPilot.Application.Features.Suppliers.Queries.GetSupplierById;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController(
    CreateSupplierHandler createSupplierHandler,
    GetAllSuppliersHandler getAllSuppliersHandler,
    GetSupplierByIdHandler getSupplierByIdHandler,
    UpdateSupplierHandler updateSupplierHandler,
    DeleteSupplierHandler deleteSupplierHandler,
    GetSupplierProductsHandler getSupplierProductsHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await createSupplierHandler.Handle(command, cancellationToken));
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

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers([FromQuery] GetAllSuppliersQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await getAllSuppliersHandler.Handle(query, cancellationToken));
        }
        catch (ValidationException exception)
        {
            var errors = exception.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());
            return ValidationProblem(new ValidationProblemDetails(errors));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSupplier([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await getSupplierByIdHandler.Handle(new GetSupplierByIdQuery(id), cancellationToken));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSupplier([FromRoute] Guid id, UpdateSupplierCommand command,
        CancellationToken cancellationToken)
    {
        UpdateSupplierCommand updateSupplierCommand = command with { Id = id };

        try
        {
            return Ok(await updateSupplierHandler.Handle(updateSupplierCommand, cancellationToken));
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
    public async Task<IActionResult> DeleteSupplier(Guid id, CancellationToken cancellationToken)
    {
        await deleteSupplierHandler.Handle(new DeleteSupplierCommand(id), cancellationToken);
        return Ok();
    }

    [HttpGet("{SupplierId:guid}/products")]
    public async Task<IActionResult> GetProducts([FromRoute] GetSupplierProductsQuery query,
        CancellationToken cancellationToken = default)
    {
        return Ok(await getSupplierProductsHandler.Handle(query, cancellationToken));
    }
}
