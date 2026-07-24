using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface IProductQueryRepository
{
    Task<GetProductByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GetProductsResponse> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
