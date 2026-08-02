using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface IPurchaseCommandRepository
{
    Task<bool> AddAsync(Purchase purchase, CancellationToken cancellationToken = default);
    Task<bool> AddWithStockMovementAsync(Purchase purchase, StockMovement stockMovement, CancellationToken cancellationToken = default);
}
