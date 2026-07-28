using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Inventories.Queries.GetAllInventories;
using StockPilot.Application.Features.Inventories.Queries.GetProductInventory;

namespace StockPilot.Persistence.Queries;

public class InventoryQueryRepository(IDbConnectionFactory dbConnectionFactory) : IInventoryQueryRepository
{
    public async Task<GetProductInventoryResponse?> GetProductInventoryById(GetProductInventoryQuery query, CancellationToken cancellationToken = default)
    {
        string sqlQuery = @"
           SELECT
                p.id AS ProductId,
                p.name AS ProductName,
                COALESCE(
                    SUM(
                        CASE
                            WHEN sm.movement_type = 0 THEN sm.quantity
                            WHEN sm.movement_type = 1 THEN -sm.quantity
                            ELSE 0
                        END
                    ),
                    0
                ) AS CurrentStock
            FROM products p
            LEFT JOIN stock_movements sm
                ON sm.product_id = p.id
            WHERE p.id = @ProductId
            GROUP BY p.id, p.name;
        ";

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        connection.Open();

        CommandDefinition command = new(
            sqlQuery,
            new { ProductId = query.ProductId },
            cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<GetProductInventoryResponse>(command);
    }

    public async Task<IEnumerable<GetAllInventoriesResponse>?> GetAllInventories(CancellationToken cancellationToken = default)
    {
        string sqlQuery = @"
            SELECT
                p.id AS ProductId,
                p.name AS ProductName,
                c.name AS CategoryName,
                COALESCE(
                    SUM(
                        CASE
                            WHEN sm.movement_type = 0 THEN sm.quantity
                            WHEN sm.movement_type = 1 THEN -sm.quantity
                            ELSE 0
                        END
                    ),
                    0
                ) AS CurrentStock
            FROM products p
            LEFT JOIN stock_movements sm
                ON sm.product_id = p.id
            left join categories c
                on c.id = p.category_id
            GROUP BY p.id, p.name, c.name;
        ";

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        connection.Open();

        CommandDefinition command = new(sqlQuery, cancellationToken: cancellationToken);
        return await connection.QueryAsync<GetAllInventoriesResponse>(command);
    }
}
