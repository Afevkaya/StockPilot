using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface ICategoryCommandRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(Category category, CancellationToken cancellationToken);
}
