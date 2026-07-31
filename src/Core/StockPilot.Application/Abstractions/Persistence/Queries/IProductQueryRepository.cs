using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;
using StockPilot.Application.Features.ProductSuppliers.Queries.GetProductSuppliers;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface IProductQueryRepository
{
    Task<GetProductByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GetProductsResponse> GetAllAsync(GetProductsQuery productsQuery, CancellationToken cancellationToken = default);
    Task<IEnumerable<GetProductSuppliersResponse>> GetProductSuppliersAsync(Guid productId, CancellationToken cancellationToken = default);
}
