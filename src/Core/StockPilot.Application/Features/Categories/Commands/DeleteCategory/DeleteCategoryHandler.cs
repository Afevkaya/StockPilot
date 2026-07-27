using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryHandler(ICategoryCommandRepository categoryCommandRepository)
{
    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? existingCategory = await categoryCommandRepository.GetByIdAsync(command.Id, cancellationToken);

        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Kategori bulunamadı. Id: {command.Id}");
        }

        await categoryCommandRepository.DeleteAsync(existingCategory, cancellationToken);
    }
}
