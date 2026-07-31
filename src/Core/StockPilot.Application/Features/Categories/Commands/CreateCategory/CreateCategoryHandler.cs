using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryHandler(ICategoryCommandRepository repository, IValidator<CreateCategoryCommand> validator)
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        Category category = new(command.Name, command.Description);
        await repository.AddAsync(category, cancellationToken);
        return new CreateCategoryResponse(category.Id);
    }
}
