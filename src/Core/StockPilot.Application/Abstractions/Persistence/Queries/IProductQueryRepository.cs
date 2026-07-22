using StockPilot.Application.Features.Products.Queries.GetProductById;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface IProductQueryRepository
{
    Task<GetProductByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
