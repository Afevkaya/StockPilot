namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public record GetProductsResponse(string Name, decimal PurchasePrice, decimal SalePrice);
