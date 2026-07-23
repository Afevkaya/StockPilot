using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public class GetProductsHandler(IProductQueryRepository productQueryRepository)
{
    public async Task<IEnumerable<GetProductsResponse>> HandleAsync(CancellationToken cancellationToken)
    {
        return await productQueryRepository.GetAllAsync(cancellationToken);
    }
}
