namespace StockPilot.Application.Features.ProductSuppliers.Queries.GetSupplierProducts;

public record GetSupplierProductsResponse(Guid Id, string ProductName, decimal PurchasePrice, decimal SalePrice);
