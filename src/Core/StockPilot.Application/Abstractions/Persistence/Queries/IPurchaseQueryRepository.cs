using StockPilot.Application.Features.Purchases.Queries.GetPurchaseById;
using StockPilot.Application.Features.Purchases.Queries.GetPurchases;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface IPurchaseQueryRepository
{
    Task<IEnumerable<GetPurchasesResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetPurchaseByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
