using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesHandler(ICategoryQueryRepository repository)
{
    public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        // Implement the logic to retrieve categories based on the query parameters.
        // This is a placeholder implementation. Replace it with actual data retrieval logic.
        return await repository.GetCategoriesAsync(query, cancellationToken);
    }
}
