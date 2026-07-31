using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public class GetProductsHandler(
    IProductQueryRepository productQueryRepository,
    IValidator<GetProductsQuery> validator)
{
    public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        return await productQueryRepository.GetAllAsync(query, cancellationToken);
    }
}
