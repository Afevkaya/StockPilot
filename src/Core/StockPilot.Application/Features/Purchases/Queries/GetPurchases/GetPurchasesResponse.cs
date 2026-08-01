namespace StockPilot.Application.Features.Purchases.Queries.GetPurchases;

public record GetPurchasesResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    Guid SupplierId,
    string SupplierName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    DateTime PurchaseDate,
    DateTime CreatedAt);
