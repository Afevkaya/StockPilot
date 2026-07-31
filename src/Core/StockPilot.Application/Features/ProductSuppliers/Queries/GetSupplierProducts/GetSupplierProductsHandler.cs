using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.ProductSuppliers.Queries.GetSupplierProducts;

public class GetSupplierProductsHandler(ISupplierQueryRepository supplierQueryRepository)
{
    public async Task<IEnumerable<GetSupplierProductsResponse>> Handle(GetSupplierProductsQuery query,
        CancellationToken cancellationToken)
    {
        return await supplierQueryRepository.GetSupplierProductsAsync(query.SupplierId, cancellationToken);
    }
}
