using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;

namespace StockPilot.Persistence.Queries;

public class ProductQueryRepository(IDbConnectionFactory dbConnectionFactory) : IProductQueryRepository
{
    public async Task<GetProductByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        CommandDefinition command = new(
            commandText: "SELECT name as Name, description as Description, purchase_price as PurchasePrice, sale_price as SalePrice FROM products WHERE id = @Id",
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        );
        return await connection.QuerySingleOrDefaultAsync<GetProductByIdResponse>(command);
    }

    public async Task<GetProductsResponse> GetAllAsync(GetProductsQuery productsQuery,
        CancellationToken cancellationToken = default)
    {
        List<string> conditions = [];
        DynamicParameters parameters = new();

        parameters.Add("Offset", (productsQuery.Page - 1) * productsQuery.PageSize);
        parameters.Add("PageSize", productsQuery.PageSize);

        if (!string.IsNullOrWhiteSpace(productsQuery.Name))
        {
            conditions.Add("name = @Name");
            parameters.Add("Name", productsQuery.Name.Trim());
        }
        if (productsQuery.MinSalePrice is not null)
        {
            conditions.Add("sale_price >= @MinSalePrice");
            parameters.Add("MinSalePrice", productsQuery.MinSalePrice.Value);
        }
        if (productsQuery.MaxSalePrice is not null)
        {
            conditions.Add("sale_price <= @MaxSalePrice");
            parameters.Add("MaxSalePrice", productsQuery.MaxSalePrice.Value);
        }

        string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        string query = $"""
            SELECT
                id AS Id,
                name AS Name,
                purchase_price AS PurchasePrice,
                sale_price AS SalePrice
            FROM products
            {whereClause}
            ORDER BY created_at DESC, id DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*)
            FROM products
            {whereClause};
            """;

        CommandDefinition command = new(
            commandText: query,
            parameters: parameters,
            cancellationToken: cancellationToken
        );

        await using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(command);
        IEnumerable<GetProductResponse> items = await multi.ReadAsync<GetProductResponse>();
        int totalCount = await multi.ReadSingleAsync<int>();
        int totalPages = (int)Math.Ceiling((double)totalCount / productsQuery.PageSize);

        return new GetProductsResponse(items, productsQuery.Page, productsQuery.PageSize, totalCount, totalPages);
    }
}
