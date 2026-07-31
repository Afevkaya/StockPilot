namespace StockPilot.Application.Features.ProductSuppliers.Queries.GetProductSuppliers;

public record GetProductSuppliersResponse(Guid Id, string Name, string? Email, string? Phone);
