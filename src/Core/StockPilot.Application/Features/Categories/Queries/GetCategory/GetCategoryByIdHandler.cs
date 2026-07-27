using StockPilot.Application.Abstractions.Persistence.Queries;

namespace StockPilot.Application.Features.Categories.Queries.GetCategory;

public class GetCategoryByIdHandler(ICategoryQueryRepository categoryQueryRepository)
{
    public async Task<GetCategoryByIdResponse?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        GetCategoryByIdResponse? category = await categoryQueryRepository.GetByIdAsync(query.Id, cancellationToken);

        if (category is null)
        {
            return null;
        }

        return category;
    }
}
