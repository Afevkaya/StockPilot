using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface ICategoryCommandRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);
}
