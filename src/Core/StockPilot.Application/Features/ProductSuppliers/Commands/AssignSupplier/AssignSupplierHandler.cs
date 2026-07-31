using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.ProductSuppliers.Commands.AssignSupplier;

public class AssignSupplierHandler(
    IProductCommandRepository productCommandRepository,
    ISupplierCommandRepository supplierCommandRepository,
    IValidator<AssignSupplierCommand> validator)
{
    public async Task Handle(AssignSupplierCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        Product? product = await productCommandRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            throw new KeyNotFoundException($"Ürün bulunamadı");
        }
        Supplier? supplier = await supplierCommandRepository.GetByIdAsync(command.SupplierId, cancellationToken);
        if (supplier is null)
        {
            throw new KeyNotFoundException("Tedarikçi bulunamadı");
        }

        bool assigned = await productCommandRepository.AssignSupplierAsync(
            command.ProductId,
            command.SupplierId,
            cancellationToken);

        if (!assigned)
        {
            throw new InvalidOperationException(
                "Ürün ile tedarikçi ilişkisi zaten mevcut.");
        }
    }
}
