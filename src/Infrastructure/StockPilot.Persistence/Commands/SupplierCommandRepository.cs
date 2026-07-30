using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Commands;
using StockPilot.Domain.Entities;

namespace StockPilot.Persistence.Commands;

public class SupplierCommandRepository(IDbConnectionFactory dbConnectionFactory) : ISupplierCommandRepository
{
    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        string sql = @"
            INSERT INTO Suppliers (id, name, contact_name, email, phone, address, created_at)
            VALUES (@Id, @Name, @ContactName, @Email, @Phone, @Address, @CreatedAt);
        ";

        CommandDefinition command = new(sql, supplier, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRows = await connection.ExecuteAsync(command);

        if (affectedRows < 0)
        {
            throw new InvalidOperationException("Tedarikçi eklenirken bir hata oluştu.");
        }
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        string sql = @"
            UPDATE Suppliers
            SET name = @Name,
                contact_name = @ContactName,
                email = @Email,
                phone = @Phone,
                address = @Address,
                updated_at = @UpdatedAt
            WHERE id = @Id;
        ";

        CommandDefinition commandDefinition = new(sql, supplier, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRows = await connection.ExecuteAsync(commandDefinition);
        if (affectedRows < 0)
        {
            throw new InvalidOperationException("Tedarikçi güncellenirken bir hata oluştu.");
        }
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string sql = @"
            SELECT s.id as Id,
                   s.name as Name,
                   s.contact_name as ContactName,
                   s.email as Email,
                   s.phone as Phone,
                   s.created_at as Createdat,
                   s.updated_at as Updatedat
            FROM suppliers s
            WHERE id = @Id
        ";

        CommandDefinition commandDefinition = new(sql, new { Id = id }, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Supplier>(commandDefinition);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string sql = "DELETE FROM suppliers s where id = @Id";
        CommandDefinition commandDefinition = new(sql, new { Id = id }, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRows = await connection.ExecuteAsync(commandDefinition);
        if (affectedRows < 0)
        {
            throw new InvalidOperationException("Tedarikçi silerken hata gerçekleşti");
        }
    }
}
