using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryHandler(
    ICategoryCommandRepository categoryCommandRepository)
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? existingCategory = await categoryCommandRepository.GetByIdAsync(command.Id, cancellationToken);

        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Kategori bulunamadı. Id: {command.Id}");
        }

        existingCategory.Update(command.Name, command.Description);

        await categoryCommandRepository.UpdateAsync(existingCategory, cancellationToken);

        return new UpdateCategoryResponse(
            existingCategory.Id,
            existingCategory.Name,
            existingCategory.Description
        );
    }
}
