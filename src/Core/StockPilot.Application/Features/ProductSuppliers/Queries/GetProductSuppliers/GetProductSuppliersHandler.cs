using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Products.Queries.GetProductById;

namespace StockPilot.Application.Features.ProductSuppliers.Queries.GetProductSuppliers;

public class GetProductSuppliersHandler(IProductQueryRepository productQueryRepository)
{
    public async Task<IEnumerable<GetProductSuppliersResponse>> Handle(GetProductSuppliersQuery query,
        CancellationToken cancellationToken)
    {
        GetProductByIdResponse? product = await productQueryRepository.GetByIdAsync(query.ProductId, cancellationToken);
        if (product is null)
        {
            throw new KeyNotFoundException("ÜRün bulunamadı");
        }
        return await productQueryRepository.GetProductSuppliersAsync(query.ProductId, cancellationToken);
    }
}
