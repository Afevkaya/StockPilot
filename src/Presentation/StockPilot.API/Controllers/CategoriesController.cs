using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Categories.Commands.CreateCategory;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CreateCategoryHandler createCategoryHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var response = await createCategoryHandler.HandleAsync(command, cancellationToken);
            return Ok(response);
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
