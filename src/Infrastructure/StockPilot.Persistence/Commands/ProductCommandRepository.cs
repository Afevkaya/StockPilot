using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Persistence.Commands;

public class ProductCommandRepository(IDbConnectionFactory dbConnectionFactory) : IProductCommandRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        const string query = @"
            INSERT INTO Products (id, name, description, purchase_price, sale_price, created_at)
            VALUES (@Id, @Name, @Description, @PurchasePrice, @SalePrice, @CreatedAt);
        ";
        CommandDefinition command = new(
            commandText: query,
            parameters: product,
            cancellationToken: cancellationToken
        );

        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException("Failed to insert Product into database.");
        }
    }
}
