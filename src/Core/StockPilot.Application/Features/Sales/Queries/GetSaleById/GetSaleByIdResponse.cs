namespace StockPilot.Application.Features.Sales.Queries.GetSaleById;

public record GetSaleByIdResponse(
    Guid SaleId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    DateTime SaleDate,
    DateTime CreatedAt);
