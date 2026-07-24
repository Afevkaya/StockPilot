namespace StockPilot.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Name = null,
    decimal? MinSalePrice = null,
    decimal? MaxSalePrice = null);

