namespace StockPilot.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductResponse(Guid Id, string Name, string? Description, decimal PurchasePrice, decimal SalePrice);
