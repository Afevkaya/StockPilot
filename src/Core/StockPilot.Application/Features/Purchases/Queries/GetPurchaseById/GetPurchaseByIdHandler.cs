using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Purchases.Queries.GetPurchaseById;

public class GetPurchaseByIdHandler(IPurchaseQueryRepository purchaseQueryRepository)
{
    public async Task<GetPurchaseByIdResponse> Handle(GetPurchaseByIdQuery query, CancellationToken cancellationToken)
    {
        var purchase = await purchaseQueryRepository.GetByIdAsync(query.Id, cancellationToken);
        if (purchase == null)
        {
            throw new InvalidOperationException($"Satın alma bulunamadı. Id: {query.Id}");
        }
        return purchase;
    }
}
