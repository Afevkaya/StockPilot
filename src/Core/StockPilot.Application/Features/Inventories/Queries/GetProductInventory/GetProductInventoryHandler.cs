using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Inventories.Queries.GetProductInventory;

public class GetProductInventoryHandler(IInventoryQueryRepository inventoryQueryRepository)
{
    public async Task<GetProductInventoryResponse?> Handle(GetProductInventoryQuery query, CancellationToken cancellationToken)
    {
        return await inventoryQueryRepository.GetProductInventoryById(query, cancellationToken);
    }
}
