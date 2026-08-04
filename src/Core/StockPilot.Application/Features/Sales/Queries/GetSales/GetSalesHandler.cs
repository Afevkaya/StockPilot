using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Sales.Queries.GetSales;

public class GetSalesHandler(
    ISaleQueryRepository saleQueryRepository,
    IValidator<GetSalesQuery> validator)
{
    public async Task<GetSalesResponse> Handle(GetSalesQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        return await saleQueryRepository.GetSalesAsync(query, cancellationToken);
    }
}
