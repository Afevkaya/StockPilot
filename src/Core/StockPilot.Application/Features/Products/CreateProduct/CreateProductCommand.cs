namespace StockPilot.Application.Features.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal PurchasePrice,
    decimal SalePrice
);
