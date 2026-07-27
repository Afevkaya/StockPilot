namespace StockPilot.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdResponse(string Name, string? Description, decimal PurchasePrice, decimal SalePrice, Guid CategoryId, string CategoryName);
