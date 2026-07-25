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
}
