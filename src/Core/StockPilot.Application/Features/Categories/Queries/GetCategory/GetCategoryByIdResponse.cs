namespace StockPilot.Application.Features.Categories.Queries.GetCategory;

public record GetCategoryByIdResponse(string Name, string Description, DateTime CreatedAt, DateTime? UpdatedAt);
