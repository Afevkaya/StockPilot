using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Categories.Queries.GetCategories;
using StockPilot.Application.Features.Categories.Queries.GetCategory;

namespace StockPilot.Persistence.Queries;

public class CategoryQueryRepository(IDbConnectionFactory dbConnectionFactory) : ICategoryQueryRepository
{
    public async Task<GetCategoriesResponse> GetCategoriesAsync(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var whereConditions = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            whereConditions.Add("Name ILIKE @Name");
            parameters.Add("Name", query.Name.Trim());
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            whereConditions.Add("(Name ILIKE @SearchTerm OR Description ILIKE @SearchTerm)");
            parameters.Add("SearchTerm", $"%{query.Search.Trim()}%");
        }

        string whereClause = whereConditions.Count > 0 ? $"WHERE {string.Join(" AND ", whereConditions)}" : string.Empty;

        string sortBy = query.SortBy?.ToLower() switch
        {
            "name" => "name",
            "createdat" => "created_at",
            "updatedat" => "updated_at",
            _ => "name"
        };

        string sortDirection = query.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";

        string sql = $@"
            SELECT id as Id, name as Name, description as Description
            FROM Categories
            {whereClause}
            ORDER BY {sortBy} {sortDirection}
            LIMIT @Limit OFFSET @Offset;

            SELECT COUNT(*) FROM Categories {whereClause};
        ";
        parameters.Add("Limit", query.PageSize);
        parameters.Add("Offset", (query.Page - 1) * query.PageSize);

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        await using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(command);
        IEnumerable<GetCategoryResponse> items = await multi.ReadAsync<GetCategoryResponse>();
        int totalCount = await multi.ReadSingleAsync<int>();
        int totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new GetCategoriesResponse(items, query.Page, query.PageSize, totalCount, totalPages);
    }

    public async Task<GetCategoryByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        string sql = @"
            SELECT name as Name, description as Description, created_at as CreatedAt, updated_at as UpdatedAt
            FROM Categories
            WHERE Id = @Id;
        ";

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<GetCategoryByIdResponse>(command);
    }
}
