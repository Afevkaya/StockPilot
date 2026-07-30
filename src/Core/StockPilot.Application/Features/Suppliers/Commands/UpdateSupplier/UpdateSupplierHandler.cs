using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierHandler(
    ISupplierCommandRepository supplierCommandRepository)
{
    public async Task<UpdateSupplierResponse> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
    {
        Supplier? supplier = await supplierCommandRepository.GetByIdAsync(command.Id, cancellationToken);
        if (supplier == null)
        {
            throw new KeyNotFoundException("Tedarikçi bulunamadı");
        }

        supplier.Update(command.Name, command.ContactName, command.Email, command.Phone, command.Address);
        await supplierCommandRepository.UpdateAsync(supplier, cancellationToken);

        return new UpdateSupplierResponse(supplier.Id, supplier.Name, supplier.Email, supplier.Phone);
    }
}
