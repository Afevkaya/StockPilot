using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface ISaleCommandRepository
{
    Task<bool> AddWithStockMovementAsync(Sale sale, StockMovement stockMovement, CancellationToken cancellationToken = default);
}
