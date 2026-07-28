using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Persistence.Commands;

public class StockMovementCommandCommandRepository(IDbConnectionFactory dbConnectionFactory) : IStockMovementCommandRepository
{
    public async Task AddAsync(StockMovement stockMovement, CancellationToken cancellationToken = default)
    {
        string sql = @"
            INSERT INTO stock_movements (id, product_id, quantity, movement_type, created_at)
            VALUES (@Id, @ProductId, @Quantity, @MovementType, @CreatedAt);
        ";

        CommandDefinition command = new(parameters: stockMovement, commandText: sql, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException("Stok hareketi eklenirken bir hata oluştu.");
        }
    }
}
