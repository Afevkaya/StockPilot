using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Sales.Commands.CreateSale;
using StockPilot.Application.Features.Sales.Queries.GetSaleById;
using StockPilot.Application.Features.Sales.Queries.GetSales;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSale(
        [FromBody] CreateSaleCommand command,
        [FromServices] CreateSaleHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await handler.Handle(command, cancellationToken);
            return CreatedAtAction(nameof(GetSaleById), new { id = response.Id }, response);
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
    public async Task<IActionResult> GetSales([FromQuery] GetSalesQuery query, [FromServices] GetSalesHandler handler, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await handler.Handle(query, cancellationToken));
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

    [HttpGet("{id:guid}", Name = "GetSaleById")]
    public async Task<IActionResult> GetSaleById([FromRoute] Guid id, [FromServices] GetSaleByIdHandler handler, CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetSaleByIdQuery(id), cancellationToken));
    }
}
