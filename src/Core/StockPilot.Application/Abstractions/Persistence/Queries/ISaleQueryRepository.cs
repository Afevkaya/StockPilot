using StockPilot.Application.Features.Sales.Queries.GetSaleById;
using StockPilot.Application.Features.Sales.Queries.GetSales;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface ISaleQueryRepository
{
    Task<GetSalesResponse> GetSalesAsync(GetSalesQuery query, CancellationToken cancellationToken = default);
    Task<GetSaleByIdResponse?> GetSaleByIdAsync(Guid saleId, CancellationToken cancellationToken = default);
}
