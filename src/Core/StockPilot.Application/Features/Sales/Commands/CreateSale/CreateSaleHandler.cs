using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Inventories.Queries.GetProductInventory;
using StockPilot.Domain.Entities;
using StockPilot.Domain.Enums;

namespace StockPilot.Application.Features.Sales.Commands.CreateSale;

public class CreateSaleHandler(
    IProductCommandRepository productCommandRepository,
    IInventoryQueryRepository inventoryQueryRepository,
    ISaleCommandRepository saleCommandRepository,
    IValidator<CreateSaleCommand> validator)
{
    public async Task<CreateSaleResponse> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var product = await productCommandRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            throw new ArgumentException($"Product with ID {command.ProductId} not found.");
        }

        var inventor = await inventoryQueryRepository.GetProductInventoryById(new GetProductInventoryQuery(product.Id), cancellationToken);

        if (inventor is null || inventor.CurrentStock < command.Quantity)
        {
            throw new InvalidOperationException($"Yetersiz stok. Mevcut stok: {inventor?.CurrentStock ?? 0}, Talep edilen miktar: {command.Quantity}");
        }

        Sale sale = new(command.ProductId, command.Quantity, command.UnitPrice, command.SaleDate);
        StockMovement stockMovement = new(product.Id, command.Quantity, MovementType.StockOut);
        bool isAdded = await saleCommandRepository.AddWithStockMovementAsync(sale, stockMovement, cancellationToken);

        if (!isAdded)
        {
            throw new InvalidOperationException("Sistemsel bir hata meydana geldi");
        }

        return new CreateSaleResponse(sale.Id);
    }
}
