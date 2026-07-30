using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierHandler(
    ISupplierCommandRepository supplierCommandRepository,
    IValidator<CreateSupplierCommand> validator)
{
    public async Task<Guid> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var supplier = new Supplier(
            command.Name,
            command.ContactName,
            command.Email,
            command.Phone,
            command.Address
        );

        await supplierCommandRepository.AddAsync(supplier, cancellationToken);
        return supplier.Id;
    }
}
