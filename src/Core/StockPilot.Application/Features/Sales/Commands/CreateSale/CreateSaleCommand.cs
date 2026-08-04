namespace StockPilot.Application.Features.Sales.Commands.CreateSale;

public record CreateSaleCommand(Guid ProductId, int Quantity, decimal UnitPrice, DateTime SaleDate);
