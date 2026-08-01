using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Purchases.Commands.CreatePurchase;
using StockPilot.Application.Features.Purchases.Queries.GetPurchaseById;
using StockPilot.Application.Features.Purchases.Queries.GetPurchases;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/purchases")]
public class PurchasesController(
    CreatePurchaseHandler createPurchaseHandler,
    GetPurchasesHandler getPurchasesHandler,
    GetPurchaseByIdHandler getPurchaseByIdHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePurchase(CreatePurchaseCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await createPurchaseHandler.Handle(command, cancellationToken));
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
    public async Task<IActionResult> GetPurchases(CancellationToken cancellationToken)
    {
        return Ok(await getPurchasesHandler.Handle(cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPurchaseById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await getPurchaseByIdHandler.Handle(new GetPurchaseByIdQuery(id), cancellationToken));
    }
}
