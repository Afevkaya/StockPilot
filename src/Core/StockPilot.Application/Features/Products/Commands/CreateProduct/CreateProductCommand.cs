namespace StockPilot.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal PurchasePrice,
    decimal SalePrice,
    Guid CategoryId
);
