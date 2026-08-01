namespace StockPilot.Application.Features.Purchases.Queries.GetPurchaseById;

public record GetPurchaseByIdResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string? ProductDescription,
    Guid SupplierId,
    string SupplierName,
    string? ContactName,
    string? Email,
    string? Phone,
    string? Address,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    DateTime PurchaseDate,
    DateTime CreatedAt);
