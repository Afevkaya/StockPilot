using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;

namespace StockPilot.Application.Features.ProductSuppliers.Commands.RemoveSupplier;

public class RemoveSupplierHandler(
    IProductCommandRepository productCommandRepository,
    IValidator<RemoveSupplierCommand> validator)
{
    public async Task Handle(RemoveSupplierCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        await productCommandRepository.RemoveSupplierAsync(command.ProductId, command.SupplierId, cancellationToken);
    }
}
