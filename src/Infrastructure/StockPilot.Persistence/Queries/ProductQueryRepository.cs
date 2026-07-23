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

    public async Task<IEnumerable<GetProductsResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        CommandDefinition command = new(
            commandText: "SELECT name as Name, purchase_price as PurchasePrice, sale_price as SalePrice FROM products ORDER BY created_at DESC",
            cancellationToken: cancellationToken
        );
        return await connection.QueryAsync<GetProductsResponse>(command);
    }
}
