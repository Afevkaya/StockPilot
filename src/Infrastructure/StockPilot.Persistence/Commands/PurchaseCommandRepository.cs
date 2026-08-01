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
}
