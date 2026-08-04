using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Sales.Queries.GetSaleById;

public class GetSaleByIdHandler(ISaleQueryRepository saleQueryRepository)
{
    public async Task<GetSaleByIdResponse> Handle(GetSaleByIdQuery query, CancellationToken cancellationToken)
    {
        var sale = await saleQueryRepository.GetSaleByIdAsync(query.Id, cancellationToken);
        if (sale is null)
        {
            throw new KeyNotFoundException("Satış bulunamadı");
        }

        return sale;
    }
}
