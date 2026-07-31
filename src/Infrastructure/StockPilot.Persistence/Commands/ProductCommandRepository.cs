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
            INSERT INTO Products (id, name, description, purchase_price, sale_price, category_id, created_at)
            VALUES (@Id, @Name, @Description, @PurchasePrice, @SalePrice, @CategoryId, @CreatedAt);
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
                category_id = @CategoryId,
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
                category_id as CategoryId,
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

    public async Task<bool> AssignSupplierAsync(
        Guid productId,
        Guid supplierId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                           INSERT INTO product_suppliers (product_id, supplier_id)
                           VALUES (@ProductId, @SupplierId)
                           ON CONFLICT (product_id, supplier_id) DO NOTHING;
                           """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                ProductId = productId,
                SupplierId = supplierId
            },
            cancellationToken: cancellationToken);

        using IDbConnection connection =
            dbConnectionFactory.CreateConnection();

        int affectedRows = await connection.ExecuteAsync(command);

        return affectedRows == 1;
    }

    public async Task RemoveSupplierAsync(Guid productId, Guid supplierId, CancellationToken cancellationToken = default)
    {
        const string sql = """
                            delete from product_suppliers
                            where supplier_id = @SupplierId
                            and product_id = @ProductId
                         """;

        CommandDefinition command = new(sql, new { ProductId = productId, SupplierId = supplierId },
            cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        int affectedRows = await connection.ExecuteAsync(command);
        if (affectedRows < 1)
        {
            throw new InvalidOperationException("Tedarikçi ürün silme işlemi yaparken hata oluştu.");
        }
    }
}
