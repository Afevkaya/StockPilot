using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface IProductCommandRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
}
