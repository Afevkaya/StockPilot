using System.Text.Json.Serialization;

namespace StockPilot.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    [property: JsonIgnore]
    Guid Id,
    string Name,
    string? Description,
    decimal PurchasePrice,
    decimal SalePrice,
    Guid CategoryId);
