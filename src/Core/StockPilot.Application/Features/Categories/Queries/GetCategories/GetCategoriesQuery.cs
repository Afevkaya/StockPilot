namespace StockPilot.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery(
    int Page = 1,
    int PageSize = 10,
    string? Name = null,
    string? SortBy = null,
    string? SortDirection = "asc",
    string? Search = null);
