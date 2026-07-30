using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdHandler(ISupplierQueryRepository getSupplierRepository)
{
    public async Task<GetSupplierByIdResponse> Handle(GetSupplierByIdQuery getSupplierByIdQuery, CancellationToken cancellationToken)
    {
        GetSupplierByIdResponse? supplier = await getSupplierRepository.GetByIdAsync(getSupplierByIdQuery, cancellationToken);
        if (supplier == null)
        {
            throw new ArgumentNullException("Tedarikçi bulunamadı");
        }
        return supplier;
    }
}
