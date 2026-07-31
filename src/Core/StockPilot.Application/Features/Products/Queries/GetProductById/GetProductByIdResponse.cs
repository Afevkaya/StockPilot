namespace StockPilot.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdResponse(
    string Name,
    string? Description,
    decimal PurchasePrice,
    decimal SalePrice,
    Guid CategoryId,
    string CategoryName,
    IEnumerable<GetSuppliersByProduct> Suppliers);

public record GetSuppliersByProduct(
    Guid Id,
    string Name,
    string? Email,
    string? Phone);

