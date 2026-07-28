using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface IStockMovementCommandRepository
{
    Task AddAsync(StockMovement stockMovement, CancellationToken cancellationToken = default);
}
