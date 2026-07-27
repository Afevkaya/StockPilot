using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Persistence.Commands;

public class CategoryCommandRepository(IDbConnectionFactory dbConnectionFactory) : ICategoryCommandRepository
{
    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        const string query = @"
            INSERT INTO Categories (id, name, description, created_at)
            VALUES (@Id, @Name, @Description, @CreatedAt);
        ";
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        CommandDefinition command = new(
            commandText: query,
            parameters: category,
            cancellationToken: cancellationToken
        );
        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException("Kategori kayıt işlemi yapılırken bir hata gerçekleşti.");
        }
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        const string query = @"
            UPDATE Categories
            SET name = @Name,
                description = @Description,
                updated_at = @UpdatedAt
            WHERE id = @Id;
        ";
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        CommandDefinition command = new(
            commandText: query,
            parameters: category,
            cancellationToken: cancellationToken
        );
        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException("Kategori güncelleme işlemi yapılırken bir hata gerçekleşti.");
        }
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query = @"
            SELECT id as Id, name as Name, description as Description, updated_at as UpdatedAt
            FROM Categories
            WHERE id = @Id;
        ";
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        CommandDefinition command = new(
            commandText: query,
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        );
        return await connection.QuerySingleOrDefaultAsync<Category>(command);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
    {
        const string query = @"
            DELETE FROM Categories
            WHERE id = @Id;
        ";
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        CommandDefinition command = new(
            commandText: query,
            parameters: new { Id = category.Id },
            cancellationToken: cancellationToken
        );
        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException("Kategori silme işlemi yapılırken bir hata gerçekleşti.");
        }
    }
}
