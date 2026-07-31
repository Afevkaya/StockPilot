using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdHandler(IProductQueryRepository productQueryRepository)
{
    public async Task<GetProductByIdResponse?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default)
    {
        var product = await productQueryRepository.GetByIdAsync(query.Id, cancellationToken);
        if (product is null)
        {
            return null;
        }

        return product;
    }
}
