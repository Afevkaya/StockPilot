namespace StockPilot.Application.Features.Sales.Queries.GetSales;

public record GetSalesResponse(int Page, int PageSize, int TotalCount, int TotalPages, IEnumerable<GetSaleResponse> Items);
public record GetSaleResponse(
    Guid SaleId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    DateTime SaleDate,
    DateTime CreatedAt);
