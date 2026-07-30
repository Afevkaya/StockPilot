using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierHandler(ISupplierCommandRepository supplierCommandRepository)
{
    public async Task Handle(DeleteSupplierCommand command, CancellationToken cancellationToken)
    {
        Supplier? supplier = await supplierCommandRepository.GetByIdAsync(command.Id);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Tedarikçi bulunamadı");
        }

        await supplierCommandRepository.DeleteAsync(command.Id, cancellationToken);
    }
}
