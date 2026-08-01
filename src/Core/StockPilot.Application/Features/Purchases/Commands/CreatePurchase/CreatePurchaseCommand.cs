namespace StockPilot.Application.Features.Purchases.Commands.CreatePurchase;

public record CreatePurchaseCommand(
    Guid ProductId,
    Guid SupplierId,
    int Quantity,
    decimal UnitPrice,
    DateTime PurchaseDate
);
