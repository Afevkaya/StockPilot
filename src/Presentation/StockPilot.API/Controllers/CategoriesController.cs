using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockPilot.Application.Features.Categories.Commands.CreateCategory;
using StockPilot.Application.Features.Categories.Commands.DeleteCategory;
using StockPilot.Application.Features.Categories.Commands.UpdateCategory;
using StockPilot.Application.Features.Categories.Queries.GetCategories;
using StockPilot.Application.Features.Categories.Queries.GetCategory;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(
    CreateCategoryHandler createCategoryHandler,
    GetCategoriesHandler getCategoriesHandler,
    GetCategoryByIdHandler getCategoryByIdHandler,
    UpdateCategoryHandler updateCategoryHandler,
    DeleteCategoryHandler deleteCategoryHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var response = await createCategoryHandler.Handle(command, cancellationToken);
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

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var response = await getCategoriesHandler.Handle(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        var response = await getCategoryByIdHandler.Handle(new GetCategoryByIdQuery(id), cancellationToken);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        UpdateCategoryCommand updatedCommand = command with { Id = id };
        try
        {
            var response = await updateCategoryHandler.Handle(updatedCommand, cancellationToken);
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
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        await deleteCategoryHandler.Handle(new DeleteCategoryCommand(id), cancellationToken);
        return NoContent();
    }
}
