using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Purchases.Queries.GetPurchases;

public class GetPurchasesHandler(IPurchaseQueryRepository purchaseQueryRepository)
{
    public async Task<IEnumerable<GetPurchasesResponse>> Handle(CancellationToken cancellationToken)
    {
        return await purchaseQueryRepository.GetAllAsync(cancellationToken);
    }
}
