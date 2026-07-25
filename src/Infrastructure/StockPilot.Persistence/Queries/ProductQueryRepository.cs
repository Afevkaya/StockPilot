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
        var builder = new SqlBuilder();

        if (!string.IsNullOrWhiteSpace(productsQuery.Name))
        {
            builder.Where("name LIKE @Name", new { Name = productsQuery.Name.Trim() });
        }

        if (productsQuery.MinSalePrice is not null)
        {
            builder.Where("sale_price >= @MinSalePrice", new { productsQuery.MinSalePrice });
        }

        if (productsQuery.MaxSalePrice is not null)
        {
            builder.Where("sale_price <= @MaxSalePrice", new { productsQuery.MaxSalePrice });
        }

        if (!string.IsNullOrWhiteSpace(productsQuery.Search))
        {
            builder.Where("(name ILIKE @Search OR description ILIKE @Search)", new { Search = $"%{productsQuery.Search.Trim()}%" });
        }

        string sortBy = productsQuery.SortBy?.ToLower() switch
        {
            "name" => "name",
            "saleprice" => "sale_price",
            "purchaseprice" => "purchase_price",
            "createdat" => "created_at",
            _ => "name"
        };

        string sortDirection = productsQuery.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";
        builder.OrderBy(sortBy, sortDirection);

        SqlBuilder.Template template = builder.AddTemplate(@"
        SELECT
            id AS Id,
            name AS Name,
            purchase_price AS PurchasePrice,
            sale_price AS SalePrice
        FROM products
        /**where**/
        /**orderby**/
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(*)
        FROM products
        /**where**/;");

        var parameters = new DynamicParameters(template.Parameters);
        parameters.Add("Offset", (productsQuery.Page - 1) * productsQuery.PageSize);
        parameters.Add("PageSize", productsQuery.PageSize);

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        var command = new CommandDefinition(
            commandText: template.RawSql,
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
