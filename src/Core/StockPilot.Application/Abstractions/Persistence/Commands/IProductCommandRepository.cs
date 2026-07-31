using StockPilot.Domain.Entities;

namespace StockPilot.Application.Abstractions.Persistence.Commands;

public interface IProductCommandRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AssignSupplierAsync(Guid productId, Guid supplierId, CancellationToken cancellationToken = default);
    Task RemoveSupplierAsync(Guid productId, Guid supplierId, CancellationToken cancellationToken = default);
}
