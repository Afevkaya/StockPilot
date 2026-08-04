using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Persistence.Commands;

public class SaleCommandRepository(IDbConnectionFactory dbConnectionFactory) : ISaleCommandRepository
{
    public async Task<bool> AddWithStockMovementAsync(Sale sale, StockMovement stockMovement, CancellationToken cancellationToken = default)
    {
        const string sql = """
            WITH inserted_sale AS
            (
                INSERT INTO sales
                (
                    id,
                    product_id,
                    quantity,
                    unit_price,
                    sale_date,
                    created_at
                )
                SELECT
                    @SaleId,
                    @ProductId,
                    @Quantity,
                    @UnitPrice,
                    @SaleDate,
                    @CreatedAt
            )
            INSERT INTO stock_movements
            (
                id,
                product_id,
                quantity,
                movement_type,
                created_at
            )
            SELECT
                @StockMovementId,
                @ProductId,
                @Quantity,
                @MovementType,
                @StockMovementCreatedAt;
        """;

        var parameters = new
        {
            SaleId = sale.Id,
            sale.ProductId,
            sale.Quantity,
            sale.UnitPrice,
            sale.SaleDate,
            sale.CreatedAt,

            StockMovementId = stockMovement.Id,
            MovementType = (int)stockMovement.MovementType,
            StockMovementCreatedAt = stockMovement.CreatedAt
        };

        CommandDefinition command = new(sql, parameters, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRow = await connection.ExecuteAsync(command);
        return affectedRow == 1;
    }
}
