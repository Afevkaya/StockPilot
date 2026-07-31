using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.StockMovements.Commands.CreateStockMovement;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/stockmovements")]
public class StockMovementsController(CreateStockMovementHandler createStockMovementHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStockMovement(CreateStockMovementCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await createStockMovementHandler.Handle(command, cancellationToken));
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
