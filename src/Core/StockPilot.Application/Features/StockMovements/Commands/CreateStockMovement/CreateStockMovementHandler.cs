using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.StockMovements.Commands.CreateStockMovement;

public class CreateStockMovementHandler(
    IStockMovementCommandRepository stockMovementCommandRepository,
    IProductCommandRepository productCommandRepository,
    IValidator<CreateStockMovementCommand> validator)
{
    public async Task<CreateStockMovementResponse> HandleAsync(CreateStockMovementCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        Product? product = await productCommandRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException("Ürün bulunamadı.");
        }
        var stockMovement = new StockMovement(command.ProductId, command.Quantity, command.MovementType);
        await stockMovementCommandRepository.AddAsync(stockMovement, cancellationToken);
        return new CreateStockMovementResponse(stockMovement.Id);
    }
}
