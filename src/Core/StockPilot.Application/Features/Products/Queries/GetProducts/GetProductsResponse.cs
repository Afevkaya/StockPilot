namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public record GetProductsResponse(Guid Id, string Name, decimal PurchasePrice, decimal SalePrice);
