using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersHandler(
    ISupplierQueryRepository supplierQueryRepository,
    IValidator<GetAllSuppliersQuery> validator)
{
    public async Task<GetAllSuppliersResponse?> Handle(GetAllSuppliersQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        return await supplierQueryRepository.GetAllAsync(query, cancellationToken);
    }
}
