using FluentValidation;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Application.Features.Purchases.Commands.CreatePurchase;

public class CreatePurchaseHandler(
    IPurchaseCommandRepository purchaseCommandRepository,
    IProductCommandRepository productCommandRepository,
    ISupplierCommandRepository supplierCommandRepository,
    IValidator<CreatePurchaseCommand> validator)
{
    public async Task<CreatePurchaseResponse> Handle(CreatePurchaseCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var product = await productCommandRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Ürün bulunamadı. Id: {command.ProductId}");
        }

        var supplier = await supplierCommandRepository.GetByIdAsync(command.SupplierId, cancellationToken);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Tedarikçi bulunamadı. Id: {command.SupplierId}");
        }

        var purchase = new Domain.Entities.Purchase(
            command.ProductId,
            command.SupplierId,
            command.Quantity,
            command.UnitPrice,
            command.PurchaseDate);

        var stockMovement = new StockMovement(
            command.ProductId,
            command.Quantity,
            Domain.Enums.MovementType.StockIn);

        bool isAdded =
            await purchaseCommandRepository.AddWithStockMovementAsync(purchase, stockMovement, cancellationToken);

        if (!isAdded)
        {
            throw new InvalidOperationException("Ürün belirtilen tedarikçi ile ilişkilendirilmemiştir.");

        }
        return new CreatePurchaseResponse(purchase.Id);
    }
}
