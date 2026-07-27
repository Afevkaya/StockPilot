using StockPilot.Application.Features.Categories.Queries.GetCategories;
using StockPilot.Application.Features.Categories.Queries.GetCategory;

namespace StockPilot.Application.Abstractions.Persistence.Queries;

public interface ICategoryQueryRepository
{
    Task<GetCategoriesResponse> GetCategoriesAsync(GetCategoriesQuery query, CancellationToken cancellationToken);
    Task<GetCategoryByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
