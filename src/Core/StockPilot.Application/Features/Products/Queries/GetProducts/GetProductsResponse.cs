namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public record GetProductsResponse(IEnumerable<GetProductResponse> Items, int Page, int PageSize, int TotalCount, int TotalPages);

public record GetProductResponse(Guid Id, string Name, decimal PurchasePrice, decimal SalePrice, Guid CategoryId, string CategoryName);
