using StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;
using StockPilot.Application.Features.Suppliers.Queries.GetSupplierById;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface ISupplierQueryRepository
{
    Task<GetAllSuppliersResponse?> GetAllAsync(GetAllSuppliersQuery query, CancellationToken cancellationToken = default);
    Task<GetSupplierByIdResponse?> GetByIdAsync(GetSupplierByIdQuery query, CancellationToken cancellationToken = default);
}
