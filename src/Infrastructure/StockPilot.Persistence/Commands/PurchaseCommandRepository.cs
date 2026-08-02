using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Persistence.Commands;

public class PurchaseCommandRepository(IDbConnectionFactory dbConnectionFactory) : IPurchaseCommandRepository
{
    public async Task<bool> AddAsync(Purchase purchase, CancellationToken cancellationToken)
    {
        const string sql = @"
            insert into purchases (id, supplier_id, product_id, quantity, unit_price, purchase_date, created_at)
            select @Id, @SupplierId, @ProductId, @Quantity, @UnitPrice, @PurchaseDate, @CreatedAt
            where exists
            (
                select 1
                from product_suppliers
                where product_id = @ProductId
                and supplier_id = @SupplierId
            );
        ";

        CommandDefinition command = new(sql, purchase, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRow = await connection.ExecuteAsync(command);

        return affectedRow == 1;
    }

    public async Task<bool> AddWithStockMovementAsync(
    Purchase purchase,
    StockMovement stockMovement,
    CancellationToken cancellationToken = default)
    {
        const string sql = """
            WITH inserted_purchase AS
            (
                INSERT INTO purchases
                (
                    id,
                    supplier_id,
                    product_id,
                    quantity,
                    unit_price,
                    purchase_date,
                    created_at
                )
                SELECT
                    @PurchaseId,
                    @SupplierId,
                    @ProductId,
                    @Quantity,
                    @UnitPrice,
                    @PurchaseDate,
                    @PurchaseCreatedAt
                WHERE EXISTS
                (
                    SELECT 1
                    FROM product_suppliers
                    WHERE product_id = @ProductId
                      AND supplier_id = @SupplierId
                )
                RETURNING id
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
                @StockMovementCreatedAt
            FROM inserted_purchase;
            """;

        var parameters = new
        {
            PurchaseId = purchase.Id,
            purchase.SupplierId,
            purchase.ProductId,
            purchase.Quantity,
            purchase.UnitPrice,
            purchase.PurchaseDate,
            PurchaseCreatedAt = purchase.CreatedAt,

            StockMovementId = stockMovement.Id,
            MovementType = (int)stockMovement.MovementType,
            StockMovementCreatedAt = stockMovement.CreatedAt
        };

        var command = new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            cancellationToken: cancellationToken);

        using IDbConnection connection = dbConnectionFactory.CreateConnection();

        int affectedRows = await connection.ExecuteAsync(command);

        return affectedRows == 1;
    }
}
