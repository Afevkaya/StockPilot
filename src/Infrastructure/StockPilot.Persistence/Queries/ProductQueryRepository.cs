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

    public async Task<GetProductsResponse> GetAllAsync(int page = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        string query = @"SELECT
                            id as Id, name as Name,
                            purchase_price as PurchasePrice, sale_price as SalePrice
                        FROM products
                        ORDER BY created_date desc, id desc
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                        SELECT COUNT(*) FROM products;";

        CommandDefinition command = new(
            commandText: query,
            parameters: new { Offset = (page - 1) * pageSize, PageSize = pageSize },
            cancellationToken: cancellationToken
        );

        await using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(command);
        IEnumerable<GetProductResponse> items = multi.Read<GetProductResponse>();
        int totalCount = multi.ReadSingle<int>();
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new GetProductsResponse(items, page, pageSize, totalCount, totalPages);
    }
}
