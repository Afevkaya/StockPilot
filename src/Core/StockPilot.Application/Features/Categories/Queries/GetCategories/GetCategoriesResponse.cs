namespace StockPilot.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesResponse(IEnumerable<GetCategoryResponse> Items, int Page, int PageSize, int TotalCount, int TotalPages);
public record GetCategoryResponse(Guid Id, string Name, string? Description);
