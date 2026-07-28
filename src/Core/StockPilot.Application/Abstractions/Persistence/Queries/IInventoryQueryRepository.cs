using StockPilot.Application.Features.Inventories.Queries.GetAllInventories;
using StockPilot.Application.Features.Inventories.Queries.GetProductInventory;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface IInventoryQueryRepository
{
    Task<GetProductInventoryResponse?> GetProductInventoryById(GetProductInventoryQuery query,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<GetAllInventoriesResponse>?> GetAllInventories(CancellationToken cancellationToken = default);
}
