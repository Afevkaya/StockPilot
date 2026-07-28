using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Inventories.Queries.GetAllInventories;

public class GetAllInventoriesHandler(IInventoryQueryRepository inventoryQueryRepository)
{
    public async Task<IEnumerable<GetAllInventoriesResponse>?> Handle(CancellationToken cancellationToken)
    {
        return await inventoryQueryRepository.GetAllInventories(cancellationToken);
    }
}
