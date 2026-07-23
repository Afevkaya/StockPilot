using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface IProductQueryRepository
{
    Task<GetProductByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GetProductsResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}
