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
            throw new InvalidOperationException("Ürün kayıt işlemi yapılırken bir hata gerçekleşti.");
        }
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        const string query = @"
            UPDATE Products
            SET name = @Name,
                description = @Description,
                purchase_price = @PurchasePrice,
                sale_price = @SalePrice,
                updated_at = @UpdatedAt
            WHERE id = @Id;
        ";
        CommandDefinition command = new(
            commandText: query,
            parameters: product,
            cancellationToken: cancellationToken
        );

        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException($"Ürün güncellenirken bir hata gerçekleşti.");
        }
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        const string query = @"
            SELECT
                id as Id, name as Name, description as Description,
                purchase_price as PurchasePrice, sale_price as SalePrice,
                created_at as CreatedAt, updated_at as UpdatedAt
            FROM Products
            WHERE id = @Id;
        ";
        CommandDefinition command = new(
            commandText: query,
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        );
        return await connection.QuerySingleOrDefaultAsync<Product>(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        const string query = @"
            DELETE FROM Products
            WHERE id = @Id;
        ";
        CommandDefinition command = new(
            commandText: query,
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        );

        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException($"Ürün silme işleminde bir hata gerçekleşti.");
        }
    }
}
